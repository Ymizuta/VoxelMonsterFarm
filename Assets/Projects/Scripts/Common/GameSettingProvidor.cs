using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Common
{
	public class GameSettingProvidor : SingletonMonoBehaviour<GameSettingProvidor>
	{
		[Header("ƒgƒŒ[ƒjƒ“ƒO‚Å‘‚¦‚é”æ˜J")]
		[SerializeField] private float trainingFatigue;
		[Header("‹x—{‚ÅŒ¸‚é”æ˜J")]
		[SerializeField] private float takeRestRemovedFatigue;

		public float TrainingFatigue => trainingFatigue;
		public float TakeRestRemovedFatigue => takeRestRemovedFatigue;
	}
}
