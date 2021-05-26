using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Voxel
{
	public static class SaveDataManager
	{
		private static readonly string DefaultSaveKey = "DefaultSaveKey";

		public static SaveData SaveData;

		public static void Save()
		{
			var jsonStr = JsonUtility.ToJson(SaveData);
			PlayerPrefs.SetString(DefaultSaveKey, jsonStr);
			PlayerPrefs.Save();
		}

		public static void Load()
		{
			var str = PlayerPrefs.GetString(DefaultSaveKey);
			if(str == "")
			{
				SaveData = SaveData.Default;
				// デフォルトのモンスターを用意
				SaveData.Monsters.Add(MonsterParam.Default);
				SaveData.CurrentMonsterId = 1;
			}
			else
			{
				SaveData = JsonUtility.FromJson<SaveData>(str);
			}
		}
	}
}
