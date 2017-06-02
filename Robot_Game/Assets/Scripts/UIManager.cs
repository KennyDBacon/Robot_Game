using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	TargetConnectivityScreen targetConnectivityScreen;
	MenuScreen menuScreen;

	GameObject infoTexts;
	Text objectiveText, objectiveSubText, centerText, scoreText, scoreSubText;

	void Awake ()
	{
		targetConnectivityScreen = this.transform.Find ("TargetConnectivityScreen").GetComponent<TargetConnectivityScreen> ();
		menuScreen = this.transform.Find ("MenuScreen").GetComponent<MenuScreen> ();

		infoTexts = this.transform.Find ("InfoTexts").gameObject;
		objectiveText = infoTexts.transform.Find ("ObjectiveText").GetComponent<Text> ();
		objectiveSubText = infoTexts.transform.Find ("ObjectiveSubText").GetComponent<Text> ();
		centerText = infoTexts.transform.Find ("CenterText").GetComponent<Text> ();
		scoreText = infoTexts.transform.Find ("ScoreText").GetComponent<Text> ();
		scoreSubText = infoTexts.transform.Find ("ScoreSubText").GetComponent<Text> ();
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

	public void UpdateScoreText (string info)
	{
		scoreSubText.text = info;
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
