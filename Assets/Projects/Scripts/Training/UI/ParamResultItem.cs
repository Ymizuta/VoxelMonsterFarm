using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Training
{
	public class ParamResultItem : MonoBehaviour
	{
		[SerializeField] private Text paramName;
		[SerializeField] private Slider slider;
		[SerializeField] private Image sliderCol;
		[SerializeField] private Text addValText;
		[SerializeField] private Text beforeValText;
		[SerializeField] private Text afterValText;

		public void SetData(ParamType type, int beforeVal, int addVal)
		{
			var calc = new StringCalculator();
			paramName.text = calc.GetParamName(type);
			slider.value = beforeVal + addVal;
			addValText.text = addVal.ToString();
			beforeValText.text = beforeVal.ToString();
			afterValText.text = (beforeVal + addVal).ToString();
			sliderCol.color = GetParamColor(type);
		}

		private Color GetParamColor(ParamType type)
		{
			switch (type)
			{
				case ParamType.Hp:
					return Color.yellow;
				case ParamType.Power:
					return Color.red;
				case ParamType.Guts:
					return Color.green;
				case ParamType.Hit:
					return new Color(148.0f / 255.0f, 87.0f / 255.0f, 164.0f / 255.0f);
				case ParamType.Speed:
					return new Color(0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
				case ParamType.Deffence:
					return Color.blue;
				default:
					Debug.LogWarning("想定していないタイプが選択されました type = " + type);
					return Color.gray;
			}
		}
	}
}
