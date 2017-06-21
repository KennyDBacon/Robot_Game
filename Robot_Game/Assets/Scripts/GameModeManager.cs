using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
	bool isGameRunning;

	Mode currentGameMode;
	/*
	float maxTime = 60.0f * 2.0f;
	float timeRemaining;
*/
	DifficultySetting currentDifficulty;
	float spawnInterval, minSpawnInterval, maxSpawnInterval;

	int blueIndividualScore, redIndividualScore;
	int blueTeamScore, redTeamScore;

	public enum Mode
	{
		PvP = 0,
		PvE = 1
	}

	void Start ()
	{
		currentDifficulty = DataManager.DifficultySettings [0];

		blueTeamScore = 0;
		redTeamScore = 0;
	}

	void Update ()
	{
		if (IsGameRunning) {
			//TimeHandler ();
			if (timeLeft > 0) {
				if (!timeTicking) {
					StartCoroutine (NewTimeHandler ());
				}
			}
		}
	}
	/*
	void TimeHandler ()
	{
		if (timeRemaining > 0.0f) {
			timeRemaining -= Time.deltaTime;

			int minute = (int)(timeRemaining / 60.0f);
			int second = (int)(timeRemaining % 60.0f);

			string timeString = "";
			if (second < 10) {
				timeString = minute.ToString () + ":0" + second.ToString ();
			} else {
				timeString = minute.ToString () + ":" + second.ToString ();
			}

			GameManager.UIManager.UpdateTimer (timeString);
		} else {
			if (currentGameMode == Mode.PvP) {
				EndRound (GameEndScreen.Title.Buffer_RoundEnd);
			} else {
				EndRound (GameEndScreen.Title.PVE_Win_Survived);
			}
		}
	}
*/
	int totalTime;
	int timeLeft;
	bool timeTicking;

	IEnumerator NewTimeHandler ()
	{
		timeTicking = true;
		yield return new WaitForSeconds (1.0f);

		timeLeft--;

		GameManager.UIManager.UpdateTimer (GetTimeString);

		if (timeLeft <= 0) {
			if (currentGameMode == Mode.PvP) {
				EndRound (GameEndScreen.Title.Buffer_RoundEnd);
			} else {
				EndRound (GameEndScreen.Title.PVE_Win_Survived);
			}
		}

		timeTicking = false;
	}

	public void AddScore (Target.Team team, int value)
	{
		switch (team) {
		case Target.Team.Blue:
			blueIndividualScore += value;
			break;
		case Target.Team.Red:
			redIndividualScore += value;
			break;
		case Target.Team.None:
			GameManager.Player.UpdateScore (value);
			break;
		}

		GameManager.UIManager.UpdateScore ();
	}

	public void SetGameMode (Mode mode)
	{
		isGameRunning = true;
		currentGameMode = mode;

		GameManager.UIManager.EnableUI ();

		switch (mode) {
		case Mode.PvP:
			//timeRemaining = 60.0f * 1.0f;
			totalTime = 60 * 1;
			timeLeft = totalTime;

			blueIndividualScore = 0;
			redIndividualScore = 0;
			blueTeamScore = 0;
			redTeamScore = 0;
			break;
		case Mode.PvE:
			//timeRemaining = 60.0f * 10.0f;
			totalTime = 60 * 10;
			timeLeft = totalTime;

			GameManager.Player.Reset ();
			break;
		}

		GameManager.UIManager.UpdateTimer (GetTimeString);
		GameManager.UIManager.UpdateScore ();

		minSpawnInterval = currentDifficulty.MinSpawnInterval;
		maxSpawnInterval = currentDifficulty.MaxSpawnInterval;

		spawnInterval = GetNewSpawnInterval;

		GameManager.IREController.Clear ();

		GameManager.AudioManager.PlayMusic ();
	}

	public void RestartGameMode ()
	{
		isGameRunning = true;

		switch (currentGameMode) {
		case Mode.PvP:
			//timeRemaining = 60.0f * 1.0f;
			totalTime = 60 * 1;
			timeLeft = totalTime;

			blueIndividualScore = 0;
			redIndividualScore = 0;
			break;
		case Mode.PvE:
			//timeRemaining = 60.0f * 10.0f;
			totalTime = 60 * 10;
			timeLeft = totalTime;

			GameManager.Player.Reset ();
			break;
		}

		GameManager.UIManager.UpdateTimer (GetTimeString);
		GameManager.UIManager.UpdateScore ();

		spawnInterval = GetNewSpawnInterval;

		GameManager.IREController.Clear ();

		GameManager.AudioManager.PlayMusic ();
	}

	public void SetDifficulty (int index)
	{
		currentDifficulty = DataManager.DifficultySettings [index];
	}

	public void EndRound (GameEndScreen.Title title)
	{
		if (currentGameMode == Mode.PvP) {
			blueTeamScore += blueIndividualScore;
			redTeamScore += redIndividualScore;
		}

		GameManager.AudioManager.StopMusic ();

		GameManager.UIManager.GameEndScreen.EnableScreen (title);
		isGameRunning = false;
	}

	public float GetNewSpawnInterval {
		get {
			float value = 1.0f;
			float rate = currentDifficulty.SpawnIntensity;

			value = ((float)timeLeft / (float)totalTime) / rate;
			value = Mathf.Max (value, 0.0f);

			return Mathf.Lerp (minSpawnInterval, maxSpawnInterval, value);
		}
	}

	public float GetDiversityRate {
		get {
			float rate = currentDifficulty.SpawnIntensity;
			return ((float)timeLeft / (float)totalTime) / rate;
		}
	}

	public bool IsAllowMenuToggle {
		get {
			if (isGameRunning) {
				return true;
			}

			return false;
		}
	}

	public bool IsGameRunning {
		get { return isGameRunning && !GameManager.UIManager.IsPaused; }
	}

	public int PVPResult {
		get {
			if (redTeamScore > blueTeamScore) {
				return -1;
			} else if (blueTeamScore > redTeamScore) {
				return 1;
			} else {
				return 0;
			}
		}
	}

	public string GetTimeString {
		get {
			int minute = timeLeft / 60;
			int second = timeLeft % 60;

			string timeString = "";
			if (second < 10) {
				timeString = minute.ToString () + ":0" + second.ToString ();
			} else {
				timeString = minute.ToString () + ":" + second.ToString ();
			}

			return timeString;
		}
	}

	public Mode CurrentGameMode {
		get { return currentGameMode; }
	}

	public DifficultySetting CurrentDifficulty {
		get { return currentDifficulty; }
	}

	public float SpawnInterval {
		get { return spawnInterval; }
	}

	public int BlueIndividualScore {
		get { return blueIndividualScore; }
	}

	public int RedIndividualScore {
		get { return redIndividualScore; }
	}

	public int BlueTeamScore {
		get { return blueTeamScore; }
	}

	public int RedTeamScore {
		get { return redTeamScore; }
	}
}