using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	TargetConnectivityScreen targetConnectivityScreen;
	MenuScreen menuScreen;

	Text objectiveText, objectiveSubText, centerText;

	void Awake ()
	{
		targetConnectivityScreen = this.transform.Find ("TargetConnectivityScreen").GetComponent<TargetConnectivityScreen> ();
		menuScreen = this.transform.Find ("MenuScreen").GetComponent<MenuScreen> ();

		objectiveText = this.transform.Find ("ObjectiveText").GetComponent<Text> ();
		objectiveSubText = objectiveText.transform.Find ("Text").GetComponent<Text> ();
		centerText = this.transform.Find ("CenterText").GetComponent<Text> ();
	}

	void Start ()
	{
		menuScreen.EnableScreen ("Main Menu");
	}

	void Update ()
	{
		/*
		if (GameManager.IREController.IsUpdateConnectivity) {
			targetConnectivityScreen.UpdateIndicator ();
		}*/
	}

	public void ToggleMenuScreen ()
	{
		if (!menuScreen.gameObject.activeSelf) {
			menuScreen.EnableScreen ("Main Menu");
		} else if (GameManager.GameModeManager.IsAllowMenuToggle) {
			menuScreen.gameObject.SetActive (false);
		}
	}

	public void SetObjectiveText (string title)
	{
		objectiveText.text = title;
	}

	public void UpdateObjectiveSubText (string info)
	{
		objectiveSubText.text = info;
	}

	public void UpdateCenterText (string info)
	{
		centerText.text = info;
	}

	public bool IsPaused {
		get { return menuScreen.gameObject.activeSelf; }
	}

	public TargetConnectivityScreen TargetConnectivityScreen {
		get { return targetConnectivityScreen; }
	}

	public MenuScreen MenuScreen {
		get { return menuScreen; }
	}
}
