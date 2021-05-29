using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Voxel.Training
{
	public class ShootingManager : TrainingManagerBase
	{
		[SerializeField] private TrainingMonster monster;
		[SerializeField] private Transform[] shootTrans;
		[SerializeField] private Transform[] targets;
		[SerializeField] private GameObject[] bullets;
		[SerializeField] private ParticleSystem[] hitEffects;
		private bool[] isHit;
		private bool IsFinished { get
			{
				for (int i = 0; i < isHit.Length; i++)
				{
					if (!isHit[i]) return false;
				}
				return true;
			} }

		public override IEnumerator Run(TrainingResult result)
		{
			isHit = new bool[targets.Length];

			yield return base.Run(result);

			yield return StartShoot();

			yield return new WaitUntil(() => IsFinished);
			yield return new WaitForSeconds(1f);
		}

		private IEnumerator StartShoot()
		{
			for (int i = 0; i < shootTrans.Length; i++)
			{
				bool isDone = false;
				// ”­ŽËˆÊ’u‚ÉˆÚ“®
				monster.transform.DOJump(shootTrans[i].position, 1f, 1, 1f)
					.SetEase(Ease.Linear)
					.OnComplete(() => 
					{
						isDone = true;
						var idx = i;
						// ’e‚ð”ò‚Î‚·
						bullets[idx].transform.DOMove(targets[idx].position, 1f)
							.OnComplete(() => 
							{
								// ’e‚ðÁ‚·
								hitEffects[idx].Play();
								bullets[idx].gameObject.SetActive(false);
								isHit[idx] = true;
							});
						isDone = true;
					});
				yield return new WaitUntil(() => isDone);
			}
		}
	}
}
