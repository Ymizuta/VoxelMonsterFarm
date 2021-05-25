using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Farm
{
	public class MonsterParamItem : MonoBehaviour
	{
		[SerializeField] private Text valueText;
		[SerializeField] private Slider slider;

		public void SetData(int val)
		{
			valueText.text = val.ToString();
			slider.value = val;
		}
	}
}
