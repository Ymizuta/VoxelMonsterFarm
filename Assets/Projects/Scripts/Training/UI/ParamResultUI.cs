using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Training
{
	public class ParamResultUI : MonoBehaviour
	{
		[SerializeField] private Text resultText;
		[SerializeField] private ParamResultItem[] items;
		[SerializeField] private Transform paramWindow;

		/// <summary>
		/// 「成功」等を表示する
		/// </summary>
		public void ShowResult()
		{
			resultText.gameObject.SetActive(true);
		}

		public void HideResult()
		{
			resultText.gameObject.SetActive(false);
		}

		/// <summary>
		/// 成長パラメータ表示
		/// </summary>
		public void ShowParam(MonsterParam param, int[] addVal)
		{
			SetData(param, addVal);
			paramWindow.gameObject.SetActive(true);
		}

		public void HideParam()
		{
			paramWindow.gameObject.SetActive(false);
		}

		private void SetData(MonsterParam param, int[] addVal)
		{
			var vals = new int[] 
			{
				param.Hp,
				param.Power,
				param.Guts,
				param.Hit,
				param.Speed,
				param.Deffence,
			};
			for (int i = 0; i < addVal.Length; i++) items[i].gameObject.SetActive(false);
			for (int i = 0; i < addVal.Length; i++)
			{
				if (addVal[i] != 0)
				{
					items[i].gameObject.SetActive(true);
					items[i].SetData((ParamType)i, vals[i], addVal[i]);
				}
			}
		}
	}
}
