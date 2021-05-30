using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Tournament
{
	public class TournamentMonsterParam
	{
		public TournamentMonsterParam(
			string name,
			int hp,
			int attack,
			int guts,
			int deffence,
			int speed,
			int luck,
			int monsterModelId)
		{
			this.MonsterName = name;
			this.Hp = hp;
			this.Attack = attack;
			this.Guts = guts;
			this.Diffence = deffence;
			this.Speed = speed;
			this.Luck = luck;
			this.MonsterModelId = monsterModelId;
		}
		public string MonsterName { get; private set; }
		public int Hp { get; private set; }
		public int Attack { get; private set; } // çUåÇóÕ
		public int Guts { get; private set; } // ÉKÉbÉc
		public int Diffence { get; private set; } // ñhå‰óÕ
		public int Speed { get; private set; } // ë¨ìx
		public int Luck { get; private set; } // â^
		public int MonsterModelId { get; private set; }
	}
}
