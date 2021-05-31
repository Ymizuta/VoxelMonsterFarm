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
	/// 大会シーンや戦闘シーンの間でデータを共有するためのクラス
	/// </summary>
	public class TournamentCommonModel : SingletonMonoBehaviour<TournamentCommonModel>
	{
		public int TournamentId { get; private set; }
		public TournamentGrade Grade { get; private set; }
		public string TournamentName { get; private set; }
		public TournamentMonsterParam[] MonsterParams { get; set; } // 参加モンスター
		public ResultType[][] Results { get; private set; }
		public List<int> MatchOrderList { get; private set; }
		public int RotationCount { get; private set; } // 現在のローテーション数
		public int CurrentRotationMatchIdx { get; set; } // 現在のローテーションでの試合数
		public int MaxRotationCount { get; private set; } // 全ローテーション数
		public int MatchCountPerRotation { get; private set; } // １ローテーションあたりの試合数

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
		/// 参加モンスターの初期化
		/// </summary>
		/// <param name="tournamentId"></param>
		private void InitMonsterParam(Farm.TournamentData data)
		{
			// 最初に自分のモンスターは確定で登録
			MonsterParams = new TournamentMonsterParam[data.MonsterCount];
			MonsterParams[0] = new MonsterTournamentParamConvertor().ConvertMonsterParamToTournamentParam(SaveDataManager.SaveData.CurrentMonster);
			// 候補のパラムを取得
			var candidats = MonsterParamMasterManager.GetMonsterParams(data.Grade);
			// 抽選を行う
			var count = data.MonsterCount - 1; // 自分のモンスターは必ず参加するので-1する
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
		/// 勝敗表データの初期化
		/// </summary>
		private void InitResult()
		{
			Results = new ResultType[MonsterParams.Length][];
			for (int i = 0; i < Results.Length; i++)
			{
				Results[i] = new ResultType[Results.Length];
				for (int j = 0; j < MonsterParams.Length; j++) Results[i][j] = ResultType.None;
				// 自分の枠にはスラッシュを入れる
				Results[i][i] = ResultType.Self;
			}
		}

		/// <summary>
		/// 試合の順番にかかる情報の初期化
		/// </summary>
		private void InitMatchOrderList()
		{
			MatchOrderList = new List<int>();
			for (int i = 0; i < MonsterParams.Length / 2; i++) MatchOrderList.Add(i);
			for (int i = MonsterParams.Length - 1; MonsterParams.Length / 2 <= i; i--) MatchOrderList.Add(i);
			// ローテーション数
			MaxRotationCount = MatchOrderList.Count - 1;
			RotationCount = 1;
			// ローテーションごとの試合数
			MatchCountPerRotation = MatchOrderList.Count / 2;
			CurrentRotationMatchIdx = 0;
		}

		/// <summary>
		/// 試合順番のローテーションを更新
		/// </summary>
		public void UpdateMatchOrderList()
		{
			RotationCount++;
			CurrentRotationMatchIdx = 0;
			// 組み合わせリストの更新
			var firstMoveVal = MatchOrderList[MatchOrderList.Count / 2];
			var secondMoveVal = MatchOrderList[MatchOrderList.Count / 2 - 1];
			MatchOrderList.Remove(firstMoveVal);
			MatchOrderList.Insert(1, firstMoveVal);
			MatchOrderList.Remove(secondMoveVal);
			MatchOrderList.Add(secondMoveVal);
		}

		/// <summary>
		/// 勝敗を登録する
		/// </summary>
		/// <param name="winnerIdx"></param>
		/// <param name="loserIdx"></param>
		public void SetResult(int winnerIdx, int loserIdx)
		{
			Results[winnerIdx][loserIdx] = ResultType.Win;
			Results[loserIdx][winnerIdx] = ResultType.Lose;
		}

		/// <summary>
		/// 全試合終了しているか
		/// </summary>
		/// <returns></returns>
		public bool IsEndTournament()
		{
			return MaxRotationCount < RotationCount;
		}

		/// <summary>
		/// 今のローテーションの試合が一巡しているか
		/// </summary>
		/// <returns></returns>
		public bool IsEndCurrentCycle()
		{
			return MatchCountPerRotation <= CurrentRotationMatchIdx;
		}

		/// <summary>
		/// 指定したインデックスのモンスターパラムを返す
		/// </summary>
		/// <param name="monsterIdx"></param>
		/// <returns></returns>
		public TournamentMonsterParam GetMonsterParam(int monsterIdx)
		{
			return MonsterParams[monsterIdx];
		}
	}
}
