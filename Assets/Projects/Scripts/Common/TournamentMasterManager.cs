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
			// �z��̏�����
			var datas = new TournamentData[System.Enum.GetNames(typeof(TournamentGrade)).Length - 1][];
			for (int i = 0; i < datas.Length; i++)
			{
				datas[i] = new TournamentData[4];
				for (int j = 0; j < datas[i].Length; j++)
				{
					datas[i][j] = new TournamentData() { Grade = TournamentGrade.None };
				}
			}
			// ���Y���̃f�[�^������
			var list = CommonMasterManager.Instance.TournamentParamMater.sheets[0].list.FindAll(x => x.month == month);
			for (int i = 0; i < list.Count; i++)
			{
				var data = list[i];
				var grade = data.grade;
				var week = data.week;
				datas[grade][week - 1] = new TournamentData() { Grade = (TournamentGrade)grade, MonsterCount = data.monsterCount, TournamentName = data.tournamentName, Week = week };
			}
			return datas;
		}
	}
}
