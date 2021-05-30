using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Farm
{
	public enum TournamentGrade
	{
		None = -1,
		A = 0,
		B,
		C,
	}

	public class TournamentSchedule : MonoBehaviour
	{
		[SerializeField] private Text monthText;
		[SerializeField] private Transform scheduleRoot;
		[SerializeField] private GameObject[] items;
		[SerializeField] private RectTransform cursor;
		[SerializeField] private TournamentScheduleDetailBoard detail;

		private const int WeekCount = 4;

		private TournamentScheduleItem[][] scheduleItems;
		private int month;

		public void Initialize()
		{
			InitItemArray();
		}

		private void InitItemArray()
		{
			// ジャグ配列を初期化
			var gradeCount = 3;
			var gradeIdx = 0;
			var weekIdx = 0;
			scheduleItems = new TournamentScheduleItem[gradeCount][];
			for (int i = 0; i < scheduleItems.Length; i++) scheduleItems[i] = new TournamentScheduleItem[WeekCount];
			for (int i = 0; i < items.Length; i++)
			{
				if (weekIdx == WeekCount)
				{
					gradeIdx++;
					weekIdx = 0;
				}
				scheduleItems[gradeIdx][weekIdx] = items[i].GetComponent<TournamentScheduleItem>();
				weekIdx++;
			}
		}

		public void Show()
		{
			scheduleRoot.gameObject.SetActive(true);
		}

		public void Show(int month, TournamentData[][] datas)
		{
			UpdateShedule(month, datas);
			Show();
		}

		public void Hide()
		{
			scheduleRoot.gameObject.SetActive(false);
		}

		/// <summary>
		/// 月の更新
		/// </summary>
		public void UpdateShedule(int month,TournamentData[][] datas)
		{
			this.month = month;
			monthText.text = $"{month}月";
			ResetSchedule();
			for (int i = 0; i < datas.Length; i++)
			{
				for (int j = 0; j < datas[0].Length; j++)
				{
					scheduleItems[i][j].SetData(datas[i][j].Grade);
				}
			}
		}

		/// <summary>
		/// 渡されたインデックスの大会パネルを選択
		/// </summary>
		/// <param name="grade"></param>
		/// <param name="week"></param>
		public void Select(TournamentGrade grade, int week, TournamentData data)
		{
			UnselectAllPanels();
			scheduleItems[(int)grade][week - 1].Select();
			cursor.position = scheduleItems[(int)grade][week - 1].Rtf.position;
			detail.SetData(this.month, data);
		}

		/// <summary>
		/// 全てのパネルを非選択にする
		/// </summary>
		private void UnselectAllPanels()
		{
			for (int i = 0; i < scheduleItems.Length; i++)
			{
				for (int j = 0; j < scheduleItems[0].Length; j++)
				{
					scheduleItems[i][j].UnSelect();
				}
			}
		}

		/// <summary>
		/// 全てのパネルに表示されている情報をリセットする
		/// </summary>
		private void ResetSchedule()
		{
			for (int i = 0; i < scheduleItems.Length; i++)
			{
				for (int j = 0; j < scheduleItems[0].Length; j++)
				{
					scheduleItems[i][j].ResetData();
				}
			}
		}
	}
}
