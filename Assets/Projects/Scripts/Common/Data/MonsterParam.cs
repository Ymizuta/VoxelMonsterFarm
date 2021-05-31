using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel
{
	public enum TournamentGrade
	{
		None = -1,
		A = 0,
		B,
		C,
	}

	[System.Serializable]
	public class MonsterParam
	{
		[SerializeField] private float fatigue;
		[SerializeField] private int monsterId;
		[SerializeField] private string monsterName;
		[SerializeField] private int livingWeek;
		[SerializeField] private int winCount;
		[SerializeField] private int monsterModelId;
		[SerializeField] private int hp;
		[SerializeField] private int power;
		[SerializeField] private int guts;
		[SerializeField] private int hit;
		[SerializeField] private int speed;
		[SerializeField] private int deffence;

		public int MonseterId { get { return monsterId; } set { monsterId = value; } }
		public string MonsterName { get { return monsterName; } set { monsterName = value; } }
		public int LivingWeek { get { return livingWeek; } set { livingWeek = value; } }
		public int BattleCount { get; set; }
		public int WinCount { get { return winCount; } set { winCount = value; } }
		public int MonsterModelId { get { return monsterModelId; } set { monsterModelId = value; } }
		// ��J�x
		public float Fatigue {
			get { return fatigue; }
			set { fatigue = Mathf.Clamp(value, 0, 100);} }
		// �p�����[�^
		public int Hp { get { return hp; } set { hp = value; } } // ���C�t
		public int Power { get { return power; } set { power = value; } } // ������
		public int Guts { get { return guts; } set { guts = value; } } // �K�b�c
		public int Hit { get { return hit; } set { hit = value; } } // ����
		public int Speed { get { return speed; } set { speed = value; } } // ���
		public int Deffence { get { return deffence; } set { deffence = value; } } // ��v��

		public static MonsterParam Default => Common.MonsterParamMasterManager.GetParam(1);
	}
}
