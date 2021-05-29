using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Battle
{
	public class BattleMonsterEffects : MonoBehaviour
	{
		[SerializeField] ParticleSystem attackedEffect;
		[SerializeField] ParticleSystem guradEffect;
		[SerializeField] ParticleSystem chargeEffect;

		public void PlayAttacked()
		{
			attackedEffect.Play();
		}

		public void PlayCharge()
		{
			chargeEffect.Play();
		}
	}
}
