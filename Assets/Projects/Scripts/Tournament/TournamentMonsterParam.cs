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
			int luck)
		{
			this.MonsterName = name;
			this.Hp = hp;
			this.Attack = attack;
			this.Guts = guts;
			this.Diffence = deffence;
			this.Speed = speed;
			this.Luck = luck;
		}

		public string MonsterName { get; private set; }
		public int Hp { get; private set; }
		public int Attack { get; private set; } // UŒ‚—Í
		public int Guts { get; private set; } // ƒKƒbƒc
		public int Diffence { get; private set; } // –hŒä—Í
		public int Speed { get; private set; } // ‘¬“x
		public int Luck { get; private set; } // ‰^
	}
}
