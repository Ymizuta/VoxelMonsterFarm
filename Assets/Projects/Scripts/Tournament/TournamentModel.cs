using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Voxel.Tournament
{
	public class TournamentModel : ModelBase
	{
		public enum CommandType
		{
			None,
			Initialize,
			TopMenu,
			TournamentBoard,
			MainLoop,
			Ending,
		}

		public enum TopMenuType
		{
			Match = 0, // ����
			AbstentionNextMatch, // ���̎���������
			AbstentionTournament, // ������
		}

		public ReactiveProperty<CommandType> Command = new ReactiveProperty<CommandType>();

		public TournamentMonsterParam[] MonsterParams => TournamentCommonModel.Instance.MonsterParams;
		public string[] MenuStrs { get; private set; } = new string[] {"����", "����", "������", };

		public bool IsPlayerTurn { get; set; } // �v���C���[�I��
		public bool IsAbstentionNextMatch = false; // ���̎���������
		public bool IsAbstentionTournament = false; // ��������

		public int CurrentMonsterIdx { get { return TournamentCommonModel.Instance.MatchOrderList[TournamentCommonModel.Instance.CurrentRotationMatchIdx]; } }
		public int CounterMonsterIdx { get { return 
					TournamentCommonModel.Instance.MatchOrderList[TournamentCommonModel.Instance.CurrentRotationMatchIdx 
					+ TournamentCommonModel.Instance.MatchOrderList.Count / 2]; } }

		public bool IsOperatable { get
			{
				return !GameCommonModel.Instance.IsPopupShowed;
			} }

		public override void Initialize()
		{
			base.Initialize();
		}

		/// <summary>
		/// �ł������������������X�^�[�p������Ԃ�
		/// </summary>
		/// <returns></returns>
		public TournamentMonsterParam GetTournamentWinner()
		{
			var winnerIdx = -1;
			var maxCount = 0;
			for (int i = 0; i < TournamentCommonModel.Instance.Results.Length; i++)
			{
				var winCount = 0;
				for (int j = 0; j < TournamentCommonModel.Instance.Results[0].Length; j++)
				{
					if (TournamentCommonModel.Instance.Results[i][j] == ResultType.Win) winCount++;
				}
				if (maxCount < winCount)
				{
					maxCount = winCount;
					winnerIdx = i;
				}
			}
			return TournamentCommonModel.Instance.GetMonsterParam(winnerIdx);
		}

		/// <summary>
		/// �����̃����X�^�[�̏��ԂȂ�true��Ԃ�
		/// </summary>
		/// <returns></returns>
		public bool IsMyMonsterTurn()
		{
			return CurrentMonsterIdx == 0;
		}

		public void SetResult(int winnerIdx, int loserIdx)
		{
			TournamentCommonModel.Instance.SetResult(winnerIdx, loserIdx);		
		}

		/// <summary>
		/// �S�����I�����Ă����true��Ԃ�
		/// </summary>
		/// <returns></returns>
		public bool IsEndTournament()
		{
			return TournamentCommonModel.Instance.IsEndTournament();
		}

		/// <summary>
		/// �����̃C���f�b�N�X�����ɐi�߂�
		/// </summary>
		public void IncrementMatchIdx()
		{
			TournamentCommonModel.Instance.CurrentRotationMatchIdx++;
			if (IsEndCurrentCycle())
			{
				TournamentCommonModel.Instance.UpdateMatchOrderList();
			}
		}

		/// <summary>
		/// ���̃��[�e�[�V�����̎������ꏄ���Ă����true��Ԃ�
		/// </summary>
		/// <returns></returns>
		private bool IsEndCurrentCycle()
		{
			return TournamentCommonModel.Instance.IsEndCurrentCycle();
		}
	}
}
