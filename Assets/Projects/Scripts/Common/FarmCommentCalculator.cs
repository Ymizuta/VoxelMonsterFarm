using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Common
{
	public class FarmCommentCalculator
	{
		private const string NotExistComment = "対応するコメントがありません";

		/// <summary>
		/// 体調コメント
		/// </summary>
		/// <returns></returns>
		public string GetConditionComment(string monsterName, MonsterCondition condition)
		{
			switch (condition)
			{
				case MonsterCondition.VeryFine:
					return $"{monsterName}はとっても元気です！";
				case MonsterCondition.Fine:
					return $"{monsterName}は元気です";
				case MonsterCondition.Tired:
					return $"{monsterName}は少し疲れています";
				case MonsterCondition.VeryTired:
					return $"{monsterName}はかなり疲れています…";
				default:
					return NotExistComment;
			}
		}
	}
}
