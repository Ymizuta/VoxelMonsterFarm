using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.SceneManagement;

namespace Voxel.Tournament
{
	public class TournamentSceneData : SceneData
	{
		public TournamentSceneData(int tournamentId)
		{
			this.TournamentId = tournamentId;
			this.IsInitialize = true;
		}

		public TournamentSceneData(int winnerIdx, int loserIdx)
		{
			this.WinMonsterIdx = winnerIdx;
			this.LoseMonsterIdx = loserIdx;
		}
		public bool IsInitialize { get; private set; }
		public int TournamentId { get; private set; }
		public int WinMonsterIdx { get; private set; }
		public int LoseMonsterIdx { get; private set; }
	}
}
