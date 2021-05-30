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
		/// ������
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
			Training = 0, // �琬
			TakeRest, // �x�{
			Tournament, // ���
			Params,
		}

		public enum TrainingMenu
		{
			Running = 0, // ���荞��
			Domino, // �h�~�m�|��
			Shooting, // �˓I
			Studying, // �׋�
			AvoidRock, // �����
			LogShock, // �ۑ���
		}

		public ReactiveProperty<CommandType> Command = new ReactiveProperty<CommandType>();

		public readonly string[] FarmTopMenuStrs = new string[] { "�琬", "�x�{", "���", "�\�͒l"};
		public readonly string[] TrainingMenuStrs = new string[] { "���荞��", "�h�~�m�|��", "����Ă�", "�ҕ׋�", "���Δ���", "�ۑ���" };

		public MonsterParam MonsterParam { get; private set; }
		public bool IsOperatable { get
			{
				return !GameCommonModel.Instance.IsPopupShowed;
			} }

		// todo MVP�𕪂��������悢
		// �\��\�ɌW��f�[�^
		public TournamentData[][] ScheduleDatas { get; private set; } = new TournamentData[3][]
		{
			new TournamentData[4],
			new TournamentData[4],
			new TournamentData[4],
		};
		public ReactiveProperty<int> SelectedMonth { get; private set; } = new ReactiveProperty<int>(1);
		public ReactiveProperty<TournamentGrade> SelectedGrade { get; private set; } = new ReactiveProperty<TournamentGrade>(TournamentGrade.C);
		public ReactiveProperty<int> SelectedWeek { get; private set; } = new ReactiveProperty<int>(1);
		// �I�𒆂̃X�P�W���[���̃f�[�^��Ԃ�
		public TournamentData SelectedScheduleData => ScheduleDatas[(int)SelectedGrade.Value][SelectedWeek.Value - 1];

		public override void Initialize()
		{
			base.Initialize();
			MonsterParam = SaveDataManager.SaveData.CurrentMonster;

			// ���̗\��\�f�[�^��������
			for (int i = 0; i < ScheduleDatas.Length; i++)
			{
				for (int j = 0; j < ScheduleDatas[0].Length; j++)
				{
					ScheduleDatas[i][j] = new TournamentData() { Grade = TournamentGrade.None};
				}
			}
			ScheduleDatas[(int)TournamentGrade.A][1 - 1] = new TournamentData() { Grade = TournamentGrade.A, Week = 1, TournamentName = "�O���[�hA��ʐ�", MonsterCount = 4 };
			ScheduleDatas[(int)TournamentGrade.B][2 - 1] = new TournamentData() { Grade = TournamentGrade.B, Week = 2, TournamentName = "�O���[�hB������", MonsterCount = 4 };
			ScheduleDatas[(int)TournamentGrade.C][3 - 1] = new TournamentData() { Grade = TournamentGrade.C, Week = 3, TournamentName = "�O���[�hC��ʐ�", MonsterCount = 4 };
			ScheduleDatas[(int)TournamentGrade.A][4 - 1] = new TournamentData() { Grade = TournamentGrade.A, Week = 4, TournamentName = "�O���[�hA������", MonsterCount = 6 };
		}

		public string GetInitComment()
		{
			return new FarmCommentCalculator().GetConditionComment(MonsterParam.MonsterName, new MonsterCalculator().CalcCondition(MonsterParam.Fatigue));
		}

		#region Schedule
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
