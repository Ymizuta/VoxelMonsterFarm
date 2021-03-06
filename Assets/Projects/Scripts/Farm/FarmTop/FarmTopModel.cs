using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Voxel.UI;
using Voxel.Common;

namespace Voxel.Farm
{
	public class FarmTopModel : ModelBase
	{
		/// <summary>
		/// 操作状態
		/// </summary>
		public enum CommandType
		{
			None,
			FarmTopMenu,
			TrainingMenu,
			MonsterParam,
			Schedule,
		}

		public enum FarmTopMenu
		{
			Training = 0, // 育成
			TakeRest, // 休養
			Tournament, // 大会
			Params,
		}

		public enum TrainingMenu
		{
			Running = 0, // 走り込み
			Domino, // ドミノ倒し
			Shooting, // 射的
			Studying, // 勉強
			AvoidRock, // 岩避け
			LogShock, // 丸太受け
		}

		public ReactiveProperty<CommandType> Command = new ReactiveProperty<CommandType>();

		public readonly string[] FarmTopMenuStrs = new string[] { "育成", "休養", "大会", "能力値"};
		public readonly string[] TrainingMenuStrs = new string[] { "走り込み", "ドミノ倒し", "しゃてき", "猛勉強", "巨石避け", "丸太受け" };

		public MonsterParam MonsterParam { get; private set; }
		public bool IsOperatable { get
			{
				return !GameCommonModel.Instance.IsPopupShowed;
			} }

		// todo MVPを分けた方がよい
		// 予定表に係るデータ
		public TournamentData[][] ScheduleDatas { get; private set; }
		public ReactiveProperty<int> SelectedMonth { get; private set; } = new ReactiveProperty<int>(1);
		public ReactiveProperty<TournamentGrade> SelectedGrade { get; private set; } = new ReactiveProperty<TournamentGrade>(TournamentGrade.C);
		public ReactiveProperty<int> SelectedWeek { get; private set; } = new ReactiveProperty<int>(1);
		// 選択中のスケジュールのデータを返す
		public TournamentData SelectedScheduleData => ScheduleDatas[(int)SelectedGrade.Value][SelectedWeek.Value - 1];

		public override void Initialize()
		{
			base.Initialize();
			MonsterParam = SaveDataManager.SaveData.CurrentMonster;

			// 予定表データを初期化
			MonthDataUpdate();
		}

		public string GetInitComment()
		{
			return new FarmCommentCalculator().GetConditionComment(MonsterParam.MonsterName, new MonsterCalculator().CalcCondition(MonsterParam.Fatigue));
		}

		#region Schedule
		public bool CanEntryTournament()
		{
			return
				MonsterParam.Grade <= SelectedScheduleData.Grade
				&& SelectedScheduleData.Month == GameCommonModel.Instance.Month.Value
				&& SelectedScheduleData.Week == GameCommonModel.Instance.Week.Value;
		}
		public void MonthDataUpdate()
		{
			ScheduleDatas = TournamentMasterManager.GetTournamentDatas(SelectedMonth.Value);
		}
		public void SelectUpTournamentSchedule()
		{
			if (SelectedGrade.Value == TournamentGrade.A) return;
			SelectedGrade.Value--;
		}
		public void SelectDownTournamentSchedule()
		{
			if (SelectedGrade.Value == TournamentGrade.C) return;
			SelectedGrade.Value++;
		}
		public void SelectLeftTournamentSchedule()
		{
			var val = SelectedWeek.Value - 1;
			if (val < 1)
			{
				SelectedWeek.Value = 4;
				PrevMonth();
			}
			else
			{
				SelectedWeek.Value = val;
			}
		}
		public void SelectRightTournamentSchedule()
		{
			var val = SelectedWeek.Value + 1;
			if (4 < val)
			{
				SelectedWeek.Value = 1;
				NextMonth();
			}
			else
			{
				SelectedWeek.Value = val;
			}
		}
		private void NextMonth()
		{
			var val = SelectedMonth.Value + 1;
			if (12 < val) val = 1;
			SelectedMonth.Value = val;
		}
		private void PrevMonth()
		{
			var val = SelectedMonth.Value - 1;
			if (val < 1) val = 12;
			SelectedMonth.Value = val;
		}
		#endregion

		protected override void OnBack()
		{
			base.OnBack();
		}
	}
}
