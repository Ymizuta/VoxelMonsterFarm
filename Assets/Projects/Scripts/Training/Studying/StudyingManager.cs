using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Voxel.Training
{
	public class StudyingManager : TrainingManagerBase
	{
		[SerializeField] Light directionalLight;
		[SerializeField] Color eveningColor;
		[SerializeField] Color nightColor;
		private bool isFinished;
		private const float lightChangeInterval = 2f;

		public override IEnumerator Run(TrainingResult result)
		{
			yield return base.Run(result);

			yield return StartStudying();

			yield return new WaitUntil(() => isFinished);
		}

		private IEnumerator StartStudying()
		{
			// ’‹ŠÔ¨—[•û¨–é‚ÆF‚ª•Ï‚í‚Á‚Ä‚¢‚­
			yield return new WaitForSeconds(lightChangeInterval);
			ChangeLightColor(
				eveningColor, 
				() =>
				{
					ChangeLightColor(nightColor, () => { isFinished = true; });
				});
		}

		private void ChangeLightColor(Color lightColor, System.Action action)
		{
			DOTween.To(
				() => directionalLight.color,
				color => directionalLight.color = color,
				lightColor,
				lightChangeInterval)
				.OnComplete(() => action());
		}
	}
}
