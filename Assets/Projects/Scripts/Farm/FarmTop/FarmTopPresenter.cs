using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Voxel.UI;
using Voxel.SceneManagement;
using Voxel.Common;

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
			isInit = true;
		}

		public override IEnumerator Run(SceneManagement.SceneData data = null)
		{
			yield return base.Run();
			View.OnBeforeMoveIn(Model.FarmTopMenuStrs, Model.TrainingMenuStrs, Model.MonsterParam);
			View.FarmTopMenu.Show();
			Comment.Instance.Show(Model.GetInitComment());
			if (FadeManager.Instance.IsFadeOut)
			{
				FadeManager.Instance.PlayFadeIn(() =>
				{
					Bind();
					Model.Command.Value = FarmTopModel.CommandType.FarmTopMenu;
				});
			}
			else
			{
				Bind();
				Model.Command.Value = FarmTopModel.CommandType.FarmTopMenu;
			}
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
			// �\��\�̍X�V
			Model.SelectedGrade
				.Where(_ => Model.Command.Value == FarmTopModel.CommandType.Schedule)
				.Subscribe(_ => { View.TournamentSchedule.Select(Model.SelectedGrade.Value, Model.SelectedWeek.Value, Model.SelectedScheduleData); }).AddTo(this);
			Model.SelectedWeek
				.Where(_ => Model.Command.Value == FarmTopModel.CommandType.Schedule)
				.Subscribe(_ => { View.TournamentSchedule.Select(Model.SelectedGrade.Value, Model.SelectedWeek.Value, Model.SelectedScheduleData); }).AddTo(this);
			Model.SelectedMonth
				.Where(_ => Model.Command.Value == FarmTopModel.CommandType.Schedule)
				.Subscribe(_ => 
				{
					// ���̍X�V
					Model.MonthDataUpdate();
					View.TournamentSchedule.UpdateShedule(Model.SelectedMonth.Value, Model.ScheduleDatas);
				}).AddTo(this);
		}

		/// <summary>
		/// ����̕R�Â�
		/// </summary>
		/// <param name="type"></param>
		private void OnChangeCommand(FarmTopModel.CommandType type)
		{
			if (setEventsDisposable != null) setEventsDisposable.Dispose();
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
				case FarmTopModel.CommandType.Schedule:
					SetEventsTournamentSchedule();
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
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ =>
				{
					OnDecideFarmTopMenu((FarmTopModel.FarmTopMenu)View.FarmTopMenu.CurrentIdx.Value);
				}).AddTo(setEventsDisposable);
			// �J�[�\������Ɉړ�
			InputManager.Instance.OnUpKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => View.FarmTopMenu.SelectUp()).AddTo(setEventsDisposable);
			// �J�[�\�������Ɉړ�
			InputManager.Instance.OnDownKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => View.FarmTopMenu.SelectDown()).AddTo(setEventsDisposable);
		}

		/// <summary>
		/// �琬���j���[�̑���
		/// </summary>
		private void SetEventsTrainingMenu()
		{
			// ���j���[�I��
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ =>
				{
					OnDecideTrainingMenu(View.TrainingMenu.CurrentIdx.Value);
				}).AddTo(setEventsDisposable);
			// �߂�
			InputManager.Instance.OnBKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ =>
				{
					View.TrainingMenu.Hide();
					OnReturnFarmTopMenu();
				}).AddTo(setEventsDisposable);
			// �J�[�\������Ɉړ�
			InputManager.Instance.OnUpKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => View.TrainingMenu.SelectUp()).AddTo(setEventsDisposable);
			// �J�[�\�������Ɉړ�
			InputManager.Instance.OnDownKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => View.TrainingMenu.SelectDown()).AddTo(setEventsDisposable);
			// �J�[�\�������Ɉړ�
			InputManager.Instance.OnLeftKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => View.TrainingMenu.SelectLeft()).AddTo(setEventsDisposable);
			// �J�[�\�����E�Ɉړ�
			InputManager.Instance.OnRightKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => View.TrainingMenu.SelectRight()).AddTo(setEventsDisposable);
		}

		/// <summary>
		/// �\��\�̑���
		/// </summary>
		private void SetEventsTournamentSchedule()
		{
			// ���I��
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ =>
				{
					if (Model.CanEntryTournament())
					{
						YesNoPopup.Instance.Show(() =>
						{
							FadeManager.Instance.PlayFadeOut(() =>
							{
								Comment.Instance.Hide();
								var data = new Tournament.TournamentSceneData(Model.SelectedScheduleData.TournamentId);
								SceneLoader.Instance.ChangeScene(SceneLoader.SceneName.Tournament, data);
								OnBack();
							});
						}, () => { }, "���̑��ɎQ�����܂����H");
					}
					else
					{
						ConfirmPopup.Instance.Show(() => {}, "���̓����͑I���ł��܂���");
					}
				}).AddTo(setEventsDisposable);
			// �߂�
			InputManager.Instance.OnBKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ =>
				{
					View.TournamentSchedule.Hide();
					OnReturnFarmTopMenu();
				}).AddTo(setEventsDisposable);
			// �J�[�\������Ɉړ�
			InputManager.Instance.OnUpKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => Model.SelectUpTournamentSchedule()).AddTo(setEventsDisposable);
			// �J�[�\�������Ɉړ�
			InputManager.Instance.OnDownKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => Model.SelectDownTournamentSchedule()).AddTo(setEventsDisposable);
			// �J�[�\�������Ɉړ�
			InputManager.Instance.OnLeftKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => Model.SelectLeftTournamentSchedule()).AddTo(setEventsDisposable);
			// �J�[�\�����E�Ɉړ�
			InputManager.Instance.OnRightKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => Model.SelectRightTournamentSchedule()).AddTo(setEventsDisposable);
		}

		/// <summary>
		/// �����X�^�[�p����
		/// </summary>
		private void SetEventsMonsterParam()
		{
			// �߂�
			InputManager.Instance.OnBKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
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
					YesNoPopup.Instance.Show(() => 
					{
						Model.MonsterParam.Fatigue -= GameSettingProvidor.Instance.TakeRestRemovedFatigue;
						FarmCalendarManager.Instance.NextWeek();
					}, () => { }, "���T�͋x�{�ɂ��܂����H");
					break;
				case FarmTopModel.FarmTopMenu.Training:
					// �g���[�j���O�Ɉړ�
					Comment.Instance.Hide();
					View.FarmTopMenu.Hide();
					Model.Command.Value = FarmTopModel.CommandType.TrainingMenu;
					View.TrainingMenu.Show();
					break;
				case FarmTopModel.FarmTopMenu.Tournament:
					Comment.Instance.Hide();
					View.FarmTopMenu.Hide();
					Model.Command.Value = FarmTopModel.CommandType.Schedule;
					View.TournamentSchedule.Show(Model.SelectedMonth.Value, Model.ScheduleDatas);
					// �I���J�[�\��������
					View.TournamentSchedule.Select(Model.SelectedGrade.Value, Model.SelectedWeek.Value, Model.SelectedScheduleData);
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
		/// �g���[�j���O���j���[�����肵���Ƃ��ɌĂ΂�A�Y���V�[���֑J�ڂ���
		/// </summary>
		/// <param name="selectIdx"></param>
		public void OnDecideTrainingMenu(int selectIdx)
		{
			var type = (FarmTopModel.TrainingMenu)selectIdx;
			var sceneName = SceneLoader.SceneName.Training;
			var menu = Training.TrainingType.Running; 
			switch (type)
			{
				case FarmTopModel.TrainingMenu.Running:
					menu = Training.TrainingType.Running;
					break;
				case FarmTopModel.TrainingMenu.Shooting:
					menu = Training.TrainingType.Shooting;
					break;
				case FarmTopModel.TrainingMenu.Domino:
					menu = Training.TrainingType.Domino;
					break;
				case FarmTopModel.TrainingMenu.Studying:
					menu = Training.TrainingType.Studying;
					break;
				case FarmTopModel.TrainingMenu.AvoidRock:
					menu = Training.TrainingType.AvoidRock;
					break;
				case FarmTopModel.TrainingMenu.LogShock:
					menu = Training.TrainingType.LogShock;
					break;
				default:
					Debug.LogWarning("�z�肵�Ă��Ȃ��^�C�v���I������܂��� type =" + type);
					break;
			}
			YesNoPopup.Instance.Show(() =>
			{
				OnBack();
				FadeManager.Instance.PlayFadeOut(() => SceneLoader.Instance.ChangeScene(sceneName, new Training.TrainingSceneData(menu)));
			}, () => { }, "�g���[�j���O���n�߂܂����H");
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
