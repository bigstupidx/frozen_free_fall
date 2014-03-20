using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ResourceExport : ScriptableObject {
		
	[MenuItem ("Mobility Games/AssetBundles/Export Res Files From Resources")]
	public static void BuildAssetBundleFromResourcesSelection() {
		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		string[] assetNames = new string[selection.Length];
		Debug.Log("Creating asset bundle containing: ");
		for(int i = 0; i < selection.Length; i++) {
			string fullPath = AssetDatabase.GetAssetPath(selection[i]);
			int idx = fullPath.LastIndexOf('.');
			if (idx > 0) {
				fullPath = fullPath.Remove(idx);
			}
			assetNames[i] = fullPath.Substring(fullPath.IndexOf("Resources/") + "Resources/".Length);
			Debug.Log(assetNames[i]);
		}
		
		if (selection.Length > 0) {
			string savePath = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");
		 	if (savePath.Length != 0) {
				BuildPipeline.BuildAssetBundleExplicitAssetNames(selection, assetNames, savePath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
				                               EditorUserBuildSettings.activeBuildTarget);
				
			} 
		}
	}
	
//	[MenuItem ("Assets/Update External Resources List")]
//	public static void UpdateExternalResourcesList() {
//		string savePath = EditorUtility.SaveFolderPanel("Select Resource Folder", "here", "here");
//		if (savePath.Length == 0) {
//			return;
//		}
//		
//		string[] dirs = Directory.GetDirectories(savePath);
//		string[] files = Directory.GetFiles(savePath);
//		for(int i = 0; i < dirs.Length; i++) {
//			Debug.Log(dirs[i]);
//		}
//		for(int i = 0; i < files.Length; i++) {
//			Debug.Log(files[i]);
//		}
//	}
	
	
	[MenuItem ("Mobility Games/AssetBundles/Export Single Files From Resources")]
	public static void BuildSingleAssetBundleFromResourcesSelection() {
		//TODO: make this as a parameter for this function and spawn another menu item
		bool skipExistingFiles = true;
		
		string savePath = EditorUtility.SaveFolderPanel("Save Bundles", "here", "here");
		if (savePath.Length == 0) {
			return;
		}
		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		if (selection.Length == 0) {
			return;
		}
		string[] assetNames = new string[selection.Length];
		Debug.Log("Creating asset bundles for " + assetNames.Length + " assets... ");
		
		List<Object> toInclude = new List<Object>();
		List<string> downloadList = new List<string>();
		
		for(int i = 0; i < selection.Length; i++) {
			string fullPath = AssetDatabase.GetAssetPath(selection[i]);
			// remove the Assets/ folder name from the path name because it's contained in the Application.dataPath folder name
			assetNames[i] = fullPath.Substring(fullPath.IndexOf("Assets/") + "Assets/".Length);	
			
			// check if this asset is just a folder
			if ( (File.GetAttributes(Application.dataPath + "/" + assetNames[i]) & FileAttributes.Directory) == FileAttributes.Directory) {
				// create asset bundles folder structure for the assets that are just folders
				Directory.CreateDirectory(savePath + "/" + assetNames[i]);
			} else {
				string assetDir = savePath + "/" + assetNames[i].Remove(assetNames[i].LastIndexOf('/'));
				if ( !Directory.Exists(assetDir) ) {
					Directory.CreateDirectory(assetDir);
				}
				
				// load the asset that is not a folder
//				Object[] assets = EditorUtility.CollectDependencies(new Object[]{ selection[i] });
				toInclude.Clear();
				Object[] assets = AssetDatabase.LoadAllAssetsAtPath("Assets/" + assetNames[i]);
				Object mainAsset = AssetDatabase.LoadMainAssetAtPath("Assets/" + assetNames[i]);
				if (assets.Length > 1) {
					Debug.Log("Loaded " + assets.Length + " dependencies for: " + assetNames[i] + " with main asset: " + mainAsset.name);
					for(int j = 0; j < assets.Length; j++) {
						if (assets[j] != mainAsset) {
							Debug.Log("-> asset: " + assets[j].name);
							toInclude.Add(assets[j]);
						} else {
							Debug.Log("Avoided adding the main asset twice!!!");
						}
					}
				} else if (assets.Length > 0 && assets[0] != mainAsset) {
					toInclude.Add(assets[0]);
				} else if (assets.Length == 0 && mainAsset == null) {
					continue;
				}
				
				// remove file extension from the asset path
				int idx = assetNames[i].LastIndexOf('.');
				if (idx > 0) {
					assetNames[i] = assetNames[i].Remove(idx);
				}
				try {
					// add to file download list
					downloadList.Add(assetNames[i] + ".assetbundle");
					
					if (skipExistingFiles && !File.Exists(savePath + "/" + assetNames[i] + ".assetbundle") || !skipExistingFiles) {
						BuildPipeline.BuildAssetBundle(mainAsset, toInclude.ToArray(), savePath + "/" + assetNames[i] + ".assetbundle", BuildAssetBundleOptions.CollectDependencies
						                               | BuildAssetBundleOptions.CompleteAssets, EditorUserBuildSettings.activeBuildTarget);
					}
				} catch(UnityException) {
					Debug.Log("Error while processing asset: " + assetNames[i]);
				}
			}
		}
		
		// create the file download list
		FileStream fs = File.Create(savePath + "/resources.lst");
		StreamWriter sw = new StreamWriter(fs);
		sw.WriteLine(downloadList.Count);
		for(int i = 0; i < downloadList.Count; i++) {
			sw.WriteLine(downloadList[i]);
		}
		sw.Close();
		fs.Close();
		
//				BuildPipeline.BuildAssetBundle(null, new Object[] {}, savePath, BuildAssetBundleOptions.CompleteAssets, EditorUserBuildSettings.activeBuildTarget);
				
//				BuildPipeline.BuildAssetBundleExplicitAssetNames(selection, assetNames, savePath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
//				                               EditorUserBuildSettings.activeBuildTarget);		
	}

//	[MenuItem ("Assets/Export Resource Files")]
//	public static void BuildAssetBundleFromSelection() {
//		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
//		string[] assetNames = new string[selection.Length];
//		Debug.Log("Creating asset bundle containing: ");
//		for(int i = 0; i < selection.Length; i++) {
//			string fullPath = AssetDatabase.GetAssetPath(selection[i]);
//			int idx = fullPath.LastIndexOf('.');
//			if (idx > 0) {
//				fullPath = fullPath.Remove(idx);
//			}
//			assetNames[i] = fullPath.Substring(fullPath.IndexOf("Assets/") + "Assets/".Length);
//			Debug.Log(assetNames[i]);
//		}
//		
//		if (selection.Length > 0) {
//			string savePath = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");
//		 	if (savePath.Length != 0) {
//				BuildPipeline.BuildAssetBundleExplicitAssetNames(selection, assetNames, savePath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
//				                               EditorUserBuildSettings.activeBuildTarget);
//				
//			} 
//		}
//	}

	
	//TODO: allow only files with *.unity extension!
	[MenuItem ("Mobility Games/AssetBundles/Export Scene Files")]
	public static void BuildStreamedScenesFromSelection() {
		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		if (selection.Length > 0) {
			string savePath = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");
		 	if (savePath.Length != 0) {
				string[] scenes = new string[selection.Length];
				for(int i = 0; i < scenes.Length; i++) {
					scenes[i] = AssetDatabase.GetAssetPath(selection[i]);
					Debug.Log("Preparing to export scenes: " + scenes[i]);
				}
				
				BuildPipeline.BuildPlayer(scenes, savePath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.BuildAdditionalStreamedScenes);
			}
		}
	}	
}
