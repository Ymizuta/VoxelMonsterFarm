using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.Farm;

namespace Voxel.Common
{
	public static class TournamentMasterManager
	{
		public static TournamentData[][] GetTournamentDatas(int month)
		{
			// 配列の初期化
			var datas = new TournamentData[System.Enum.GetNames(typeof(TournamentGrade)).Length - 1][];
			for (int i = 0; i < datas.Length; i++)
			{
				datas[i] = new TournamentData[4];
				for (int j = 0; j < datas[i].Length; j++)
				{
					datas[i][j] = new TournamentData() { Grade = TournamentGrade.None };
				}
			}
			// 当該月のデータを検索
			var list = CommonMasterManager.Instance.TournamentParamMater.sheets[0].list.FindAll(x => x.month == month);
			for (int i = 0; i < list.Count; i++)
			{
				var data = list[i];
				var grade = data.grade;
				var week = data.week;
				datas[grade][week - 1] = new TournamentData() { TournamentId = data.id, Grade = (TournamentGrade)grade, MonsterCount = data.monsterCount, TournamentName = data.tournamentName, Week = week };
			}
			return datas;
		}

		public static TournamentData GetTournamentData(int id)
		{
			var data = CommonMasterManager.Instance.TournamentParamMater.sheets[0].list.Find(x => x.id == id);
			return new TournamentData() { TournamentId = id, Grade = (TournamentGrade)data.grade, TournamentName = data.tournamentName, MonsterCount = data.monsterCount, Week = data.week };
		}
	}
}
