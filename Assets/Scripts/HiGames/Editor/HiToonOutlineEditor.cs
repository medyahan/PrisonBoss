using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HiToonOutlineEditor : HiToonEditor
{
    public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
    {
        base.OnGUI(editor, properties);
        EditorGUILayout.LabelField("Outline");
        EditorGUI.indentLevel++;
        
        
        DrawStandard(editor, FindProperty("_OutlineColor", properties));
        DrawStandard(editor, FindProperty("_OutlineWidth", properties));

        EditorGUI.indentLevel--;
    }
}
