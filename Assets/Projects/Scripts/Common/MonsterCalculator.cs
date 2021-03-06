using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Common
{
	public enum MonsterCondition
	{
		VeryFine, // とても元気
		Fine, // 元気
		Tired, // 疲れている
		VeryTired, // とても疲れている
	}

	public class MonsterCalculator
	{
		/// <summary>
		/// 疲労値から状態を返す
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
