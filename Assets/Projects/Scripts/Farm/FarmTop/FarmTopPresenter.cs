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
			// 予定表の更新
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
					// 月の更新
					Model.MonthDataUpdate();
					View.TournamentSchedule.UpdateShedule(Model.SelectedMonth.Value, Model.ScheduleDatas);
				}).AddTo(this);
		}

		/// <summary>
		/// 操作の紐づけ
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
		/// 牧場TOPメニューの操作
		/// </summary>
		private void SetEventsFarmTopMenu()
		{
			// 決定
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ =>
				{
					OnDecideFarmTopMenu((FarmTopModel.FarmTopMenu)View.FarmTopMenu.CurrentIdx.Value);
				}).AddTo(setEventsDisposable);
			// カーソルを上に移動
			InputManager.Instance.OnUpKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => View.FarmTopMenu.SelectUp()).AddTo(setEventsDisposable);
			// カーソルを下に移動
			InputManager.Instance.OnDownKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => View.FarmTopMenu.SelectDown()).AddTo(setEventsDisposable);
		}

		/// <summary>
		/// 育成メニューの操作
		/// </summary>
		private void SetEventsTrainingMenu()
		{
			// メニュー選択
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ =>
				{
					OnDecideTrainingMenu(View.TrainingMenu.CurrentIdx.Value);
				}).AddTo(setEventsDisposable);
			// 戻る
			InputManager.Instance.OnBKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ =>
				{
					View.TrainingMenu.Hide();
					OnReturnFarmTopMenu();
				}).AddTo(setEventsDisposable);
			// カーソルを上に移動
			InputManager.Instance.OnUpKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => View.TrainingMenu.SelectUp()).AddTo(setEventsDisposable);
			// カーソルを下に移動
			InputManager.Instance.OnDownKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => View.TrainingMenu.SelectDown()).AddTo(setEventsDisposable);
			// カーソルを左に移動
			InputManager.Instance.OnLeftKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => View.TrainingMenu.SelectLeft()).AddTo(setEventsDisposable);
			// カーソルを右に移動
			InputManager.Instance.OnRightKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => View.TrainingMenu.SelectRight()).AddTo(setEventsDisposable);
		}

		/// <summary>
		/// 予定表の操作
		/// </summary>
		private void SetEventsTournamentSchedule()
		{
			// 大会選択
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
						}, () => { }, "この大会に参加しますか？");
					}
					else
					{
						ConfirmPopup.Instance.Show(() => {}, "この日程は選択できません");
					}
				}).AddTo(setEventsDisposable);
			// 戻る
			InputManager.Instance.OnBKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ =>
				{
					View.TournamentSchedule.Hide();
					OnReturnFarmTopMenu();
				}).AddTo(setEventsDisposable);
			// カーソルを上に移動
			InputManager.Instance.OnUpKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => Model.SelectUpTournamentSchedule()).AddTo(setEventsDisposable);
			// カーソルを下に移動
			InputManager.Instance.OnDownKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => Model.SelectDownTournamentSchedule()).AddTo(setEventsDisposable);
			// カーソルを左に移動
			InputManager.Instance.OnLeftKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => Model.SelectLeftTournamentSchedule()).AddTo(setEventsDisposable);
			// カーソルを右に移動
			InputManager.Instance.OnRightKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ => Model.SelectRightTournamentSchedule()).AddTo(setEventsDisposable);
		}

		/// <summary>
		/// モンスターパラム
		/// </summary>
		private void SetEventsMonsterParam()
		{
			// 戻る
			InputManager.Instance.OnBKeyDownAsObservable
				.Where(_ => Model.IsOperatable)
				.Subscribe(_ =>
				{
					View.MonsterParamWindow.Hide();
					OnReturnFarmTopMenu();
				}).AddTo(setEventsDisposable);
		}

		/// <summary>
		/// トップメニューでメニューを選択したときに呼ばれる
		/// </summary>
		/// <param name="type"></param>
		private void OnDecideFarmTopMenu(FarmTopModel.FarmTopMenu type)
		{
			switch (type)
			{
				case FarmTopModel.FarmTopMenu.TakeRest:
					// 休養を取る
					YesNoPopup.Instance.Show(() => 
					{
						Model.MonsterParam.Fatigue -= GameSettingProvidor.Instance.TakeRestRemovedFatigue;
						FarmCalendarManager.Instance.NextWeek();
					}, () => { }, "今週は休養にしますか？");
					break;
				case FarmTopModel.FarmTopMenu.Training:
					// トレーニングに移動
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
					// 選択カーソル初期化
					View.TournamentSchedule.Select(Model.SelectedGrade.Value, Model.SelectedWeek.Value, Model.SelectedScheduleData);
					break;
				case FarmTopModel.FarmTopMenu.Params:
					Comment.Instance.Hide();
					View.FarmTopMenu.Hide();
					Model.Command.Value = FarmTopModel.CommandType.MonsterParam;
					View.MonsterParamWindow.Show();
					break;
				default:
					Debug.LogWarning("想定していないタイプが選択されました type =" + type);
					break;
			}
		}

		/// <summary>
		/// トレーニングメニューを決定したときに呼ばれ、該当シーンへ遷移する
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
					Debug.LogWarning("想定していないタイプが選択されました type =" + type);
					break;
			}
			YesNoPopup.Instance.Show(() =>
			{
				OnBack();
				FadeManager.Instance.PlayFadeOut(() => SceneLoader.Instance.ChangeScene(sceneName, new Training.TrainingSceneData(menu)));
			}, () => { }, "トレーニングを始めますか？");
		}

		/// <summary>
		/// ファームトップUIに戻る
		/// </summary>
		private void OnReturnFarmTopMenu()
		{
			View.FarmTopMenu.Show();
			Model.Command.Value = FarmTopModel.CommandType.FarmTopMenu;
			Comment.Instance.Show();
		}
	}
}
