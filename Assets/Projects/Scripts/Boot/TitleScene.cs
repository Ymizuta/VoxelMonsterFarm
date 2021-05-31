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
		// �X�^�[�g�E�R���e�B�j���[
		[SerializeField] private Text startButtonText;
		[SerializeField] private Text continueButtonText;
		// �����X�^�[�I��
		[SerializeField] private Transform monsterSelectUi;
		[SerializeField] private Text monsterName;
		[SerializeField] private Image monsterImg;
		[SerializeField] private Sprite[] monsterIcons;

		private ReactiveProperty<bool> IsStartButton = new ReactiveProperty<bool>(true);
		private Color selectedColor = Color.yellow;

		// �I���\�ȃ����X�^�[���
		private ReactiveProperty<int> SelectedMonsterIdx = new ReactiveProperty<int>();
		private int[] selectableMonsterIds = new int[] { 1,2,3,4,5,6};

		public override void Initialize(SceneData data = null)
		{
			base.Initialize(data);
			FadeManager.Instance.PlayFadeIn(() =>
			{
				startButtonText.color = selectedColor;
				SetEventsDefault();
			});
		}

		private void SetEventsDefault()
		{
			var disposable = new CompositeDisposable();

			// �I���{�^����ς���
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
			}).AddTo(disposable);
			// ����
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Subscribe(_ => 
				{
					if (IsStartButton.Value)
					{
						SetEventsMonsterSelects();
						disposable.Dispose();
					}
					else
					{
						// �R���e�B�j���[�{�^��
						FadeManager.Instance.PlayFadeOut(() =>
						{
							SaveDataManager.Load();
							GameCommonModel.Instance.SetCalendar();
							SceneLoader.Instance.ChangeScene(SceneLoader.SceneName.Farm);
							disposable.Dispose();
						});
					}
				}).AddTo(disposable);
			// �㉺
			InputManager.Instance.OnUpKeyDownAsObservable.Subscribe(_ => { IsStartButton.Value = !IsStartButton.Value; }).AddTo(disposable);
			InputManager.Instance.OnDownKeyDownAsObservable.Subscribe(_ => { IsStartButton.Value = !IsStartButton.Value; }).AddTo(disposable);
		}

		private void SetEventsMonsterSelects()
		{
			monsterSelectUi.gameObject.SetActive(true);
			var disposable = new CompositeDisposable();
			SelectedMonsterIdx
				.Subscribe(idx => 
				{
					var id = selectableMonsterIds[idx];
					var data =  Common.MonsterParamMasterManager.GetParam(id);
					monsterImg.sprite = monsterIcons[idx];
					monsterName.text = data.MonsterName;
				}).AddTo(disposable);
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Subscribe(_ => 
				{
					FadeManager.Instance.PlayFadeOut(() =>
					{
						// ������
						SaveDataManager.ResetSaveData(selectableMonsterIds[SelectedMonsterIdx.Value]);
						GameCommonModel.Instance.SetCalendar();
						SceneLoader.Instance.ChangeScene(SceneLoader.SceneName.Farm);
						disposable.Dispose();
					});
				}).AddTo(disposable);
			InputManager.Instance.OnRightKeyDownAsObservable
				.Subscribe(_ => 
				{
					var val = SelectedMonsterIdx.Value + 1;
					if (selectableMonsterIds.Length <= val) val = 0;
					SelectedMonsterIdx.Value = val;
				}).AddTo(disposable);
			InputManager.Instance.OnLeftKeyDownAsObservable
				.Subscribe(_ =>
				{
					var val = SelectedMonsterIdx.Value - 1;
					if (val < 0) val = selectableMonsterIds.Length - 1;
					SelectedMonsterIdx.Value = val;
				}).AddTo(disposable);
		}
	}
}
