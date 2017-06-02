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

	void Update ()
	{
		
	}

	public void UpdateScore (int value)
	{
		score += value;

		GameManager.UIManager.UpdateScoreText (score.ToString ());
	}

	public void TakeDamage (int value)
	{
		health -= value;
		GameManager.UIManager.UpdateCenterText (health.ToString ());

		UpdateBubbleColour ();

		if (health <= 0) {
			GameManager.GameModeManager.EndGame ();
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
	}

	public int Health {
		get { return health; }
	}

	public Color Transparent (Color color)
	{
		color.a *= 0.55f;
		return color;
	}
}
