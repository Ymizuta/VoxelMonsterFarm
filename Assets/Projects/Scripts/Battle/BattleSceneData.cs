using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.SceneManagement;

namespace Voxel.Battle
{
	public class BattleSceneData : SceneData
	{
		public BattleSceneData(int currentMonsterIdx, int counterMonsterIdx)
		{
			this.CurrentMonsterIdx = currentMonsterIdx;
			this.CounterMonsterIdx = counterMonsterIdx;
		}

		public int CurrentMonsterIdx;
		public int CounterMonsterIdx;
	}
}
