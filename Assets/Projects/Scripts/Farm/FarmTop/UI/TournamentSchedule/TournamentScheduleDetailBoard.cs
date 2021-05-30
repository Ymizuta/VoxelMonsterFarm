using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Farm
{
	public class TournamentScheduleDetailBoard : MonoBehaviour
	{
		[SerializeField] private Text timeText;
		[SerializeField] private Text titleText;
		[SerializeField] private Text gradeText;
		[SerializeField] private Text monsterCountText;

		public void SetData(int month, TournamentData data)
		{
			if(data.Grade == TournamentGrade.None)
			{
				timeText.text = "-";
				titleText.text = "-";
				gradeText.text = "-";
				monsterCountText.text = "-";
			}
			else
			{
				timeText.text = $"{month}��{data.Week}�T";
				titleText.text = data.TournamentName;
				gradeText.text = $"�O���[�h {data.Grade}";
				monsterCountText.text = $"�Q������ {data.MonsterCount}��";
			}
		}
	}
}
