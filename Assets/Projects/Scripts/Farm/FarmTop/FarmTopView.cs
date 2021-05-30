using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Farm
{
	public class FarmTopView : ViewBase
	{
		private TournamentSchedule schedule;
		private FarmTopMenu farmTopMenu;
		private TrainingMenu trainingMenu;
		private MonsterParamWindow monsterParamWindow;
		private bool isLoaded;

		public FarmTopMenu FarmTopMenu => farmTopMenu;
		public TrainingMenu TrainingMenu => trainingMenu;
		public MonsterParamWindow MonsterParamWindow => monsterParamWindow;
		public TournamentSchedule TournamentSchedule => schedule;

		public override IEnumerator Initialize()
		{
			yield return base.Initialize();
			Load(() => { isLoaded = true; });
			yield return new WaitUntil(() => isLoaded);
		}

		/// <summary>
		/// �J�ڑO
		/// </summary>
		public void OnBeforeMoveIn(string[] topMenuStrs, string[] trainingMenuStrs, MonsterParam param)
		{
			farmTopMenu.Initialize(topMenuStrs);
			farmTopMenu.Hide();
			trainingMenu.Initialize(trainingMenuStrs);
			trainingMenu.Hide();
			monsterParamWindow.Initialize(param);
			monsterParamWindow.Hide();
			schedule.Initialize();
			schedule.Hide();
		}

		private async void Load(Action action = null)
		{
			// �g�b�v���j���[
			var result = await AddressableManager.Instance.Load("Assets/Projects/AddressableAssets/Prefabs/UI/SelectMenu.prefab");			
			farmTopMenu = Instantiate(result, CanvasManager.Instance.BackCanvas.transform).GetComponent<FarmTopMenu>();
			// �琬���j���[
			result = await AddressableManager.Instance.Load("Assets/Projects/AddressableAssets/Prefabs/UI/TrainingMenu.prefab");
			trainingMenu = Instantiate(result, CanvasManager.Instance.BackCanvas.transform).GetComponent<TrainingMenu>();
			// �p����
			result = await AddressableManager.Instance.Load("Assets/Projects/AddressableAssets/Prefabs/UI/MonsterParamWindow.prefab");
			monsterParamWindow = Instantiate(result, CanvasManager.Instance.BackCanvas.transform).GetComponent<MonsterParamWindow>();
			// �\��\
			result = await AddressableManager.Instance.Load("Assets/Projects/AddressableAssets/Prefabs/UI/TournamentSchedule.prefab");
			schedule = Instantiate(result, CanvasManager.Instance.BackCanvas.transform).GetComponent<TournamentSchedule>();
			action?.Invoke();			
		}

		protected override void OnBack()
		{
			base.OnBack();
		}
	}
}
