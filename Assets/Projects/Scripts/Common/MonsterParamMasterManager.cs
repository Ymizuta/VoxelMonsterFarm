using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Common
{
	public static class MonsterParamMasterManager
	{
		public static MonsterParam GetParam(int monsterId)
		{
			var data = CommonMasterManager.Instance.MonsterParamMaster.sheets[0].list.Find(x => x.id == monsterId);
			return new MonsterParam()
			{
				MonseterId = data.id,
				MonsterModelId = data.modelId,
				MonsterName = data.monsterName,
				Hp = data.hp,
				Guts = data.guts,
				Power = data.power,
				Hit = data.hit,
				Speed = data.speed,
				Deffence = data.deffence,
			};
		}

		/// <summary>
		/// マスタからトーナメントパラムを取得
		/// </summary>
		/// <param name="monsterId"></param>
		/// <returns></returns>
		public static Tournament.TournamentMonsterParam GetTournamentParam(int monsterId)
		{
			var conv = new Tournament.MonsterTournamentParamConvertor();
			return conv.ConvertMonsterParamToTournamentParam(GetParam(monsterId));
		}
	}
}
