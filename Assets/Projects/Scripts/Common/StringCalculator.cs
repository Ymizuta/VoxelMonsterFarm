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
					return "�̗�";
				case ParamType.Power:
					return "������";
				case ParamType.Int:
					return "��������";
				case ParamType.Speed:
					return "�͂₳";
				case ParamType.Consentration:
					return "�W����";
				default:
					Debug.LogWarning("�z�肵�Ă��Ȃ��^�C�v���I������܂��� type = " + type);
					return "�s��";
			}
		}
	}
}
