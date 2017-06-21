using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuScreen : MonoBehaviour
{
	Text titleText;

	GameObject mainScreen, modeScreen, settingScreen;

	float slideDuration = 0.3f;

	void Awake ()
	{
		titleText = this.transform.Find ("Title").GetComponent<Text> ();

		mainScreen = this.transform.Find ("MainScreen").gameObject;
		modeScreen = this.transform.Find ("ModeScreen").gameObject;
		settingScreen = this.transform.Find ("SettingScreen").gameObject;

		modeScreen.transform.localPosition = new Vector3 (Screen.width, modeScreen.transform.localPosition.y, modeScreen.transform.localPosition.z);
		settingScreen.transform.localPosition = new Vector3 (Screen.width, settingScreen.transform.localPosition.y, settingScreen.transform.localPosition.z);
	}

	public void SlideScreen (int page)
	{
		switch (page) {
		case -1:
			mainScreen.transform.DOLocalMoveX (0.0f, slideDuration);
			modeScreen.transform.DOLocalMoveX (Screen.width, slideDuration);
			settingScreen.transform.DOLocalMoveX (Screen.width, slideDuration);
			break;
		case 1:
			mainScreen.transform.DOLocalMoveX (-Screen.width, slideDuration);
			modeScreen.transform.DOLocalMoveX (0.0f, slideDuration);
			break;
		case 2:
			mainScreen.transform.DOLocalMoveX (-Screen.width, slideDuration);
			settingScreen.transform.DOLocalMoveX (0.0f, slideDuration);

			settingScreen.transform.Find ("DifficultyText").GetComponent<Text> ().text = "Difficulty: " + GameManager.GameModeManager.CurrentDifficulty.ID;
			break;
		}
	}

	public void EnableScreen ()
	{
		this.gameObject.SetActive (true);
	}

	public void SetGameMode (int index)
	{
		switch (index) {
		case 1:
			GameManager.GameModeManager.SetGameMode (GameModeManager.Mode.PvP);
			break;
		case 2:
			GameManager.GameModeManager.SetGameMode (GameModeManager.Mode.PvE);
			break;
		}

		this.gameObject.SetActive (false);
	}

	public void SetDifficulty (int index)
	{
		GameManager.GameModeManager.SetDifficulty (index);

		settingScreen.transform.Find ("DifficultyText").GetComponent<Text> ().text = "Difficulty: " + GameManager.GameModeManager.CurrentDifficulty.ID;
	}
}
