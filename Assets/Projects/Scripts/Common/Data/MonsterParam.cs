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
			this.MonsterName = "�C�b�k";
			this.LivingWeek = 18;
			this.RaceCount = 10;
			this.WinCount = 8;
			// �̗́A������A���������A���x�A�W����
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
