using UnityEngine;
using UnityEditor;
using System.Collections;

public class Builder : ScriptableObject
{
	public static string lastAndroidBuildName;
	
	[MenuItem ("Mobility Games/Build/Build")]
	public static void BuildProject() 
	{
		bool abortBuild = false;
		
		if ( System.IO.Directory.Exists(Application.dataPath + "/Scripts/Profiler") && 
			!System.IO.Directory.Exists(Application.dataPath + "/Editor/Profiler")) {
			System.IO.Directory.Move(Application.dataPath + "/Scripts/Profiler", Application.dataPath + "/Editor/Profiler");
		}

		if ( System.IO.Directory.Exists(Application.dataPath + "/Scripts/Profiler.meta") && 
			!System.IO.Directory.Exists(Application.dataPath + "/Editor/Profiler.meta")) {
			System.IO.File.Move(Application.dataPath + "/Scripts/Profiler.meta", Application.dataPath + "/Editor/Profiler.meta");
		}

		AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		string[] scenes = new string[EditorBuildSettings.scenes.Length];
		for (int i = 0; i < scenes.Length; ++i) {
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}
		
		
#if UNITY_ANDROID
		EditorUserBuildSettings.appendProject = false;
		
		string savePath = EditorUtility.SaveFilePanel("Build " + BuildTarget.Android, 
													   EditorUserBuildSettings.GetBuildLocation(BuildTarget.Android), "", "apk");
	  	if(savePath.Length != 0) {
			// Reset build location and name
//	    	string dir = System.IO.Path.GetDirectoryName(savePath);
			EditorUserBuildSettings.SetBuildLocation(BuildTarget.Android, savePath);
		} else {
			abortBuild = true;
		}
#else
		EditorUserBuildSettings.appendProject = true;
#endif
		if ( !abortBuild ) {
			UpdateBuildNumber();
			
			BuildPipeline.BuildPlayer(scenes, EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget), 
			                          EditorUserBuildSettings.activeBuildTarget, BuildOptions.AcceptExternalModificationsToPlayer);
		}
		
		//TODO: this is project specific. We should find a way to externalize it from this generic build tool.
		System.IO.Directory.Move(Application.dataPath + "/Editor/Profiler", Application.dataPath + "/Scripts/Profiler");
		System.IO.File.Move(Application.dataPath + "/Editor/Profiler.meta", Application.dataPath + "/Scripts/Profiler.meta");
		AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
}
	
	private static void UpdateBuildNumber() 
	{
		string currentDate = "" + System.DateTime.Now.Year + "." + System.DateTime.Now.Month.ToString("00") + "." + System.DateTime.Now.Day.ToString("00");
		TextAsset txt = (TextAsset)Resources.LoadAssetAtPath("Assets/StreamingAssets/build_number.txt", typeof(TextAsset));
		
		string val = "";
		if (txt == null) 
		{
			val = currentDate + ".1";
		} 
		else 
		{
			System.IO.StringReader reader = new System.IO.StringReader(txt.text);
			val = reader.ReadLine();
			reader.Close();
			
			
			if (val.Contains(currentDate)) {
				int buildNumber = int.Parse(val.Replace(currentDate + ".", ""));
				val = currentDate + "." + (buildNumber + 1);
			} else {
				val = currentDate + ".1";
			}
		}
		
		//write the val
		System.IO.TextWriter writer = System.IO.File.CreateText("Assets/StreamingAssets/build_number.txt");
		writer.Write(val);
		writer.Close();
		AssetDatabase.ImportAsset("Assets/StreamingAssets/build_number.txt", ImportAssetOptions.ForceUpdate);
	}
}

