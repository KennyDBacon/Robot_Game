using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
	Text titleText;

	void Awake ()
	{
		titleText = this.transform.Find ("Title").GetComponent<Text> ();
	}

	public void EnableScreen (string t)
	{
		titleText.text = t;
		this.gameObject.SetActive (true);
	}

	public void SetGameMode (int index)
	{
		switch (index) {
		case 1:
			GameManager.GameModeManager.SetGameMode (GameModeManager.Mode.Assault);
			break;
		case 2:
			GameManager.GameModeManager.SetGameMode (GameModeManager.Mode.TimeAttack);
			break;
		}

		this.gameObject.SetActive (false);
	}
}
