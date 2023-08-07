using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class HiToonEditor : ShaderGUI
{

    private enum HeightGradientType
    {
        WorldSpace,
        ObjectSpace
    }

    private HeightGradientType _heightGradientType;

    // made static to hold foldout states globally
    private static Dictionary<string, bool> _foldoutStates = new Dictionary<string, bool>{
        {"ShadeColorOverride", false},
        {"Specular", false},
        {"Rim", false},
        {"Backlight", false},
        {"HeightGradient", false},
        {"Emission", false},
        {"Normal", false}
    };

    protected void DrawStandard(MaterialEditor editor, MaterialProperty property)
    {
        string displayName = property.displayName;
        // Remove everything in square brackets.
        displayName = Regex.Replace(displayName, @" ?\[.*?\]", string.Empty);
        var guiContent = new GUIContent(displayName);
        editor.ShaderProperty(property, guiContent);
    }

    protected void DrawRange(MaterialEditor editor, MaterialProperty property, float min, float max)
    {
        var value = property.floatValue;

        value = EditorGUILayout.Slider(property.displayName, value, min, max);

        property.floatValue = value;
    }


    public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
    {
        if (editor.targets.Length > 1)
        {
            EditorGUILayout.HelpBox("Multi-object editing with HiToonEditor is not supported yet.", MessageType.Info);
            base.OnGUI(editor, properties);
            return;
        }
        var material = editor.target as Material;

        // draw main properties


        DrawStandard(editor, FindProperty("_BaseColor", properties));
        DrawStandard(editor, FindProperty("_BaseMap", properties));
        DrawRange(editor, FindProperty("_LightAffection", properties), 0, 1);
        DrawRange(editor, FindProperty("_Shade", properties), 0, 1);
        DrawRange(editor, FindProperty("_ShadeDistance", properties), -1, 1);
        DrawStandard(editor, FindProperty("_ShadeSmoothness", properties));


        //DrawStandard(editor, FindProperty("_LightAffection", properties));

        //draw keyword properties
        DrawKeywordGroup("Shade Color Override", "_OVERRIDE_SHADE_COLOR", "ShadeColorOverride", editor, properties, "_ShadeColorOverride");
        DrawKeywordGroup("Emission", "_ENABLE_EMISSION", "Emission", editor, properties, "_EmissionMap", "_EmissionColor");
        DrawKeywordGroup("Specular", "_ENABLE_SPECULAR", "Specular", editor, properties, "_SpecularColor", "_SpecularSize", "_Smoothness");
        DrawKeywordGroup("Rim Lighting", "_ENABLE_RIM", "Rim", editor, properties, "_RimColor", "_Rim", "_RimMultiplier");
        DrawKeywordGroupWithCallback("Back Lighting", "_ENABLE_BACKLIGHT", "Backlight", editor, properties, 
            () => EditorGUILayout.HelpBox("Color alpha affects the intesity", MessageType.Info),
            null,
            null,
            "_BacklightColor", "_BacklightOffset");
        
        DrawKeywordGroup("Normal Map", "_ENABLE_NORMAL_MAP", "Normal", editor, properties, "_NormalMap");

        DrawKeywordGroupWithCallback("Height Gradient", "_ENABLE_HEIGHT_GRADIENT", "HeightGradient", editor, properties,
            () => EditorGUILayout.HelpBox("Color alpha affects the intesity", MessageType.Info),
            () => DrawHeightGradientType(editor, properties),
            null,
            "_HeightGradientStartColor", "_HeightGradientStartPosition", "_HeightGradientEndPosition");

        DrawStandard(editor, FindProperty("_ReceivedShadowsEnabled", properties));

        //DrawKeywords(materialEditor, properties);
    }

    private void DrawHeightGradientType(MaterialEditor editor, MaterialProperty[] properties)
    {
        var material = editor.target as Material;
        var property = FindProperty("_HeightGradientType", properties);
        Debug.Log(FindProperty("_HeightGradientType", properties).floatValue);
        property.floatValue = (float)(HeightGradientType)EditorGUILayout.EnumPopup((HeightGradientType)(int)property.floatValue);
        switch ((int)property.floatValue)
        {
            case 0:
                material.DisableKeyword("_HEIGHTGRADIENTTYPE_OBJECTSPACE");
                material.EnableKeyword("_HEIGHTGRADIENTTYPE_WORLDSPACE");
                break;
            case 1:
                material.EnableKeyword("_HEIGHTGRADIENTTYPE_OBJECTSPACE");
                material.DisableKeyword("_HEIGHTGRADIENTTYPE_WORLDSPACE");
                break;
        }
    }

    private void DrawKeywordGroup(string groupName, string keywordName, string foldoutState, MaterialEditor editor, MaterialProperty[] properties, params string[] targetProperties)
    {
        var material = editor.target as Material;
        _foldoutStates[foldoutState] = DrawKeywordFoldout(groupName, keywordName, editor, _foldoutStates[foldoutState]);
        if (material.IsKeywordEnabled(keywordName))
        {
            if (_foldoutStates[foldoutState])
            {
                EditorGUI.indentLevel++;
                foreach (var propertyName in targetProperties)
                {
                    DrawStandard(editor, FindProperty(propertyName, properties));
                }
                EditorGUI.indentLevel--;
            }
        }
    }
    private void DrawKeywordGroupWithCallback(string groupName, string keywordName, string foldoutState, MaterialEditor editor, MaterialProperty[] properties, Action preEditorCallback, Action postEditorCallback, Action<MaterialProperty> perPropertyCallback, params string[] targetProperties)
    {
        var material = editor.target as Material;
        _foldoutStates[foldoutState] = DrawKeywordFoldout(groupName, keywordName, editor, _foldoutStates[foldoutState]);
        if (material.IsKeywordEnabled(keywordName))
        {
            if (_foldoutStates[foldoutState])
            {
                EditorGUI.indentLevel++;
                preEditorCallback?.Invoke();
                foreach (var propertyName in targetProperties)
                {
                    var property = FindProperty(propertyName, properties);
                    perPropertyCallback?.Invoke(property);
                    DrawStandard(editor, property);
                }
                postEditorCallback?.Invoke();
                EditorGUI.indentLevel--;
            }
        }
    }

    private bool DrawKeywordFoldout(string name, string keywordName, MaterialEditor editor, bool toggle)
    {
        var target = editor.target as Material;
        bool propertyEnabled = target.IsKeywordEnabled(keywordName);

        EditorGUILayout.BeginHorizontal();
        propertyEnabled = EditorGUILayout.Toggle(propertyEnabled, GUILayout.ExpandWidth(false), GUILayout.Width(24));
        EditorGUILayout.Space(4, false);
        var ret = EditorGUILayout.Foldout(toggle, name);
        EditorGUILayout.EndHorizontal();

        foreach (Material material in editor.targets)
        {
            if (propertyEnabled)
            {
                material.EnableKeyword(keywordName);
            }
            else
                material.DisableKeyword(keywordName);
        }


        return ret && propertyEnabled;
    }

    private void DrawKeywords(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        foreach (var property in properties)
        {
            if (property.name.Contains("Enable") || property.name.Contains("Override") && property.type == MaterialProperty.PropType.Float)
            {
                DrawStandard(materialEditor, property);
            }
        }
    }

    private bool ShouldSkipProperty(MaterialProperty property)
    {
        return property.displayName.StartsWith("__");
    }

    private struct FoldoutProperty
    {
        public string Category;
        public string FeatureName;
        public MaterialProperty Property;
        public FoldoutProperty(string category, string featureName, MaterialProperty property)
        {
            Category = category;
            FeatureName = featureName;
            Property = property;
        }
    }
}