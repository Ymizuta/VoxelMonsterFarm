using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Training
{
	public class TrainingMonster : MonoBehaviour
	{
		[SerializeField] private Animator animator;

		public void PlayAttackAnimation()
		{
			animator.Play("Attack");
		}

		public IEnumerator PlayAttackAnimationCoroutine()
		{
			animator.Play("Attack");
			yield return null;
			yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
		}
	}
}
