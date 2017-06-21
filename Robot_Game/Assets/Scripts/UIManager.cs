using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	TargetConnectivityScreen targetConnectivityScreen;
	MenuScreen menuScreen;
	GameEndScreen gameEndScreen;

	GameObject activeScreen;
	GameObject pvpScreen, pveScreen;

	void Awake ()
	{
		foreach (Transform child in this.transform) {
			child.gameObject.SetActive (true);
		}

		targetConnectivityScreen = this.transform.Find ("TargetConnectivityScreen").GetComponent<TargetConnectivityScreen> ();
		menuScreen = this.transform.Find ("MenuScreen").GetComponent<MenuScreen> ();
		gameEndScreen = this.transform.Find ("GameEndScreen").GetComponent<GameEndScreen> ();

		pveScreen = this.transform.Find ("PvEScreen").gameObject;
		pvpScreen = this.transform.Find ("PvPScreen").gameObject;

		activeScreen = null;
		pveScreen.SetActive (false);
		pvpScreen.SetActive (false);
	}

	void Start ()
	{
		menuScreen.EnableScreen ();
	}

	public void ToggleMenuScreen ()
	{
		if (!menuScreen.gameObject.activeSelf && !gameEndScreen.gameObject.activeSelf) {
			menuScreen.EnableScreen ();

			activeScreen.SetActive (false);

			GameManager.AudioManager.PauseMusic ();
		} else if (GameManager.GameModeManager.IsAllowMenuToggle && menuScreen.gameObject.activeSelf) {
			menuScreen.gameObject.SetActive (false);

			activeScreen.SetActive (true);

			GameManager.AudioManager.ResumeMusic ();
		}
	}

	public void EnableUI ()
	{
		if (activeScreen != null) {
			activeScreen.SetActive (false);
		}

		switch (GameManager.GameModeManager.CurrentGameMode) {
		case GameModeManager.Mode.PvP:
			activeScreen = pvpScreen;
			break;
		case GameModeManager.Mode.PvE:
			activeScreen = pveScreen;
			break;
		}

		activeScreen.SetActive (true);
	}

	public void UpdateTimer (string timerString)
	{
		activeScreen.transform.Find ("TimerSubText").GetComponent<Text> ().text = timerString;
	}

	public void UpdateScore (bool isTeam = false)
	{
		switch (GameManager.GameModeManager.CurrentGameMode) {
		case GameModeManager.Mode.PvP:
			if (!isTeam) {
				activeScreen.transform.Find ("BlueTeamSubText").GetComponent<Text> ().text = GameManager.GameModeManager.BlueIndividualScore.ToString ();
				activeScreen.transform.Find ("RedTeamSubText").GetComponent<Text> ().text = GameManager.GameModeManager.RedIndividualScore.ToString ();
			} else {
				activeScreen.transform.Find ("BlueTeamSubText").GetComponent<Text> ().text = GameManager.GameModeManager.BlueTeamScore.ToString ();
				activeScreen.transform.Find ("RedTeamSubText").GetComponent<Text> ().text = GameManager.GameModeManager.RedTeamScore.ToString ();
			}
			break;
		case GameModeManager.Mode.PvE:
			activeScreen.transform.Find ("ScoreSubText").GetComponent<Text> ().text = GameManager.Player.Score.ToString ();
			break;
		}
	}

	public void UpdateHealth ()
	{
		activeScreen.transform.Find ("HealthSubText").GetComponent<Text> ().text = GameManager.Player.Health.ToString ();
	}

	public bool IsPaused {
		get { return menuScreen.gameObject.activeSelf || gameEndScreen.gameObject.activeSelf; }
	}

	public TargetConnectivityScreen TargetConnectivityScreen {
		get { return targetConnectivityScreen; }
	}

	public MenuScreen MenuScreen {
		get { return menuScreen; }
	}

	public GameEndScreen GameEndScreen {
		get { return gameEndScreen; }
	}
}
