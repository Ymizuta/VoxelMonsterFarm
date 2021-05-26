using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.SceneManagement;

namespace Voxel.Boot
{
	public class BootScene : MonoBehaviour
	{
		[SerializeField] private GameObject[] dontDestroyObjects;

		private void Awake()
		{
			SaveDataManager.Load();
			for (int i = 0; i < dontDestroyObjects.Length; i++)
			{
				DontDestroyOnLoad(dontDestroyObjects[i].gameObject);
			}
			SceneLoader.ChangeScene(SceneLoader.SceneName.Farm);
		}
	}
}
