using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Voxel.Training
{
	public class LogShockManagement : TrainingManagerBase
	{
		[SerializeField] private TrainingMonster monster;
		[SerializeField] private LogController controller;
		[SerializeField] private Transform goal;

		private bool isFinished;

		public override IEnumerator Run(TrainingResult result)
		{
			yield return base.Run(result);
			controller.Run();
			monster.transform.DOJump(goal.position, 1f, (int)Vector3.Distance(goal.position, monster.transform.position) / 1,  10f)
				.OnComplete(() => 
				{
					isFinished = true;
				});
			yield return new WaitUntil(() => isFinished);
		}
	}
}
