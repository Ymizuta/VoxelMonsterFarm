using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Training
{
	public class DominoManager : TrainingManagerBase
	{
		[SerializeField] private TrainingMonster monster;
		[SerializeField] private DominoController dominoController;

		public override IEnumerator Run(TrainingResult result)
		{
			yield return base.Run(result);
			yield return monster.PlayAttackAnimationCoroutine();
			yield return dominoController.Run();
		}
	}
}
