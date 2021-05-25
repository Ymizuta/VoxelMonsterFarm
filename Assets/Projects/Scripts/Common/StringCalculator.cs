using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel
{
	public enum ParamType
	{
		Life = 0,
		Power,
		Int,
		Speed,
		Consentration,
	}

	public class StringCalculator
	{
		public string GetParamName(ParamType type)
		{
			switch (type)
			{
				case ParamType.Life:
					return "体力";
				case ParamType.Power:
					return "ちから";
				case ParamType.Int:
					return "かしこさ";
				case ParamType.Speed:
					return "はやさ";
				case ParamType.Consentration:
					return "集中力";
				default:
					Debug.LogWarning("想定していないタイプが選択されました type = " + type);
					return "不明";
			}
		}
	}
}
