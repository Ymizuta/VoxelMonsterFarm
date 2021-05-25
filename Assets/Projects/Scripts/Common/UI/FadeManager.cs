using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Voxel.UI
{
	public class FadeManager : SingletonMonoBehaviour<FadeManager>
	{
		[SerializeField] private Image img;
		[SerializeField] private float fadeTime = 1f;

		public bool IsFadeOut { get; private set; }

		public void PlayFadeIn(Action action = null)
		{
			img.color = new Color(0f,0f,0f, 1f);
			PlayFade(0f, () => 
			{
				IsFadeOut = false;
				action?.Invoke();
			});
		}

		public void PlayFadeOut(Action action = null)
		{
			img.color = new Color(0f, 0f, 0f, 0f);
			PlayFade(1f, () => 
			{
				IsFadeOut = true;
				action?.Invoke();
			});
		}

		private void PlayFade(float fadeVal, Action action = null)
		{
			DOTween.ToAlpha(
				() => img.color,
				color => img.color = color,
				fadeVal,
				fadeTime)
				.OnComplete(() => action?.Invoke());
		}
	}
}
