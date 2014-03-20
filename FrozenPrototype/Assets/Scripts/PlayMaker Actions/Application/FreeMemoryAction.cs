using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory(ActionCategory.Application)]
[Tooltip("Frees all unusued unity assets and calls the garbage collector to free up unused memory.")]
public class FreeMemoryAction : FsmStateAction
{	
	// Code that runs on entering the state.
	public override void OnEnter()
	{
		FreeMemoryAction.FreeMemory();
		
		Finish();
	}
	
	public static void FreeMemory()
	{
		Debug.Log("FSM FreeMemory - Resources.UnloadUnusedAssets()");
		Resources.UnloadUnusedAssets();
		
		Debug.Log("FSM FreeMemory - System.GC.Collect()");
		System.GC.Collect();
		
		if (!isSpecailDevice())
		{
			Debug.Log("FSM FreeMemory - System.GC.WaitForPendingFinalizers()");
			System.GC.WaitForPendingFinalizers();
		}
	}
	
	static bool isSpecailDevice()
	{
		string specialDeviceStr = PlayerPrefs.GetString("device_key_words", "MI 2");
		Debug.Log("Special Device = " + specialDeviceStr.ToString());
		string [] devices = specialDeviceStr.Split(new char[] {','});
		
		string currentDevice = MFPDeviceAndroid.Instance.getDeviceModel();
		Debug.Log("Device: " + currentDevice.ToString());
		
		bool isSpecial = false;
		for (int i = 0; i < devices.Length; i++)
		{
			if (currentDevice.IndexOf(devices[i]) != -1)
			{
				isSpecial = true;
				break;
			}
		}
		
		return isSpecial;
	}
}
