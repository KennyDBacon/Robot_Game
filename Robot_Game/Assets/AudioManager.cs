using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public List<AudioClip> PlayList;

	AudioSource audioPlayer;

	bool isDimMusic;
	float dimTimer;

	void Start ()
	{
		audioPlayer = this.GetComponent<AudioSource> ();
	}

	public void PlayMusic ()
	{
		audioPlayer.Stop ();

		int index = 0;

		switch (GameManager.GameModeManager.CurrentGameMode) {
		case GameModeManager.Mode.PvP:
			index = Random.Range (0, 2);

			//audioPlayer.loop = false;
			audioPlayer.clip = PlayList [index];
			audioPlayer.PlayDelayed (0.21f);
			break;
		case GameModeManager.Mode.PvE:
			index = Random.Range (2, 5);

			//audioPlayer.loop = true;
			audioPlayer.clip = PlayList [index];
			audioPlayer.Play ();
			break;
		}
	}

	public void ResumeMusic ()
	{
		audioPlayer.UnPause ();
	}

	public void PauseMusic ()
	{
		audioPlayer.Pause ();
	}

	public void StopMusic ()
	{
		audioPlayer.Stop ();
	}
}
