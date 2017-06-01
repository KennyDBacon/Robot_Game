using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBinds : MonoBehaviour
{
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			GameManager.UIManager.ToggleMenuScreen ();
		}
	}
}
