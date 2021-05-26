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
	/// 大会シーンや戦闘シーンの間でデータを共有するためのクラス
	/// </summary>
	public class TournamentCommonModel : SingletonMonoBehaviour<TournamentCommonModel>
	{
		public TournamentMonsterParam[] MonsterParams { get; set; } = new TournamentMonsterParam[]
			{
				new TournamentMonsterParam("イッヌ", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("ネッコ", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("ウッシ", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("トッリ", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("ブッタ", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("ヒツッジ", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("ヤッギ", 100, 30, 30, 25, 20, 45),
				new TournamentMonsterParam("ドラゴン", 100, 30, 30, 25, 20, 45),
			};

		public ResultType[][] Results { get; private set; }

		public void Initialize()
		{
			InitResult();
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
		/// 勝敗を登録する
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
