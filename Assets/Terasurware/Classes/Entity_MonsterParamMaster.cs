using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_MonsterParamMaster : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{		
		public int id;
		public int monsterType;
		public string monsterName;
		public int modelId;
		public int grade;
		public int hp;
		public int power;
		public int guts;
		public int hit;
		public int speed;
		public int deffence;
	}
}

