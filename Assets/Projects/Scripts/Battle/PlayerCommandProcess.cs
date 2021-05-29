using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.UI;

namespace Voxel.Battle
{
	public class PlayerCommandProcess : MonoBehaviour
	{
		BattleCommandMenu menu;

		public void Initialize(BattleCommandMenu menu)
		{
			this.menu = menu;
		}

		public IEnumerator PlayerProcess(BattleMonsterParam param, CommandParam commandParam)
		{
			Comment.Instance.Show("どうする?");
			menu.Show();
			while (true)
			{
				yield return null;
				if (Input.GetKeyDown(KeyCode.Space))
				{
					menu.Hide();
					commandParam.command = (BattleCommand)menu.CurrentIdx.Value;
					yield break;
				}
				else if (Input.GetKeyDown(KeyCode.UpArrow))
				{
					menu.SelectUp();
				}
				else if (Input.GetKeyDown(KeyCode.DownArrow))
				{
					menu.SelectDown();
				}
				else if (Input.GetKeyDown(KeyCode.LeftArrow))
				{
					menu.SelectLeft();
				}
				else if (Input.GetKeyDown(KeyCode.RightArrow))
				{
					menu.SelectRight();
				}
			}
		}

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
