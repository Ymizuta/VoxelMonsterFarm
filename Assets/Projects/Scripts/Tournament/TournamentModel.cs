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
			Match = 0, // 試合
			AbstentionNextMatch, // 次の試合を棄権
			AbstentionTournament, // 大会棄権
		}

		public ReactiveProperty<CommandType> Command = new ReactiveProperty<CommandType>();

		public TournamentMonsterParam[] MonsterParams => TournamentCommonModel.Instance.MonsterParams;
		public string[] MenuStrs { get; private set; } = new string[] {"試合", "棄権", "大会棄権", };

		public bool IsPlayerTurn { get; set; } // プレイヤー選択中
		public bool IsAbstentionNextMatch = false; // 次の試合を棄権
		public bool IsAbstentionTournament = false; // 大会を棄権

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
		/// 最も勝利数が多いモンスターパラムを返す
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
		/// 自分のモンスターの順番ならtrueを返す
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
		/// 全試合終了していればtrueを返す
		/// </summary>
		/// <returns></returns>
		public bool IsEndTournament()
		{
			return TournamentCommonModel.Instance.IsEndTournament();
		}

		/// <summary>
		/// 試合のインデックスを次に進める
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
		/// 今のローテーションの試合が一巡していればtrueを返す
		/// </summary>
		/// <returns></returns>
		private bool IsEndCurrentCycle()
		{
			return TournamentCommonModel.Instance.IsEndCurrentCycle();
		}
	}
}
