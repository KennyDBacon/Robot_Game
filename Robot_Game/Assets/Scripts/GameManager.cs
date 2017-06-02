using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static IREController IREController;
	public static GameModeManager GameModeManager;
	public static UIManager UIManager;

	public static Transform Center;
	public static Player Player;

	void Awake ()
	{
		IREController = GetComponentInChildren<IREController> ();
		GameModeManager = GetComponentInChildren<GameModeManager> ();
		UIManager = GameObject.Find ("Canvas").GetComponent<UIManager> ();

		Center = GameObject.Find ("PlayerCenter").transform;
		Player = Center.GetComponent<Player> ();
	}

	public void ExitGame ()
	{
		Application.Quit ();
	}
}
