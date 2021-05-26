using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Voxel.UI;

namespace Voxel.Farm
{
	public class FarmTopModel : ModelBase
	{
		/// <summary>
		/// 操作状態
		/// </summary>
		public enum CommandType
		{
			None,
			FarmTopMenu,
			TrainingMenu,
			MonsterParam,
		}

		public enum FarmTopMenu
		{
			Training = 0, // 育成
			TakeRest, // 休養
			Tournament, // 大会
			Params,
		}

		public enum TrainingMenu
		{
			Running = 0, // 走り込み
			ObstacleCourse, // 障害物コース
			Swimming, // 水泳
			Meditation, // 瞑想
			DestroyObstacle, // 障害物破壊
		}

		public ReactiveProperty<CommandType> Command = new ReactiveProperty<CommandType>();

		public readonly string[] FarmTopMenuStrs = new string[] { "育成", "休養", "大会", "能力値"};
		public readonly string[] TrainingMenuStrs = new string[] { "走り込み", "障害物", "水泳", "瞑想", "打撃訓練"};

		public MonsterParam MonsterParam { get; private set; }

		public override void Initialize()
		{
			base.Initialize();
			MonsterParam = SaveDataManager.SaveData.CurrentMonster;
		}

		public string GetInitComment()
		{
			var comments = new string[] 
			{
				"イッヌ は元気だよ！",
				"イッヌ はすっごく元気だよ！",
				"イッヌ は少し疲れてるみたい…",
			};
			return comments[Random.Range(0,3)];
		}

		protected override void OnBack()
		{
			base.OnBack();
		}
	}
}
