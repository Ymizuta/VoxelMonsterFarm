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
					if (param.Guts.Value == 0 && (BattleCommand)menu.CurrentIdx.Value == BattleCommand.Attack)
					{
						Comment.Instance.SetComment("ガッツが無いので攻撃はできません！");
					}
					else
					{
						menu.Hide();
						commandParam.command = (BattleCommand)menu.CurrentIdx.Value;
						yield break;
					}
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

		/// <summary>
		/// AIの選択コマンドを返す
		/// </summary>
		/// <param name="param"></param>
		/// <param name="commandParam"></param>
		/// <returns></returns>
		public IEnumerator StartProcess(BattleMonsterParam param, BattleMonsterParam counterMonsterParam, CommandParam commandParam)
		{
			yield return null;
			commandParam.command = SelectCommand(param, counterMonsterParam);
		}

		/// <summary>
		/// AIのコマンド選択
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		private BattleCommand SelectCommand(BattleMonsterParam param, BattleMonsterParam counterMonsterParam)
		{
			var rate = Random.Range(0f, 100f);

			if (0 < param.Guts.Value)
			{
				if(param.Guts.Value == param.MaxGuts)
				{
					// ガッツが満タン
					if (IsCounterMonsterGutsZero())
					{
						// 相手のガッツが０
						return BattleCommand.Attack;
					}
					else
					{
						// 相手のガッツが０ではない
						if (50f <= rate)
						{
							return BattleCommand.Attack;
						}
						else
						{
							return BattleCommand.Guard;
						}
					}
				}
				else
				{
					if (IsCounterMonsterGutsZero())
					{
						// 相手のガッツが0
						if (50f <= rate)
						{
							return BattleCommand.Attack;
						}
						else
						{
							return BattleCommand.Charge;
						}
					}
					else
					{
						// 相手のガッツが0ではない
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
				}
			}
			else
			{
				// 自分のガッツが溜まっていない
				if (IsCounterMonsterGutsZero())
				{
					// 相手のガッツが溜まっていない
					return BattleCommand.Charge;
				}
				else
				{
					// 相手のガッツが溜まっている
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
			bool IsCounterMonsterGutsZero()
			{
				return counterMonsterParam.Guts.Value == 0;
			}
		}
	}
}
