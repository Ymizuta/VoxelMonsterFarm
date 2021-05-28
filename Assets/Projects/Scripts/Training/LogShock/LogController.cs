using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Voxel.Training
{
	public class LogController : MonoBehaviour
	{
		[SerializeField] private GameObject[] rightLogs;
		[SerializeField] private GameObject[] leftLogs;

		private const float logSpeed = 0.5f;

		public void Run()
		{
			for (int i = 0; i < rightLogs.Length; i++)
			{
				rightLogs[i].transform.DOMove(rightLogs[i].transform.position + Vector3.forward * -3f, logSpeed).SetLoops(-1, LoopType.Yoyo);
			}
			for (int i = 0; i < leftLogs.Length; i++)
			{
				leftLogs[i].transform.DOMove(leftLogs[i].transform.position + Vector3.forward * 3f, logSpeed).SetLoops(-1, LoopType.Yoyo);
			}
		}
	}
}
