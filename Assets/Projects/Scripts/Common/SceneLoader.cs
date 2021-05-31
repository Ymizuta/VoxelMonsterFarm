using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Voxel.SceneManagement
{
	public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
	{
		[SerializeField] Boot.BootScene bootScene;

		public enum SceneName
		{
			Title,
			Farm,
			Running,
			Tournament,
			Battle,
			Training,
		}

		protected override void Awake()
		{
			base.Awake();
			bootScene.Initialize();
		}

		/// <summary>
		/// �����Ŏw�肵��Scene�����[�h����
		/// </summary>
		/// <param name="sceneName"></param>
		/// <param name="data">�V�[���Ԃł��Ƃ肷��f�[�^</param>
		public void ChangeScene(SceneName sceneName, SceneData data = null)
		{
			StartCoroutine(ChangeSceneAsync(sceneName, data));
		}

		/// <summary>
		/// �񓯊�Scene���[�h
		/// </summary>
		/// <param name="sceneName"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		private IEnumerator ChangeSceneAsync(SceneName sceneName, SceneData data = null)
		{
			var asyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString());
			while (!asyncOperation.isDone)
			{
				yield return null;
			}
			foreach (var item in SceneManager.GetActiveScene().GetRootGameObjects())
			{
				var scene = item.GetComponent<SceneBase>();
				if (scene != null) scene.Initialize(data);
			}
		}
	}
}
