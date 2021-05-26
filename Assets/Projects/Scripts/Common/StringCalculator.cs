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
					return "�̗�";
				case ParamType.Power:
					return "������";
				case ParamType.Guts:
					return "�K�b�c";
				case ParamType.Hit:
					return "����";
				case ParamType.Speed:
					return "�͂₳";
				case ParamType.Deffence:
					return "��v��";
				default:
					Debug.LogWarning("�z�肵�Ă��Ȃ��^�C�v���I������܂��� type = " + type);
					return "�s��";
			}
		}
	}
}
