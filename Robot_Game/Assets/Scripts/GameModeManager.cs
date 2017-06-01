using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
	bool isGameRunning;

	Mode currentGameMode;

	int maxEnemyCount = 100;
	int enemyCount;
	int health;

	float maxTime = 60.0f * 2.0f;
	float timeRemaining;

	public enum Mode
	{
		Assault,
		TimeAttack
	}

	void Update ()
	{
		if (IsGameRunning) {
			switch (currentGameMode) {
			case Mode.Assault:
				TargetPracticeHandler ();
				break;
			case Mode.TimeAttack:
				TimeAttackHandler ();
				break;
			}
		}
	}

	void TargetPracticeHandler ()
	{
		GameManager.UIManager.UpdateObjectiveSubText (enemyCount.ToString ());

		if (enemyCount <= 0) {
			EndGame ();
		}
	}

	void TimeAttackHandler ()
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

			GameManager.UIManager.UpdateObjectiveSubText (timeString);
		} else {
			EndGame ();
		}
	}

	public void TakeDamage ()
	{
		if (health > 0) {
			health--;
			GameManager.UIManager.UpdateCenterText (health.ToString ());

			if (health <= 0) {
				EndGame ();
			}
		}
	}

	public void KillTarget ()
	{
		if (enemyCount > 0) {
			enemyCount--;
		}
	}

	public void SetGameMode (Mode mode)
	{
		isGameRunning = true;
		currentGameMode = mode;

		switch (mode) {
		case Mode.Assault:
			GameManager.UIManager.SetObjectiveText ("Enemies Left");
			enemyCount = maxEnemyCount;
			health = 25;
			break;
		case Mode.TimeAttack:
			GameManager.UIManager.SetObjectiveText ("Time Remaining");
			timeRemaining = maxTime;
			health = 25;
			break;
		}

		GameManager.IREController.Clear ();
		GameManager.UIManager.UpdateCenterText (health.ToString ());
	}

	public int SpawnCount {
		get {
			int baseSpawnCount = 3;// GameManager.IREController.Targets.Count;

			int count = 0;
			switch (currentGameMode) {
			case Mode.Assault:
				count = baseSpawnCount - (int)((float)baseSpawnCount * ((float)enemyCount / (float)maxEnemyCount));
				count = Mathf.CeilToInt (count);
				break;
			case Mode.TimeAttack:
				count = (int)((float)baseSpawnCount * (maxTime - timeRemaining) / maxTime);
				count = Mathf.CeilToInt (count);
				break;
			}

			if (count <= 0) {
				count = 1;
			}

			return count;
		}
	}

	void EndGame ()
	{
		GameManager.UIManager.MenuScreen.EnableScreen ("Game Over");
		isGameRunning = false;
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
		get {
			if (isGameRunning && !GameManager.UIManager.IsPaused) {
				return true;
			}

			return false;
		}
	}
}