using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using Voxel.UI;
using Voxel.SceneManagement;

namespace Voxel.Training
{
	public class RunningManager : TrainingManagerBase
	{
		[SerializeField] private TrainingMonster runner;
		[SerializeField] private Transform pathRoot;
		[SerializeField] private float speed = 10f; // unit/•b
		private Transform[] passPoints;
		private int passIdx;
		private float jumpDistance = 3f;
		private Tween tween;
		private bool isFinished;

		public override IEnumerator Run(TrainingResult result)
		{
			yield return base.Run(result);
			Initialize();
			StartRunning();
			yield return new WaitUntil(() => isFinished);
		}

		private void Initialize()
		{
			var paths = pathRoot.GetComponentsInChildren<PathPoint>();
			passPoints = new Transform[paths.Length];
			for (int i = 0; i < paths.Length; i++)
			{
				passPoints[i] = pathRoot.GetComponentsInChildren<PathPoint>()[i].gameObject.transform;
			}
		}

		private void StartRunning()
		{
			passIdx = 1;
			Run();
		}

		private void Run()
		{
			var target = passPoints[passIdx].position;
			var distance = Vector3.Distance(target, runner.transform.position);
			int junpCount = (int)( distance/ jumpDistance);
			runner.transform.LookAt(target);
			tween = runner.transform.DOJump(passPoints[passIdx].position, 2f, junpCount, distance / speed)
				.SetEase(Ease.Linear)
				.OnComplete(() => 
				{
					passIdx++;
					if (passIdx >= passPoints.Length)
					{
						isFinished = true;
						return;
					} 
					Run();
				});
		}
	}
}
