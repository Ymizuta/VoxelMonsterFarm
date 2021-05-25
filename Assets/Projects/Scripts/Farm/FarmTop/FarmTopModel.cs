using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Voxel.SceneManagement;
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
			MonsterParam = new MonsterParam();
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

		/// <summary>
		/// トレーニングメニューを決定したときに呼ばれ、該当シーンへ遷移する
		/// </summary>
		/// <param name="selectIdx"></param>
		public void OnDecideTrainingMenu(int selectIdx)
		{
			var type = (TrainingMenu)selectIdx;
			var sceneName = SceneLoader.SceneName.Running;
			switch (type)
			{
				case TrainingMenu.Running:
					sceneName = SceneLoader.SceneName.Running;
					break;
				case TrainingMenu.Swimming:
					break;
				case TrainingMenu.ObstacleCourse:
					break;
				case TrainingMenu.Meditation:
					break;
				case TrainingMenu.DestroyObstacle:
					break;
				default:
					Debug.LogWarning("想定していないタイプが選択されました type =" + type);
					break;
			}
			FadeManager.Instance.PlayFadeOut(() => SceneLoader.ChangeScene(sceneName));
		}

		protected override void OnBack()
		{
			base.OnBack();
		}
	}
}
