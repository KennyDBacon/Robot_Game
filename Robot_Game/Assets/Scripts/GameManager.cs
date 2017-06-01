using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static IREController IREController;
	public static GameModeManager GameModeManager;
	public static UIManager UIManager;

	public static Transform Center;

	void Awake ()
	{
		IREController = GetComponentInChildren<IREController> ();
		GameModeManager = GetComponentInChildren<GameModeManager> ();
		UIManager = GameObject.Find ("Canvas").GetComponent<UIManager> ();

		Center = GameObject.Find ("PlayerCenter").transform;
	}

	public void ExitGame ()
	{
		Application.Quit ();
	}
}
