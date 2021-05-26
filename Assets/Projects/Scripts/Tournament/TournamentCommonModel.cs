using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Tournament
{
	public enum ResultType
	{
		Win = 0,
		Lose,
		Self,
		None,
	}

	/// <summary>
	/// ���V�[����퓬�V�[���̊ԂŃf�[�^�����L���邽�߂̃N���X
	/// </summary>
	public class TournamentCommonModel : SingletonMonoBehaviour<TournamentCommonModel>
	{
		public TournamentMonsterParam[] MonsterParams { get; set; } = new TournamentMonsterParam[]
			{
				new TournamentMonsterParam("�C�b�k", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("�l�b�R", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("�E�b�V", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("�g�b��", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("�u�b�^", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("�q�c�b�W", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("���b�M", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("�h���S��", 100, 30, 30, 25, 20, 45),
			};

		public ResultType[][] Results { get; private set; }

		public void Initialize()
		{
			InitResult();
		}

		/// <summary>
		/// ���s�\�f�[�^�̏�����
		/// </summary>
		private void InitResult()
		{
			Results = new ResultType[MonsterParams.Length][];
			for (int i = 0; i < Results.Length; i++)
			{
				Results[i] = new ResultType[Results.Length];
				for (int j = 0; j < MonsterParams.Length; j++) Results[i][j] = ResultType.None;
				// �����̘g�ɂ̓X���b�V��������
				Results[i][i] = ResultType.Self;
			}
		}

		/// <summary>
		/// ���s��o�^����
		/// </summary>
		/// <param name="winnerIdx"></param>
		/// <param name="loserIdx"></param>
		public void SetResult(int winnerIdx, int loserIdx)
		{
			Results[winnerIdx][loserIdx] = ResultType.Win;
			Results[loserIdx][winnerIdx] = ResultType.Lose;
		}
	}
}
