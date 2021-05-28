using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

namespace Voxel.Training
{
	public class AvoidRockManager : TrainingManagerBase
	{
		[SerializeField] private GameObject rock; // 岩
		[SerializeField] private TrainingMonster monster;
		private bool isFinished = false;

		public override IEnumerator Run(TrainingResult result)
		{
			yield return base.Run(result);

			StartRollingRock();

			yield return new WaitUntil(() => isFinished);
		}

		private void StartRollingRock()
		{
			// 避けるアニメーション
			this.gameObject.UpdateAsObservable()
				.Where(_ => Vector3.Distance(monster.transform.position, rock.transform.position) <= 5f)
				.First()
				.Subscribe(_ => 
				{
					monster.transform.DOLocalJump(monster.transform.position + monster.transform.right * 3f, 2f, 1, 1f);
				}).AddTo(this);

			// 岩が向かってくるアニメーション
			rock.transform.LookAt(monster.transform);
			rock.transform.DOMove(new Vector3(monster.transform.position.x - 10f, rock.transform.position.y, monster.transform.position.z), 5f)
				.SetEase(Ease.Linear)
				.OnComplete(() => { isFinished = true; });
			rock.transform.DOLocalRotate(new Vector3(360f, 0f, 0f), 1f, RotateMode.FastBeyond360)
				.SetEase(Ease.Linear)
				.SetLoops(-1, LoopType.Restart);
		}
	}
}
