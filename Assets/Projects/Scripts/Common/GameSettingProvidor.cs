using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Common
{
	public class GameSettingProvidor : SingletonMonoBehaviour<GameSettingProvidor>
	{
		[Header("�g���[�j���O�ő������J")]
		[SerializeField] private float trainingFatigue;
		[Header("�x�{�Ō����J")]
		[SerializeField] private float takeRestRemovedFatigue;

		public float TrainingFatigue => trainingFatigue;
		public float TakeRestRemovedFatigue => takeRestRemovedFatigue;
	}
}
