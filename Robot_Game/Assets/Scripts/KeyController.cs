using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
	public string ID;
	public bool IsConnected;

	string[] currInput;

	bool connectionCheckFlag;

	float connectionTimer;
	float connectionTimerInterval = 1.2f;
	int connectionFailedCount;
	int connectionFailedTimes = 3;

	public void Connect (string[] input)
	{
		ID = input [1];
		connectionCheckFlag = true;

		IsConnected = true;

		print (ID);
	}

	void Update ()
	{
		ConnectionCheck ();

		if (IsConnected) {
			InputHandler ();
		}
	}

	public void UpdateInput (string[] data)
	{
		connectionCheckFlag = true;

		currInput = data;
	}

	public bool CheckID (string currID)
	{
		if (ID.Equals (currID)) {
			return true;
		}

		return false;
	}

	void ConnectionCheck ()
	{
		if (IsConnected) {
			connectionTimer += Time.deltaTime;

			if (connectionTimer >= connectionTimerInterval) {
				connectionTimer = 0.0f;

				if (!connectionCheckFlag) {
					connectionFailedCount++;

					if (connectionFailedCount > connectionFailedTimes) {
						IsConnected = false;
					}
				} else {
					connectionCheckFlag = false;
					connectionFailedCount = 0;
				}
			}
		}
	}

	void InputHandler ()
	{
		if (GameManager.GameModeManager.IsGameRunning) {
			if (currInput != null) {
				// If in sword mode
				if (currInput [0].Equals ("1")) {
					// If thrust motion was received
					if (currInput [4].Contains ("T")) {
						GameManager.GameModeManager.EndRound (GameEndScreen.Title.PVE_Win_Key);
					}
				}
			}
		}
	}
}