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
		[Header("ID")]
		public int id;
		[Header("種族")]
		public int monsterType;
		[Header("名前")]
		public string monsterName;
		[Header("モデルID")]
		public int modelId;
		[Header("HP")]
		public int hp;
		[Header("ちから")]
		public int power;
		[Header("ガッツ")]
		public int guts;
		[Header("命中")]
		public int hit;
		[Header("はやさ")]
		public int speed;
		[Header("丈夫さ")]
		public int deffence;
	}
}

