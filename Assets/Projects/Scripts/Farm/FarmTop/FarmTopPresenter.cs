using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Voxel.UI;

namespace Voxel.Farm
{
	[RequireComponent(typeof(FarmTopModel))]
	[RequireComponent(typeof(FarmTopView))]
	public class FarmTopPresenter : PresenterBase
	{
		new FarmTopModel Model => base.Model as FarmTopModel;
		new FarmTopView View => base.View as FarmTopView;

		public override IEnumerator Initialize()
		{
			if (isInit) yield break;
			yield return base.Initialize();
			View.OnBeforeMoveIn(Model.FarmTopMenuStrs, Model.TrainingMenuStrs, Model.MonsterParam);
			isInit = true;
			FadeManager.Instance.PlayFadeIn(() => 
			{
				Bind();
				Model.Command.Value = FarmTopModel.CommandType.FarmTopMenu;
			});			
		}

		public override IEnumerator Run()
		{
			yield return base.Run();
			View.FarmTopMenu.Show();
			Comment.Instance.Show(Model.GetInitComment());
			if (FadeManager.Instance.IsFadeOut) FadeManager.Instance.PlayFadeIn();
			Model.Command.Value = FarmTopModel.CommandType.FarmTopMenu;
		}

		public override void OnBack()
		{
			base.OnBack();
			setEventsDisposable.Dispose();
		}

		private void Bind()
		{
			Model.Command
				.Subscribe(x =>OnChangeCommand(x)).AddTo(this);
		}

		/// <summary>
		/// ����̕R�Â�
		/// </summary>
		/// <param name="type"></param>
		private void OnChangeCommand(FarmTopModel.CommandType type)
		{
			if(setEventsDisposable != null) setEventsDisposable.Dispose();
			setEventsDisposable = new CompositeDisposable();
			switch (type)
			{
				case FarmTopModel.CommandType.None:
					break;
				case FarmTopModel.CommandType.FarmTopMenu:
					SetEventsFarmTopMenu();
					break;
				case FarmTopModel.CommandType.TrainingMenu:
					SetEventsTrainingMenu();
					break;
				case FarmTopModel.CommandType.MonsterParam:
					SetEventsMonsterParam();
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// �q��TOP���j���[�̑���
		/// </summary>
		private void SetEventsFarmTopMenu()
		{
			// ����
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Subscribe(_ =>
				{
					OnDecideFarmTopMenu((FarmTopModel.FarmTopMenu)View.FarmTopMenu.CurrentIdx.Value);
				}).AddTo(setEventsDisposable);
			// �J�[�\������Ɉړ�
			InputManager.Instance.OnUpKeyDownAsObservable
				.Subscribe(_ => View.FarmTopMenu.SelectUp()).AddTo(setEventsDisposable);
			// �J�[�\�������Ɉړ�
			InputManager.Instance.OnDownKeyDownAsObservable
				.Subscribe(_ => View.FarmTopMenu.SelectDown()).AddTo(setEventsDisposable);
		}

		/// <summary>
		/// �琬���j���[�̑���
		/// </summary>
		private void SetEventsTrainingMenu()
		{
			// ���j���[�I��
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Subscribe(_ =>
				{
					Model.OnDecideTrainingMenu(View.TrainingMenu.CurrentIdx.Value);
				}).AddTo(setEventsDisposable);
			// �߂�
			InputManager.Instance.OnBKeyDownAsObservable
				.Subscribe(_ =>
				{
					View.TrainingMenu.Hide();
					OnReturnFarmTopMenu();
				}).AddTo(setEventsDisposable);
			// �J�[�\������Ɉړ�
			InputManager.Instance.OnUpKeyDownAsObservable
				.Subscribe(_ => View.TrainingMenu.SelectUp()).AddTo(setEventsDisposable);
			// �J�[�\�������Ɉړ�
			InputManager.Instance.OnDownKeyDownAsObservable
				.Subscribe(_ => View.TrainingMenu.SelectDown()).AddTo(setEventsDisposable);
			// �J�[�\�������Ɉړ�
			InputManager.Instance.OnLeftKeyDownAsObservable
				.Subscribe(_ => View.TrainingMenu.SelectLeft()).AddTo(setEventsDisposable);
			// �J�[�\�����E�Ɉړ�
			InputManager.Instance.OnRightKeyDownAsObservable
				.Subscribe(_ => View.TrainingMenu.SelectRight()).AddTo(setEventsDisposable);
		}

		/// <summary>
		/// �����X�^�[�p����
		/// </summary>
		public void SetEventsMonsterParam()
		{
			// �߂�
			InputManager.Instance.OnBKeyDownAsObservable
				.Subscribe(_ =>
				{
					View.MonsterParamWindow.Hide();
					OnReturnFarmTopMenu();
				}).AddTo(setEventsDisposable);
		}

		/// <summary>
		/// �g�b�v���j���[�Ń��j���[��I�������Ƃ��ɌĂ΂��
		/// </summary>
		/// <param name="type"></param>
		private void OnDecideFarmTopMenu(FarmTopModel.FarmTopMenu type)
		{
			switch (type)
			{
				case FarmTopModel.FarmTopMenu.TakeRest:
					// �x�{�����
					FarmCalendarManager.Instance.NextWeek();
					break;
				case FarmTopModel.FarmTopMenu.Training:
					// �g���[�j���O�Ɉړ�
					Comment.Instance.Hide();
					View.FarmTopMenu.Hide();
					Model.Command.Value = FarmTopModel.CommandType.TrainingMenu;
					View.TrainingMenu.Show();
					break;
				case FarmTopModel.FarmTopMenu.Tournament:
					FadeManager.Instance.PlayFadeOut(() => 
					{
						Comment.Instance.Hide();
						Voxel.SceneManagement.SceneLoader.ChangeScene(SceneManagement.SceneLoader.SceneName.Tournament);
						OnBack();
					});
					break;
				case FarmTopModel.FarmTopMenu.Params:
					Comment.Instance.Hide();
					View.FarmTopMenu.Hide();
					Model.Command.Value = FarmTopModel.CommandType.MonsterParam;
					View.MonsterParamWindow.Show();
					break;
				default:
					Debug.LogWarning("�z�肵�Ă��Ȃ��^�C�v���I������܂��� type =" + type);
					break;
			}
		}

		/// <summary>
		/// �t�@�[���g�b�vUI�ɖ߂�
		/// </summary>
		private void OnReturnFarmTopMenu()
		{
			View.FarmTopMenu.Show();
			Model.Command.Value = FarmTopModel.CommandType.FarmTopMenu;
			Comment.Instance.Show();
		}
	}
}