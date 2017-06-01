using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Indicator : MonoBehaviour
{
	Image box, arrow;

	Sequence blinkSequence;

	void Awake ()
	{
		box = this.transform.Find ("Box").GetComponent<Image> ();
		arrow = this.transform.Find ("Arrow").GetComponent<Image> ();

		box.color = Color.red;
		box.canvasRenderer.SetAlpha (0.0f);
		arrow.canvasRenderer.SetAlpha (0.0f);
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
}
