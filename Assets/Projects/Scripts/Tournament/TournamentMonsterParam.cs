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
			int hit,
			int monsterModelId)
		{
			this.MonsterName = name;
			this.Hp = hp;
			this.Attack = attack;
			this.Guts = guts;
			this.Diffence = deffence;
			this.Speed = speed;
			this.Hit = hit;
			this.MonsterModelId = monsterModelId;
		}
		public string MonsterName { get; private set; }
		public int Hp { get; private set; }
		public int Attack { get; private set; } // UŒ‚—Í
		public int Guts { get; private set; } // ƒKƒbƒc
		public int Diffence { get; private set; } // –hŒä—Í
		public int Speed { get; private set; } // ‘¬“x
		public int Hit { get; private set; } // –½’†
		public int MonsterModelId { get; private set; }
	}
}
