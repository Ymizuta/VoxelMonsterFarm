using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.UI;
using Voxel.SceneManagement;

namespace Voxel
{
	public class CalendarManager : SingletonMonoBehaviour<CalendarManager>
	{
		public void NextWeek()
		{
			if(GameCommonModel.Instance.Week.Value == 4)
			{
				NextMonth();
			}
			else
			{
				GameCommonModel.Instance.Week.Value++;
			}
			SaveDataManager.SaveData.CurrentMonster.LivingWeek++;
			SaveDataManager.Save();
		}

		private void NextMonth()
		{
			if (GameCommonModel.Instance.Month.Value == 12) NextYear();
			else
			{
				GameCommonModel.Instance.Month.Value++;
				GameCommonModel.Instance.Week.Value = 1;
			}
		}

		private void NextYear()
		{
			GameCommonModel.Instance.Year.Value++;
			GameCommonModel.Instance.Month.Value = 1;
			GameCommonModel.Instance.Week.Value = 1;
		}
	}
}
