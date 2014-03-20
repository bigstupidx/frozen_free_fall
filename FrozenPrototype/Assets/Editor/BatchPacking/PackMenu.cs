using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class PackMenu : MonoBehaviour 
{	
	static string DATA_PATH = "/Users/JianYu/Desktop/Share/";
	static string SIGN_FILE_PATH = "/Users/JianYu/Desktop/Info/AndroidSign/";
	static string ANDROID_SDK_PATH = "/Users/JianYu/Desktop/SourceCode/android-sdk/";
	
	public static string BUNDLE_VERSION = AppSettings.frontEndVersion;
	static int BUNDLE_VERSION_CODE = 7;
	
	static string APP_NAME = "Frozen";
	
	const string DIANXIN_ZIYOU = "DIANXIN_ZIYOU";
	const string DIANXIN_WAIFANG = "DIANXIN_WAIFANG";
	const string LIANTONG = "LIANTONG";
	const string YIDONG_MM_SHENHE = "YIDONG_MM_SHENHE";
	const string YIDONG_MM_ZIQIANMING = "YIDONG_MM_ZIQIANMING";
	const string YIDONG_MDO = "YIDONG_MDO";
	const string TEST = "TEST";
	
	[MenuItem ("Custom Build/Android All")]
	static void PerformAndroidBuildAll()
	{		
		PerformAndroidBuild1();
		PerformAndroidBuild2();
		PerformAndroidBuild3();
		PerformAndroidBuild4();
		PerformAndroidBuild5();
		PerformAndroidBuild6();
	}
	
	[MenuItem ("Custom Build/Android DianXin ZiYou")]
	static void PerformAndroidBuild1()
	{		
		BulidTarget(DIANXIN_ZIYOU, "Android");
	}
	
	[MenuItem ("Custom Build/Android DianXin WaiFang")]
	static void PerformAndroidBuild2()
	{		
		BulidTarget(DIANXIN_WAIFANG, "Android");
	}
	
	[MenuItem ("Custom Build/Android LianTong")]
	static void PerformAndroidBuild3()
	{		
		BulidTarget(LIANTONG, "Android");
	}
	
	[MenuItem ("Custom Build/Android MDO")]
	static void PerformAndroidBuild4()
	{		
		BulidTarget(YIDONG_MDO, "Android");
	}
	
	[MenuItem ("Custom Build/Android MM ShenHe")]
	static void PerformAndroidBuild5()
	{		
		BulidTarget(YIDONG_MM_SHENHE, "Android");
	}
	
	[MenuItem ("Custom Build/Android MM ZiQianMing")]
	static void PerformAndroidBuild6()
	{		
		BulidTarget(YIDONG_MM_ZIQIANMING, "Android");
	}
	
	[MenuItem ("Custom Build/Android Test")]
	static void PerformAndroidBuild7()
	{		
		BulidTarget(TEST, "Android");
	}
	
	[MenuItem ("Custom Build/Android All")]
	static void PerformIosBuild()
	{
		BulidTarget("ios", "IOS");
	}
	
	static void BulidTarget(string platform, string target)
    {	
        string target_dir = getDataFolder();
        string target_name = APP_NAME + ".apk";
        BuildTargetGroup targetGroup = BuildTargetGroup.Android;
        BuildTarget buildTarget = BuildTarget.Android;
 
        if (target == "Android")
        {
            target_name = getApkName(platform);
            targetGroup = BuildTargetGroup.Android;
			PlayerSettings.Android.bundleVersionCode = BUNDLE_VERSION_CODE;
        }
        else if (target == "IOS")
        {
            target_name = platform;
            targetGroup = BuildTargetGroup.iPhone;
            buildTarget = BuildTarget.iPhone;
			
			PlayerSettings.bundleIdentifier = "com.disney.frozensagachina";
        }
    
        if (Directory.Exists(target_dir))
        {
             if (File.Exists(target_name))
             {
             	File.Delete(target_name);
             }
        }
		else  
		{
             Directory.CreateDirectory(target_dir);
        }
 
        switch (platform)
        {
        	case DIANXIN_ZIYOU:
            	PlayerSettings.bundleIdentifier = "com.mfp.frozen.playgame";
				CopyFiles("playgame_self");
				File.Delete(Application.dataPath + "/Plugins/Android/" + "smspayforchannel.jar");
                break;
			case LIANTONG:
                PlayerSettings.bundleIdentifier = "com.mfp.frozen.unicom"; 
				CopyFiles("unicom_self");
				File.Delete(Application.dataPath + "/Plugins/Android/" + "smspayforegame.jar");
                break;
			case DIANXIN_WAIFANG:
			case YIDONG_MM_SHENHE:
			case YIDONG_MM_ZIQIANMING:
			case YIDONG_MDO:
                PlayerSettings.bundleIdentifier = "com.mfp.frozen.playgame.disney";  
				CopyFiles("playgame_out");
				File.Delete(Application.dataPath + "/Plugins/Android/" + "smspayforegame.jar");
                break;
			case TEST:
				PlayerSettings.bundleIdentifier = "com.mfp.frozen.playgame.disney.test";  
				CopyFiles("playgame_out");
				File.Delete(Application.dataPath + "/Plugins/Android/" + "smspayforegame.jar");
                break;
        }
		
		PlayerSettings.bundleVersion = BUNDLE_VERSION;
		PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, platform); 
 
		string[] scenes = FindEnabledEditorScenes();
        GenericBuild(scenes, target_dir + "/" + target_name, buildTarget, BuildOptions.None); 
		
		if (platform == YIDONG_MM_ZIQIANMING)
		{
			Sign(target_dir + "/" + target_name);
		}
    }
	
	static string CopyFiles(string sourceFolderName)
	{
		string destFolder = Application.dataPath + "/Plugins/Android/";
		string srcFolder = Application.dataPath.Replace("FrozenPrototype/Assets", "pay_submit/") + sourceFolderName;
		
		string[] files = Directory.GetFiles(srcFolder);
		foreach (string srcFile in files)
		{
			string fileName = Path.GetFileName(srcFile);
			if (fileName.StartsWith("."))	// filter hidden files(svn, etc)
			{
				continue;
			}
			
			string destFile = destFolder + fileName;
			Debug.Log("Copy " + srcFile + " to " + destFile);
			File.Copy(srcFile, destFile, true);
		}
		
		string[] dirs = Directory.GetDirectories(srcFolder);
		foreach (string srcDir in dirs)
		{
			string dirName = Path.GetFileName(srcDir);
			if (dirName.StartsWith("."))
			{
				continue;
			}
			
			string destDir = destFolder + dirName;
			CopyDirectory(srcDir, destDir);
		}
		
		return "";
	}
	
	static void CopyDirectory(string srcDir, string tgtDir) 
	{ 
    	DirectoryInfo source = new DirectoryInfo(srcDir); 
    	DirectoryInfo target = new DirectoryInfo(tgtDir); 

    	if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase)) 
    	{ 
        	throw new Exception("");
		}

    	if (!source.Exists) 
    	{ 
        	return; 
    	} 

    	if (!target.Exists) 
    	{ 
        	target.Create(); 
    	} 

    	FileInfo[] files = source.GetFiles(); 

    	for (int i = 0; i < files.Length; i++) 
    	{ 
			if (files[i].Name.StartsWith("."))
			{
				continue;
			}
			
			string destFile = target.FullName + "/" + files[i].Name;
			Debug.Log("Copy " + files[i].FullName + " to " + destFile);
        	File.Copy(files[i].FullName, destFile, true); 
    	} 

    	DirectoryInfo[] dirs = source.GetDirectories(); 
    	for (int j = 0; j < dirs.Length; j++) 
    	{ 
			if (dirs[j].Name.StartsWith("."))
			{
				continue;
			}
			
        	CopyDirectory(dirs[j].FullName, target.FullName + "/" + dirs[j].Name); 
    	} 
	} 
	
	static void Sign(string inApkFile)
	{
		string aaptTool = ANDROID_SDK_PATH + "build-tools/19.0.0/aapt";
		string zipAlignTool = ANDROID_SDK_PATH + "tools/zipalign";
		string copyFile = SIGN_FILE_PATH + "CopyrightDeclaration.xml";
		string iapFile = SIGN_FILE_PATH + "mmiap.xml";
		string keyFile = SIGN_FILE_PATH + "cakeGooglePlay.keystore";
		string outApkFile = inApkFile.Replace(".apk", "_sign.apk");
		
		string cmd1 = aaptTool + " add " + inApkFile + " " + copyFile + " " + iapFile;
		string cmd2 = "jarsigner -verbose -keystore " + keyFile + " -storepass 111111 -sigfile CERT " + inApkFile + " dreambakery";
		string cmd3 = zipAlignTool + " -f -v 4 " + inApkFile + " " + outApkFile;
		
		Debug.Log(cmd1);
		//System.Diagnostics.Process proc1 = System.Diagnostics.Process.Start(cmd1);	
		//proc1.WaitForExit(30000);
		
		Debug.Log(cmd2);
		//System.Diagnostics.Process proc2 = System.Diagnostics.Process.Start(cmd2);	
		//proc2.WaitForExit(30000);
		
		Debug.Log(cmd3);
		//System.Diagnostics.Process proc3 = System.Diagnostics.Process.Start(cmd3);	
		//proc3.WaitForExit(30000);
		
		string shPath = getDataFolder() + "/sign.sh";
		StreamWriter sw = new StreamWriter(shPath);
		sw.WriteLine(cmd1);
		sw.WriteLine(cmd2);
		sw.WriteLine(cmd3);
		sw.Close();
	}
	
	static string getDataFolder()
	{
		DateTime now = System.DateTime.Now;
		string monthStr = now.Month < 10 ? "0" + now.Month.ToString() : now.Month.ToString();
		string dayStr = now.Day < 10 ? "0" + now.Day.ToString() : now.Day.ToString();
		string dataDir = DATA_PATH + monthStr + dayStr;
		
		if (!Directory.Exists(dataDir))
		{
			Directory.CreateDirectory(dataDir);
		}
		
		return dataDir;
	}
	
	static string getApkName(string platform)
	{
		DateTime now = System.DateTime.Now;
		string hourStr = now.Hour < 10 ? "0" + now.Hour.ToString() : now.Hour.ToString();
		string minuteStr = now.Minute < 10 ? "0" + now.Minute.ToString() : now.Minute.ToString();
		string apkName = APP_NAME + "." + platform + "." + hourStr + minuteStr + ".apk";
		return apkName;
	}
 
    private static string[] FindEnabledEditorScenes() 
	{
        List<string> EditorScenes = new List<string>();
        foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }
 
    static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {  
        EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
        string res = BuildPipeline.BuildPlayer(scenes,target_dir,build_target,build_options);
 
        if (res.Length > 0) 
		{
        	throw new Exception("BuildPlayer failure: " + res);
        }
    }

}
