using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_TournamentMaster : ScriptableObject
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
		public string tournamentName;
		public int grade;
		public int month;
		public int week;
		public int monsterCount;
		public int money;
	}
}

