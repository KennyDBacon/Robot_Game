using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	Material bubbleMat;

	int maxHealth = 25;
	int health;

	int score;

	Color baseColor = new Color (0.0f, 0.5f, 0.5f);

	void Start ()
	{
		bubbleMat = GetComponent<MeshRenderer> ().material;
		bubbleMat.color = Transparent (baseColor);
	}

	public void UpdateScore (int value)
	{
		score += value;
	}

	public void TakeDamage (int value)
	{
		if (GameManager.GameModeManager.CurrentGameMode == GameModeManager.Mode.PvE) {
			if (health > 0) {
				health -= value;
				GameManager.UIManager.UpdateHealth ();
				UpdateBubbleColour ();

				if (health <= 0) {
					GameManager.GameModeManager.EndRound (GameEndScreen.Title.PVE_Lose);
				}
			}
		}
	}

	public void UpdateBubbleColour ()
	{
		float value = (float)health / (float)maxHealth;

		bubbleMat.color = Vector4.Lerp (Transparent (Color.red), Transparent (baseColor), value);
	}

	public void Reset ()
	{
		health = maxHealth;
		GameManager.UIManager.UpdateHealth ();
		UpdateBubbleColour ();

		score = 0;
	}

	public Color Transparent (Color color)
	{
		color.a *= 0.55f;
		return color;
	}

	public int Health {
		get { return health; }
	}

	public int Score {
		get { return score; }
	}
}
