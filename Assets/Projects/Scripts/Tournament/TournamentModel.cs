using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Tournament
{
	public class TournamentModel : ModelBase
	{
		private TournamentMonsterParam[] monsterParams = new TournamentMonsterParam[] 
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

		public TournamentMonsterParam[] MonsterParams => monsterParams;

		public override void Initialize()
		{
			base.Initialize();
		}
	}
}
