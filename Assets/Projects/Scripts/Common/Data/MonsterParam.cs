using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel
{
	[System.Serializable]
	public class MonsterParam
	{
		private float fatigue;

		public int MonseterId;
		public string MonsterName { get; private set; }
		public int LivingWeek { get; private set; }
		public int RaceCount { get; set; }
		public int WinCount { get; set; }
		public int MonsterModelId { get; set; }
		// ��J�x
		public float Fatigue {
			get { return fatigue; }
			set { fatigue = Mathf.Clamp(value, 0, 100);} }
		// �p�����[�^
		public int Hp { get; set; } // ���C�t
		public int Power { get; set; } // ������
		public int Guts { get; set; } // �K�b�c
		public int Hit { get; set; } // ����
		public int Speed { get; set; } // ���
		public int Deffence { get; set; } // ��v��

		public static MonsterParam Default => new MonsterParam
		{
			MonsterModelId = (int)MonsterModel.Dog,
			MonseterId = 1,
			MonsterName = "�C�b�k",
			LivingWeek = 18,
			RaceCount = 0,
			WinCount = 0,
			Hp = 30,
			Power = 25,
			Guts = 25,
			Hit = 40,
			Speed = 35,
			Deffence = 25,
		};
	}
}
