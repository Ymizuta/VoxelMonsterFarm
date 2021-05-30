using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Common
{
	public enum MonsterCondition
	{
		VeryFine, // �ƂĂ����C
		Fine, // ���C
		Tired, // ���Ă���
		VeryTired, // �ƂĂ����Ă���
	}

	public class MonsterCalculator
	{
		/// <summary>
		/// ��J�l�����Ԃ�Ԃ�
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
