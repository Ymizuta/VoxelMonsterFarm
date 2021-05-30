using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Tournament
{
	public class MonsterTournamentParamConvertor
	{
		public TournamentMonsterParam ConvertMonsterParamToTournamentParam(MonsterParam param)
		{
			return new TournamentMonsterParam(param.MonsterName, param.Hp, param.Power, param.Guts, param.Deffence, param.Speed, param.Hit, param.MonsterModelId);
		}
	}
}
