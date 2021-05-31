using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Farm
{
	public class MonsterParamWindow : MonoBehaviour
	{
		[SerializeField] private MonsterParamItem[] items;
		[SerializeField] private GameObject windowObject;
		[SerializeField] private Text monsterName;
		[SerializeField] private Text Age;
		[SerializeField] private Text Achievement;

		private bool isCreate;

		public void Initialize(MonsterParam param)
		{
			SetData(param);
		}

		public void Show()
		{
			windowObject.SetActive(true);
		}

		public void Show(MonsterParam param)
		{
			SetData(param);
			Show();
		}

		public void SetData(MonsterParam param)
		{
			this.monsterName.text = param.MonsterName;
			this.Age.text = $"{param.LivingWeek / 12}歳　{param.LivingWeek % 12}か月";
			this.Achievement.text = $"{param.BattleCount}戦　{param.WinCount}勝";
			// 能力値設定
			var calc = new StringCalculator();
			items[(int)ParamType.Hp].SetData(calc.GetParamName(ParamType.Hp), param.Hp);
			items[(int)ParamType.Power].SetData(calc.GetParamName(ParamType.Power), param.Power);
			items[(int)ParamType.Guts].SetData(calc.GetParamName(ParamType.Guts), param.Guts);
			items[(int)ParamType.Hit].SetData(calc.GetParamName(ParamType.Hit), param.Hit);
			items[(int)ParamType.Speed].SetData(calc.GetParamName(ParamType.Speed), param.Speed);
			items[(int)ParamType.Deffence].SetData(calc.GetParamName(ParamType.Deffence), param.Deffence);
		}

		public void Hide()
		{
			windowObject.SetActive(false);
		}
	}
}
