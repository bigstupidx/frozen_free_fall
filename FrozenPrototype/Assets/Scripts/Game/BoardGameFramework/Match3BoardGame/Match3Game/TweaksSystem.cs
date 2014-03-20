using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweaksSystem : MonoBehaviour
{
	// NOT CHANGE because data of tweaks and user progress could be lost
//	public const string CRYPTO_KEY = "FROZEN12";
	
	protected static TweaksSystem instance;
	
	public Dictionary<string, int> intValues;
	public Dictionary<string, float> floatValues;
	public Dictionary<string, string> stringValues;
	
	public int OfferPrice {
		get {
			return 0;//intValues["OfferPrice"];
		}
	}
	
	public static TweaksSystem Instance {
		get {
			if (instance == null) {
				GameObject container = new GameObject("TweaksSystem");
				instance = container.AddComponent<TweaksSystem>();
				DontDestroyOnLoad(container);
			}
			
			return instance;
		}
	}
	
	public static Dictionary<string, int> GetDefaultIntValues()
	{
		return new Dictionary<string, int>() {
			{"MovesScoreMultiplier", 2000},
			{"MaxMultiplier", 10},
			{"MultipliedScore", 50},
			{"ItemsPack0", 2},
			{"ItemsPack1", 5},
			{"ItemsPack2", 10},
			{"IcePickPack0", 2},
			{"IcePickPack1", 5},
			{"IcePickPack2", 10},
			{"SnowballPack0", 1},
			{"SnowballPack1", 3},
			{"SnowballPack2", 7},
			{"SnowballFreePack0", 1},
			{"SnowballFreePack1", 2},
			{"SnowballFreePack2", 3},
			{"HourglassPack0", 1},
			{"HourglassPack1", 3},
			{"HourglassPack2", 7},
			{"HourglassFreePack0", 1},
			{"HourglassFreePack1", 2},
			{"HourglassFreePack2", 3},
			{"LifeRefillTime", 1800},
			{"TutorialTokens", 2},
			{"TutorialIcePicks", 2},
			{"TutorialSnowballs", 2},
			{"TutorialHourglasses", 2},
			{"Level1Star2", 4000},
			{"Level1Star3", 5000},
			{"Level8Destroy0", 1},
			{"Level8Destroy1", 1},
			{"Level8Star1", 20000},
			{"Level8Star2", 45000},
			{"Level8Star3", 60000},
			{"Level9Destroy0", 2},
			{"Level9Destroy1", 2},
			{"Level9Star1", 30000},
			{"Level9Star2", 55000},
			{"Level9Star3", 80000},
			{"Level12Star1", 20000},
			{"Level13Star1", 30000},
			{"Level13Star2", 50000},
			{"Level13Star3", 75000},
			{"Level18Star1", 30000},
			{"Level18Star2", 40000},
			{"Level18Star3", 50000},
			{"Level20Destroy0", 35},
			{"Level20Destroy1", 35},
			{"Level21Destroy0", 32},
			{"Level21Destroy1", 32},
			{"Level21Destroy2", 32},
			{"Level22Destroy0", 25},
			{"Level22Destroy1", 25},
			{"Level24Star1", 40000},
			{"Level33Star1", 30000},
			{"Level33Star2", 35000},
			{"Level33Star3", 45000},
			{"Level35Moves", 25},
			{"Level51Moves", 45},
			{"Level53Moves", 65},
			{"Level53Star1", 40000},
			{"Level53Star2", 60000},
			{"Level53Star3", 80000},
			{"Level58Destroy0", 2},
			{"Level62Moves", 50},
		};
	}
	
	public static Dictionary<string, float> GetDefaultFloatValues()
	{
		return new Dictionary<string, float>() {
			{"MultiplierWait", 2f},
			{"ResyncTime", 60f},
			{"Level24Time", 90f},
			{"Level33Time", 75f},
		};
	}
	
	public static Dictionary<string, string> GetDefaultStringValues()
	{
		return null;
	}
	
	void Awake() 
	{
		intValues = GetDefaultIntValues();
		
		floatValues = GetDefaultFloatValues();
		
		stringValues = GetDefaultStringValues();
	}
}

