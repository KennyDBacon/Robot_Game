using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndScreen : MonoBehaviour
{
	Text titleText;

	GameObject continueButton;

	public enum Title
	{
		MainMenu,
		Pause,
		Buffer_RoundEnd,
		PVP_Win_BlueTeam,
		PVP_Win_RedTeam,
		PVP_Tied,
		PVE_Win_Survived,
		PVE_Win_Key,
		PVE_Lose
	}

	void Start ()
	{
		titleText = this.transform.Find ("Title").GetComponent<Text> ();

		continueButton = this.transform.Find ("ContinueButton").gameObject;

		this.gameObject.SetActive (false);
	}

	public void EnableScreen (Title t)
	{
		UpdateTitle (t);
		this.gameObject.SetActive (true);
	}

	void UpdateTitle (Title t)
	{
		string title = "";

		switch (t) {
		case Title.MainMenu:
			title = "Main Menu";
			break;
		case Title.Pause:
			title = "Paused";
			break;
		case Title.Buffer_RoundEnd:
			title = "Round End";
			break;
		case Title.PVP_Win_BlueTeam:
			title = "Blue Team Wins!";
			break;
		case Title.PVP_Win_RedTeam:
			title = "Red Team Wins!";
			break;
		case Title.PVP_Tied:
			title = "Tied!";
			break;
		case Title.PVE_Win_Survived:
			title = "You survived!";
			break;
		case Title.PVE_Win_Key:
			title = "You've found the key!";
			break;
		case Title.PVE_Lose:
			title = "You were defeated!";
			break;
		}

		titleText.text = title;
	}

	public void ContinueGame ()
	{
		GameManager.GameModeManager.RestartGameMode ();
		this.gameObject.SetActive (false);
	}

	public void EndGame ()
	{
		if (continueButton.activeSelf && GameManager.GameModeManager.CurrentGameMode == GameModeManager.Mode.PvP) {
			continueButton.SetActive (false);

			if (GameManager.GameModeManager.PVPResult == -1) {
				UpdateTitle (Title.PVP_Win_RedTeam);
			} else if (GameManager.GameModeManager.PVPResult == 1) {
				UpdateTitle (Title.PVP_Win_BlueTeam);
			} else {
				UpdateTitle (Title.PVP_Tied);
			}

			GameManager.UIManager.UpdateScore (true);
		} else {
			continueButton.SetActive (true);
			this.gameObject.SetActive (false);
			GameManager.UIManager.MenuScreen.EnableScreen ();
		}
	}
}
