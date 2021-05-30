using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Common
{
	public enum MonsterCondition
	{
		VeryFine, // ‚Æ‚Ä‚àŒ³‹C
		Fine, // Œ³‹C
		Tired, // ”æ‚ê‚Ä‚¢‚é
		VeryTired, // ‚Æ‚Ä‚à”æ‚ê‚Ä‚¢‚é
	}

	public class MonsterCalculator
	{
		/// <summary>
		/// ”æ˜J’l‚©‚çó‘Ô‚ğ•Ô‚·
		/// </summary>
		/// <param name="fatigue"></param>
		/// <returns></returns>
		public MonsterCondition CalcCondition(float fatigue)
		{
			if(60 < fatigue)
			{
				return MonsterCondition.VeryTired;
			}
			else if (40 < fatigue)
			{
				return MonsterCondition.Tired;
			}
			else if (20 < fatigue)
			{
				return MonsterCondition.Fine;
			}
			else
			{
				return MonsterCondition.VeryFine;
			}
		}
	}
}
