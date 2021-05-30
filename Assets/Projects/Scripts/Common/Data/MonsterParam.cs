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
		// 疲労度
		public float Fatigue {
			get { return fatigue; }
			set { fatigue = Mathf.Clamp(value, 0, 100);} }
		// パラメータ
		public int Hp { get; set; } // ライフ
		public int Power { get; set; } // ちから
		public int Guts { get; set; } // ガッツ
		public int Hit { get; set; } // 命中
		public int Speed { get; set; } // 回避
		public int Deffence { get; set; } // 丈夫さ

		public static MonsterParam Default => Common.MonsterParamMasterManager.GetParam(1);
	}
}
