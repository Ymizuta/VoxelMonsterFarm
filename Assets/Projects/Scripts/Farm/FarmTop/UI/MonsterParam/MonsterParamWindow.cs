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
			this.Age.text = $"{param.LivingWeek / 12}çŒÅ@{param.LivingWeek % 12}Ç©åé";
			this.Achievement.text = $"{param.RaceCount}êÌÅ@{param.WinCount}èü";
			for (int i = 0; i < items.Length; i++)
			{
				items[i].SetData(param.MonsterParams[i]);
			}
		}

		public void Hide()
		{
			windowObject.SetActive(false);
		}
	}
}
