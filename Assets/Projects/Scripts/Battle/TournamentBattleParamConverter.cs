using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Battle
{
	public class TournamentBattleParamConvertor
	{
		public BattleMonsterParam ConvertTournamentParamToBattleParam(int idx, Tournament.TournamentMonsterParam tParam)
		{
			var bParam = new BattleMonsterParam
				(
					idx,
					tParam.MonsterName,
					tParam.Hp,
					tParam.Attack,
					tParam.Guts,
					tParam.Diffence,
					tParam.Speed,
					tParam.Luck
				);
			return bParam;
		}
	}
}
