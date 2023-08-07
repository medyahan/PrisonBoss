using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

public class ScreenShooterWindow : EditorWindow
{

	public enum Resolution
	{
		Portrait_2048x2732_12_9i,
		Portrait_1242x2688_6_5i,
		Custom
	}
	// Add menu named "My Window" to the Window menu
	[MenuItem("Hi/Screenshot")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		ScreenShooterWindow window = (ScreenShooterWindow)EditorWindow.GetWindow(typeof(ScreenShooterWindow));
		window.Show();
	}

	private Vector2 customResolution;
	void OnGUI()
	{
		if (GUILayout.Button("Take"))
		{
			var i = Guid.NewGuid().ToString("N");
			Directory.CreateDirectory(Application.dataPath+"/Screenshots/");
			CaptureScreenshot(i + ".png");
		}
	}

	private void CaptureScreenshot(string filename)
	{
		ScreenCapture.CaptureScreenshot(Application.dataPath+"/Screenshots/"+filename);
		/*await Task.Delay(1000);
		AssetDatabase.ImportAsset("Screenshots/"+filename);*/
	}
}
