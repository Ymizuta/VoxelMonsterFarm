using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Battle
{
	public class PlayerCommandProcess : MonoBehaviour
	{
		public IEnumerator StartProcess(BattleMonsterParam param, CommandParam commandParam)
		{
			yield return null;
			commandParam.command = SelectCommand(param);
		}

		private BattleCommand SelectCommand(BattleMonsterParam param)
		{
			var rate = Random.Range(0f, 100f);

			if (0 < param.Guts.Value)
			{
				if (50f <= rate)
				{
					return BattleCommand.Attack;
				}
				else if (20f <= rate)
				{
					return BattleCommand.Charge;
				}
				else
				{
					return BattleCommand.Guard;
				}
			}
			else
			{
				if (50f <= rate)
				{
					return BattleCommand.Charge;
				}
				else
				{
					return BattleCommand.Guard;
				}
			}
		}
	}
}
