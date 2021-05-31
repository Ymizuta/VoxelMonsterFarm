using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Voxel.SceneManagement;
using Voxel.UI;

namespace Voxel.Title
{
	public class TitleScene : SceneBase
	{
		[SerializeField] private Text startButtonText;
		[SerializeField] private Text continueButtonText;

		private ReactiveProperty<bool> IsStartButton = new ReactiveProperty<bool>(true);
		private Color selectedColor = Color.yellow;

		public override void Initialize(SceneData data = null)
		{
			base.Initialize(data);
			FadeManager.Instance.PlayFadeIn(() =>
			{
				startButtonText.color = selectedColor;
				Bind();
			});
		}

		private void Bind()
		{
			// 選択ボタンを変える
			IsStartButton.Subscribe(x =>
			{
				if (x)
				{
					startButtonText.color = selectedColor;
					continueButtonText.color = Color.black;
				}
				else
				{
					startButtonText.color = Color.black;
					continueButtonText.color = selectedColor;
				}
			}).AddTo(this);
			// 決定
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Subscribe(_ => 
				{
					if (IsStartButton.Value)
					{
						// スタートボタン
						FadeManager.Instance.PlayFadeOut(() =>
						{
							// 初期化
							SaveDataManager.ResetSaveData();
							GameCommonModel.Instance.SetCalendar();
							SceneLoader.Instance.ChangeScene(SceneLoader.SceneName.Farm);
						});
					}
					else
					{
						// コンティニューボタン
						FadeManager.Instance.PlayFadeOut(() =>
						{
							SaveDataManager.Load();
							GameCommonModel.Instance.SetCalendar();
							SceneLoader.Instance.ChangeScene(SceneLoader.SceneName.Farm);
						});
					}
				}).AddTo(this);
			// 上下
			InputManager.Instance.OnUpKeyDownAsObservable.Subscribe(_ => { IsStartButton.Value = !IsStartButton.Value; }).AddTo(this);
			InputManager.Instance.OnDownKeyDownAsObservable.Subscribe(_ => { IsStartButton.Value = !IsStartButton.Value; }).AddTo(this);
		}
	}
}
