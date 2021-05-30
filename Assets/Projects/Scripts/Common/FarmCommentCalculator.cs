using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Common
{
	public class FarmCommentCalculator
	{
		private const string NotExistComment = "�Ή�����R�����g������܂���";

		/// <summary>
		/// �̒��R�����g
		/// </summary>
		/// <returns></returns>
		public string GetConditionComment(string monsterName, MonsterCondition condition)
		{
			switch (condition)
			{
				case MonsterCondition.VeryFine:
					return $"{monsterName}�͂Ƃ��Ă����C�ł��I";
				case MonsterCondition.Fine:
					return $"{monsterName}�͌��C�ł�";
				case MonsterCondition.Tired:
					return $"{monsterName}�͏������Ă��܂�";
				case MonsterCondition.VeryTired:
					return $"{monsterName}�͂��Ȃ���Ă��܂��c";
				default:
					return NotExistComment;
			}
		}
	}
}
