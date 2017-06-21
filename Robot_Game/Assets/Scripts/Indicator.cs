using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Indicator : MonoBehaviour
{
	Image box, arrow, star;

	Sequence blinkSequence;

	void Awake ()
	{
		box = this.transform.Find ("Box").GetComponent<Image> ();
		arrow = this.transform.Find ("Arrow").GetComponent<Image> ();

		if (this.transform.Find ("Star") != null) {
			star = this.transform.Find ("Star").GetComponent<Image> ();
			star.gameObject.SetActive (false);
		}

		box.color = Color.red;
		box.canvasRenderer.SetAlpha (0.0f);
		arrow.canvasRenderer.SetAlpha (0.0f);
	}

	void Update ()
	{

	}

	public void ShowIndicator (Color color)
	{
		box.color = color;
		box.canvasRenderer.SetAlpha (1.0f);
		box.CrossFadeAlpha (0.0f, 1.5f, true);
	}

	public void ArrowBlink ()
	{
		blinkSequence = DOTween.Sequence ();

		blinkSequence.AppendCallback (() => arrow.canvasRenderer.SetAlpha (1.0f));

		blinkSequence.AppendCallback (() => arrow.DOFade (0.12f, 0.25f));
		blinkSequence.AppendCallback (() => arrow.DOFade (1.0f, 0.25f));
		blinkSequence.AppendCallback (() => arrow.DOFade (0.0f, 0.25f));

		blinkSequence.AppendCallback (() => arrow.canvasRenderer.SetAlpha (0.0f));

		blinkSequence.Play ();
	}

	public void ToggleStar (bool isActive)
	{
		if (star != null) {
			star.gameObject.SetActive (isActive);
		}
	}
}
