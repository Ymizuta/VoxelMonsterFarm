using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.Common;

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
		public int TournamentId { get; private set; }
		public TournamentGrade Grade { get; private set; }
		public string TournamentName { get; private set; }
		public TournamentMonsterParam[] MonsterParams { get; set; } // �Q�������X�^�[
		public ResultType[][] Results { get; private set; }
		public List<int> MatchOrderList { get; private set; }
		public int RotationCount { get; private set; } // ���݂̃��[�e�[�V������
		public int CurrentRotationMatchIdx { get; set; } // ���݂̃��[�e�[�V�����ł̎�����
		public int MaxRotationCount { get; private set; } // �S���[�e�[�V������
		public int MatchCountPerRotation { get; private set; } // �P���[�e�[�V����������̎�����

		public void Initialize(int tournamentId)
		{
			var data = TournamentMasterManager.GetTournamentData(tournamentId);
			TournamentId = tournamentId;
			Grade = data.Grade;
			TournamentName = data.TournamentName;
			InitMonsterParam(data);
			InitResult();
			InitMatchOrderList();
		}

		public bool IsMyMonsterWin(int idx)
		{
			return idx == 0;
		}

		/// <summary>
		/// �Q�������X�^�[�̏�����
		/// </summary>
		/// <param name="tournamentId"></param>
		private void InitMonsterParam(Farm.TournamentData data)
		{
			// �ŏ��Ɏ����̃����X�^�[�͊m��œo�^
			MonsterParams = new TournamentMonsterParam[data.MonsterCount];
			MonsterParams[0] = new MonsterTournamentParamConvertor().ConvertMonsterParamToTournamentParam(SaveDataManager.SaveData.CurrentMonster);
			// ���̃p�������擾
			var candidats = MonsterParamMasterManager.GetMonsterParams(data.Grade);
			// ���I���s��
			var count = data.MonsterCount - 1; // �����̃����X�^�[�͕K���Q������̂�-1����
			var idx = 1;
			while (count-- > 0)
			{
				var ransu = UnityEngine.Random.Range(0, candidats.Count);
				MonsterParams[idx] = MonsterParamMasterManager.GetTournamentParam(candidats[ransu].MonseterId);
				candidats.RemoveAt(ransu);
				idx++;
			}
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
		/// �����̏��Ԃɂ�������̏�����
		/// </summary>
		private void InitMatchOrderList()
		{
			MatchOrderList = new List<int>();
			for (int i = 0; i < MonsterParams.Length / 2; i++) MatchOrderList.Add(i);
			for (int i = MonsterParams.Length - 1; MonsterParams.Length / 2 <= i; i--) MatchOrderList.Add(i);
			// ���[�e�[�V������
			MaxRotationCount = MatchOrderList.Count - 1;
			RotationCount = 1;
			// ���[�e�[�V�������Ƃ̎�����
			MatchCountPerRotation = MatchOrderList.Count / 2;
			CurrentRotationMatchIdx = 0;
		}

		/// <summary>
		/// �������Ԃ̃��[�e�[�V�������X�V
		/// </summary>
		public void UpdateMatchOrderList()
		{
			RotationCount++;
			CurrentRotationMatchIdx = 0;
			// �g�ݍ��킹���X�g�̍X�V
			var firstMoveVal = MatchOrderList[MatchOrderList.Count / 2];
			var secondMoveVal = MatchOrderList[MatchOrderList.Count / 2 - 1];
			MatchOrderList.Remove(firstMoveVal);
			MatchOrderList.Insert(1, firstMoveVal);
			MatchOrderList.Remove(secondMoveVal);
			MatchOrderList.Add(secondMoveVal);
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

		/// <summary>
		/// �S�����I�����Ă��邩
		/// </summary>
		/// <returns></returns>
		public bool IsEndTournament()
		{
			return MaxRotationCount < RotationCount;
		}

		/// <summary>
		/// ���̃��[�e�[�V�����̎������ꏄ���Ă��邩
		/// </summary>
		/// <returns></returns>
		public bool IsEndCurrentCycle()
		{
			return MatchCountPerRotation <= CurrentRotationMatchIdx;
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�̃����X�^�[�p������Ԃ�
		/// </summary>
		/// <param name="monsterIdx"></param>
		/// <returns></returns>
		public TournamentMonsterParam GetMonsterParam(int monsterIdx)
		{
			return MonsterParams[monsterIdx];
		}
	}
}
