using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Voxel.Battle
{
	public class BattleMonsterStatusUI : MonoBehaviour
	{
		[SerializeField] private Text monsterNameText;
		[SerializeField] private Text monsterHpText;
		[SerializeField] private Text monsterGutsText;

		public void SetData(BattleMonsterParam param)
		{
			this.monsterNameText.text = param.MonsterName;
			this.monsterHpText.text = param.HP.Value.ToString();
			this.monsterHpText.text = param.Guts.Value.ToString();
			param.HP
				.Subscribe(hp => 
				{
					monsterHpText.text = $"HP : {hp}";
				}).AddTo(this);
			param.Guts
				.Subscribe(guts =>
				{
					monsterGutsText.text = $"ƒKƒbƒc : {guts}";
				}).AddTo(this);
		}
	}
}
