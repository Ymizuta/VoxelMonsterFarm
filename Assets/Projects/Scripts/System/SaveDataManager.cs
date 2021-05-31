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
			SetData();
			var jsonStr = JsonUtility.ToJson(SaveData);
			PlayerPrefs.SetString(DefaultSaveKey, jsonStr);
			PlayerPrefs.Save();
		}

		private static void SetData()
		{
			SaveData.Year = GameCommonModel.Instance.Year.Value;
			SaveData.Month = GameCommonModel.Instance.Month.Value;
			SaveData.Week = GameCommonModel.Instance.Week.Value;
		}

		public static void Load()
		{
			var str = PlayerPrefs.GetString(DefaultSaveKey);
			if(str == "")
			{
				SaveData = SaveData.Default;
				// デフォルトのモンスターを用意
				SaveData.Wrapper.Monsters.Add(MonsterParam.Default);
				// todo 暫定で初期のグレードを設定
				SaveData.Wrapper.Monsters[0].Grade = TournamentGrade.C;
				SaveData.CurrentMonsterId = 1;
			}
			else
			{
				SaveData = JsonUtility.FromJson<SaveData>(str);
			}
		}
	}
}
