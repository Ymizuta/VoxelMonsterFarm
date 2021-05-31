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

			Screen.SetResolution(1920/2, 1080/2, false, 60);

			for (int i = 0; i < dontDestroyObjects.Length; i++)
			{
				DontDestroyOnLoad(dontDestroyObjects[i].gameObject);
			}
			Voxel.UI.FadeManager.Instance.PlayFadeOut(() => 
			{
				SceneLoader.Instance.ChangeScene(SceneLoader.SceneName.Title);
			});
		}
	}
}
