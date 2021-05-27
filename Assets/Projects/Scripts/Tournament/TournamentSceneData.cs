using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.SceneManagement;

namespace Voxel.Tournament
{
	public class TournamentSceneData : SceneData
	{
		public TournamentSceneData(int winnerIdx, int loserIdx)
		{
			this.WinMonsterIdx = winnerIdx;
			this.LoseMonsterIdx = loserIdx;
		}
		public int WinMonsterIdx { get; private set; }
		public int LoseMonsterIdx { get; private set; }
	}
}
