using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.SceneManagement;

namespace Voxel.Boot
{
	public class BootScene : SceneBase
	{
		[SerializeField] private GameObject[] dontDestroyObjects;

		public override void Initialize(SceneData data = null)
		{
			base.Initialize(data);
			SaveDataManager.Load();
			// ������
			GameCommonModel.Instance.SetCalendar();

			for (int i = 0; i < dontDestroyObjects.Length; i++)
			{
				DontDestroyOnLoad(dontDestroyObjects[i].gameObject);
			}
			Voxel.UI.FadeManager.Instance.PlayFadeOut(() => 
			{
				SceneLoader.Instance.ChangeScene(SceneLoader.SceneName.Farm);
			});
		}
	}
}
