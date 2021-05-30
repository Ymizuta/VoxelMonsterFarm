using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel
{
	public enum MonsterModel
	{
		Dog = 0,
		Cat,
		Cow,
		Pig,
		Lion,
		Chicken,
	}

	[CreateAssetMenu(menuName = "ScriptableObjects/MonsterModelMaster", fileName = "MonsterModelMaster")]
	public class MonsterModelMaster : ScriptableObject
	{
		[SerializeField] private List<MonsterModelParam> param;
		public List<MonsterModelParam> Param => param;

		public GameObject GetMonsterObject(int id)
		{
			return param.Find(x => x.Id == id).Model;
		}
	}

	[Serializable]
	public class MonsterModelParam
	{
		public int Id;
		public GameObject Model;
	}
}
