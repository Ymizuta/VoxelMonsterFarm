using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel
{
	public enum ParamType
	{
		Hp = 0,
		Power,
		Guts,
		Hit,
		Speed,
		Deffence,
	}

	public class StringCalculator
	{
		public string GetParamName(ParamType type)
		{
			switch (type)
			{
				case ParamType.Hp:
					return "体力";
				case ParamType.Power:
					return "ちから";
				case ParamType.Guts:
					return "ガッツ";
				case ParamType.Hit:
					return "命中";
				case ParamType.Speed:
					return "はやさ";
				case ParamType.Deffence:
					return "丈夫さ";
				default:
					Debug.LogWarning("想定していないタイプが選択されました type = " + type);
					return "不明";
			}
		}
	}
}
