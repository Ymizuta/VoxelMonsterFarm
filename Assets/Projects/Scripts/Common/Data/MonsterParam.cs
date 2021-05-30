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

		public static MonsterParam Default => new MonsterParam
		{
			MonsterModelId = (int)MonsterModel.Dog,
			MonseterId = 1,
			MonsterName = "イッヌ",
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
