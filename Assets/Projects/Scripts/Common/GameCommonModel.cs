using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Voxel
{
	public class GameCommonModel : SingletonMonoBehaviour<GameCommonModel>
	{
		public ReactiveProperty<int> Year { get; set; } = new ReactiveProperty<int>();
		public ReactiveProperty<int> Month { get; set; } = new ReactiveProperty<int>();
		public ReactiveProperty<int> Week { get; set; } = new ReactiveProperty<int>();

		// êßå‰ä÷òA
		public bool IsPopupShowed { get; set; } = false;

		public void SetCalendar()
		{
			Year.Value = SaveDataManager.SaveData.Year;
			Month.Value = SaveDataManager.SaveData.Month;
			Week.Value = SaveDataManager.SaveData.Week;
		}
	}
}
