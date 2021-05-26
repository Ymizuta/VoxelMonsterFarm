using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel
{
	[System.Serializable]
	public  class SaveData
	{
		public string BreederName { get; set; }
		public int BreederGrade { get; set; }
		public int Money { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int Week { get; set; }
		public int MatchCount { get; set; } // 試合数
		public int WinMatchCount { get; set; } // 処理試合数
		public int WinTournamentCount { get; set; } // 優勝大会数
		public int[] MonsterGradeCount { get; set; } // 育成モンスターの最終グレード情報
		public List<MonsterParam> Monsters { get; set; } // 所有モンスター
		public int CurrentMonsterId;

		public static SaveData Default => new SaveData
		{
			BreederName = "ブリーダー",
			BreederGrade = 0,
			Money = 0,
			Year = 0,
			Month = 0,
			Week = 0,
			MatchCount = 0,
			WinMatchCount = 0,
			WinTournamentCount = 0,
			//MonsterGradeCount = new int[0],
			Monsters = new List<MonsterParam>(),
		};
	}
}
