using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

namespace Voxel.Training
{
	public class LogController : MonoBehaviour
	{
		[SerializeField] private GameObject[] rightLogs;
		[SerializeField] private GameObject[] leftLogs;
		[SerializeField] private ParticleSystem[] rightEffects;
		[SerializeField] private ParticleSystem[] leftEffects;
		private const float logSpeed = 0.5f;

		public void Run()
		{
			for (int i = 0; i < rightLogs.Length; i++)
			{
				int idx = i;
				rightLogs[i].transform.DOMove(rightLogs[i].transform.position + Vector3.forward * -3f, logSpeed).SetLoops(-1, LoopType.Yoyo);
				rightLogs[idx].GetComponentInChildren<BoxCollider>().OnTriggerEnterAsObservable()
					.Where(x => x.tag == "Monster")
					.First()
					.Subscribe(_ => 
					{
						rightEffects[idx].Play();
					}).AddTo(this);
			}
			for (int i = 0; i < leftLogs.Length; i++)
			{
				int idx = i;
				leftLogs[i].transform.DOMove(leftLogs[i].transform.position + Vector3.forward * 3f, logSpeed).SetLoops(-1, LoopType.Yoyo);
				leftLogs[idx].GetComponentInChildren<BoxCollider>().OnTriggerEnterAsObservable()
					.Where(x => x.tag == "Monster")
					.First()
					.Subscribe(_ =>
					{
						leftEffects[idx].Play();
					}).AddTo(this);
			}
		}
	}
}
