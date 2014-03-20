using UnityEngine;
using System.Collections;

public class CompanionSelect : MonoBehaviour
{	
	public static string iconPrefix = "menu_character_";
	public static string iconSufixOn = "_on";
	public static string iconSufixOff = "_off";
	public static string[] icons = new string[]{"anna", "elsa", "sven", "kristoff", "olaf", "pabbie", "hans", "annasmall", "elsasmall", "kristoffsmall"};
	
	public bool selected;
	protected GameObject selectedObj;
	protected GameObject unselectedObj;
	protected string selectEvent;
	protected UIPanel myPanel;
	
	protected CompanionsHolder holder;
	
	public int characterIndex = 0;
		
	void Start() 
	{
		holder = transform.parent.GetComponent<CompanionsHolder>();
		
		selectedObj = transform.Find("Selected").gameObject;
		unselectedObj = transform.Find("Unselected").gameObject;
		selectedObj.SetActive(selected);
		unselectedObj.SetActive(!selected);
		
		if (selected) {
			myPanel = selectedObj.GetComponent<UISprite>().panel;
			Select();
		} 
		else {
			myPanel = unselectedObj.GetComponent<UISprite>().panel;
		}
		
		myPanel.Refresh();
		
//		if (selected) {
//			holder.selectedCompanion = this;
//		}
		
		selectEvent = "Select " + name.Substring(name.Length - 1);
	}
	
	public void UpdateCompanion(int index)
	{
		characterIndex = index;
		
		selectedObj.GetComponent<UISprite>().spriteName = iconPrefix + icons[index] + iconSufixOn;
		unselectedObj.GetComponent<UISprite>().spriteName = iconPrefix + icons[index] + iconSufixOff;
		
		selectedObj.SetActive(selected);
		unselectedObj.SetActive(!selected);
		
		if (selected) {
			Select();
		}
	}
	
	void OnClick() 
	{ 
//		Debug.Log("Cloicked: " + name);
		
		if (enabled) 
		{
			if (!selected) {
				holder.targetFsm.SendEvent(selectEvent);
			}
			else {
				if (holder.itemDescription.shown) {
					holder.itemDescription.Hide();
				}
				else {
					holder.itemDescription.Show();
				}
			}
		}
	}

	public void SelectCompanion ()
	{
		selected = !selected;
		
		selectedObj.SetActive(selected);
		unselectedObj.SetActive(!selected);
		myPanel.Refresh();
						
		if (selected && holder.selectedCompanion != this) {
			holder.selectedCompanion.SelectCompanion();
		}
		
		if (selected) {
			Select();
		}
	}
	
	void Select()
	{
		CharacterSpecialAnimations.characterIndex = characterIndex;
		holder.tokensContainer.transform.localPosition = transform.localPosition;
		holder.selectedCompanion = this;
		holder.itemDescription.UpdateDescription(characterIndex);
	}
}

