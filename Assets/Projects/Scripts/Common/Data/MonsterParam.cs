using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel
{
	public class MonsterParam
	{
		public string MonsterName { get; private set; }
		public int LivingWeek { get; private set; }
		public int RaceCount { get; set; }
		public int WinCount { get; set; }
		public int[] MonsterParams { get; private set; }

		public MonsterParam()
		{
			this.MonsterName = "イッヌ";
			this.LivingWeek = 18;
			this.RaceCount = 10;
			this.WinCount = 8;
			// 体力、ちから、かしこさ、速度、集中力
			MonsterParams = new int[] 
			{
				30,
				25,
				28,
				45,
				40,
			};
		}
	}
}
