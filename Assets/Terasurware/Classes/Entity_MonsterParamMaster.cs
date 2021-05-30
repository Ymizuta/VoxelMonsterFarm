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
		[Header("�푰")]
		public int monsterType;
		[Header("���O")]
		public string monsterName;
		[Header("���f��ID")]
		public int modelId;
		[Header("HP")]
		public int hp;
		[Header("������")]
		public int power;
		[Header("�K�b�c")]
		public int guts;
		[Header("����")]
		public int hit;
		[Header("�͂₳")]
		public int speed;
		[Header("��v��")]
		public int deffence;
	}
}

