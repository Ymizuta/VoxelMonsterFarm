using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel
{
	[System.Serializable]
	public  class SaveData
	{
		[SerializeField] private string breederName;
		[SerializeField] private int breederGrade;
		[SerializeField] private int money;
		[SerializeField] private int year;
		[SerializeField] private int month;
		[SerializeField] private int week;
		[SerializeField] private int matchCount;
		[SerializeField] private int winMatchCount;
		[SerializeField] private int winTournamentCount;
		//[SerializeField] private int[] monsterGradeCount;
		[SerializeField] private MonstersWrapper wrapper;
		[SerializeField] private int currentMonsterId;

		public string BreederName { get { return breederName; } set { breederName = value; } }
		public int BreederGrade { get { return breederGrade; } set { breederGrade = value; } }
		public int Money { get { return money; } set { money = value; } }
		public int Year { get { return year; } set { year = value; } }
		public int Month { get { return money; } set { money = value; } }
		public int Week { get { return week; } set { week = value; } }
		public int MatchCount { get { return matchCount; } set{ matchCount = value; } } // 試合数
		public int WinMatchCount { get { return winMatchCount; } set { winMatchCount = value; } } // 処理試合数
		public int WinTournamentCount { get { return winTournamentCount; } set { winTournamentCount = value; } } // 優勝大会数
		//public int[] MonsterGradeCount { get; set; } // 育成モンスターの最終グレード情報
		public MonstersWrapper Wrapper { get { return wrapper; } private set { wrapper = value; } }
		public int CurrentMonsterId { get { return currentMonsterId; } set { currentMonsterId = value; } }

		public MonsterParam CurrentMonster => Wrapper.Monsters.Find(x => x.MonseterId == SaveDataManager.SaveData.CurrentMonsterId);

		public static SaveData Default => new SaveData
		{
			BreederName = "ブリーダー",
			BreederGrade = 0,
			Money = 0,
			Year = 0,
			Month = 1,
			Week = 1,
			MatchCount = 0,
			WinMatchCount = 0,
			WinTournamentCount = 0,
			//MonsterGradeCount = new int[0],
			Wrapper = new MonstersWrapper(),
		};
	}

	[System.Serializable]
	public class MonstersWrapper
	{
		public List<MonsterParam> Monsters = new List<MonsterParam>();
	}

}
