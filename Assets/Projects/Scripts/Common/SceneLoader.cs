using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Voxel.SceneManagement
{
	public static class SceneLoader
	{
		public enum SceneName
		{
			Farm,
			Running,
			Tournament,
			Battle,
		}

		public static void ChangeScene(SceneName sceneName)
		{
			SceneManager.LoadScene(sceneName.ToString());
		}
	}
}
