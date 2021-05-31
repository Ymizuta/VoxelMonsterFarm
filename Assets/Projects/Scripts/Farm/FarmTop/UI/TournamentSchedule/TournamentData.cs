using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Farm
{
	public class TournamentData
	{
		public int TournamentId { get; set; }
		public TournamentGrade Grade { get; set; }
		public int Month { get; set; }
		public int Week { get; set; }
		public string TournamentName { get; set; }
		public int MonsterCount { get; set; }
	}
}
