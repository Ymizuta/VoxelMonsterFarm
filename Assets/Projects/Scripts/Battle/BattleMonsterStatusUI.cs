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
		[SerializeField] private Slider hpSlider;
		[SerializeField] private Text monsterGutsText;
		[SerializeField] private Slider gutsSlider;

		public void SetData(BattleMonsterParam param)
		{
			this.monsterNameText.text = param.MonsterName;
			// HP������
			this.monsterHpText.text = $"HP:{param.HP.Value}";
			this.hpSlider.maxValue = param.HP.Value;
			this.hpSlider.value = param.HP.Value;
			// �K�b�c������
			this.monsterGutsText.text = $"Guts:{param.Guts.Value}";
			this.gutsSlider.maxValue = 3; // todo ��ŏC��
			this.gutsSlider.value = param.Guts.Value; // todo ��ŏC��

			param.HP
				.Subscribe(hp => 
				{
					monsterHpText.text = $"HP:{hp}";
					hpSlider.value = hp;
				}).AddTo(this);
			param.Guts
				.Subscribe(guts =>
				{
					monsterGutsText.text = $"Guts:{guts}";
					gutsSlider.value = guts;
				}).AddTo(this);
		}
	}
}
