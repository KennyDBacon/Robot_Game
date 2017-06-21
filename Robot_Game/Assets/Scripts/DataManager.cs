using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
	public bool IsDataReady;

	bool isCheckingData;

	static DifficultyContainer difficultyContainer;
	static TargetStatContainer targetStatContainer;

	void Awake ()
	{
		DontDestroyOnLoad (this);

		difficultyContainer = DifficultyContainer.Load (Application.streamingAssetsPath + "/Difficulty.xml");
		targetStatContainer = TargetStatContainer.Load (Application.streamingAssetsPath + "/EnemyStats.xml");

		StartCoroutine (CheckDataReady ());
	}

	void Update ()
	{
		if (!IsDataReady) {
			if (!isCheckingData) {
				StartCoroutine (CheckDataReady ());
			}
		}
	}

	IEnumerator CheckDataReady ()
	{
		isCheckingData = true;

		yield return new WaitForSeconds (1.0f);

		if (difficultyContainer.Settings != null && targetStatContainer.TargetStats != null) {
			IsDataReady = true;

			StartCoroutine (LoadSceneSequence ());
		}

		isCheckingData = false;
	}

	IEnumerator LoadSceneSequence ()
	{
		yield return new WaitForSeconds (1.0f);

		SceneManager.LoadScene ("Main");
	}

	public static List<DifficultySetting> DifficultySettings {
		get { return difficultyContainer.Settings; }
	}

	public static List<TargetStat> TargetStats {
		get { return targetStatContainer.TargetStats; }
	}
}
