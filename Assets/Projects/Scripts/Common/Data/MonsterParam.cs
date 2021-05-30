using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel
{
	[System.Serializable]
	public class MonsterParam
	{
		private float fatigue;

		public int MonseterId { get; set; }
		public string MonsterName { get; set; }
		public int LivingWeek { get; set; }
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

		public static MonsterParam Default => Common.MonsterParamMasterManager.GetParam(1);
	}
}
