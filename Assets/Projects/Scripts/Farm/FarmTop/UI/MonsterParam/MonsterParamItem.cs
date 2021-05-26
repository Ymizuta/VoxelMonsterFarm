using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Farm
{
	public class MonsterParamItem : MonoBehaviour
	{
		[SerializeField] private Text typeText;
		[SerializeField] private Text valueText;
		[SerializeField] private Slider slider;

		public void SetData(string typeStr, int val)
		{
			typeText.text = typeStr;
			valueText.text = val.ToString();
			slider.value = val;
		}
	}
}
