using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Battle
{
	public class BattleManager : MonoBehaviour
	{
		public void Initialize(SceneManagement.SceneData data)
		{
			GetComponent<BattleStateMachine>().Initialize(data);
		}
	}
}
