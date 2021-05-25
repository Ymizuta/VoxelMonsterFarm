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
		}

		private void Bind()
		{
			Model.Command
				.Subscribe(x =>OnChangeCommand(x)).AddTo(this);
		}

		/// <summary>
		/// 操作の紐づけ
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
		/// 牧場TOPメニューの操作
		/// </summary>
		private void SetEventsFarmTopMenu()
		{
			// 決定
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Subscribe(_ =>
				{
					OnDecideFarmTopMenu((FarmTopModel.FarmTopMenu)View.FarmTopMenu.CurrentIdx.Value);
				}).AddTo(setEventsDisposable);
			// カーソルを上に移動
			InputManager.Instance.OnUpKeyDownAsObservable
				.Subscribe(_ => View.FarmTopMenu.SelectUp()).AddTo(setEventsDisposable);
			// カーソルを下に移動
			InputManager.Instance.OnDownKeyDownAsObservable
				.Subscribe(_ => View.FarmTopMenu.SelectDown()).AddTo(setEventsDisposable);
		}

		/// <summary>
		/// 育成メニューの操作
		/// </summary>
		private void SetEventsTrainingMenu()
		{
			// メニュー選択
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Subscribe(_ =>
				{
					Model.OnDecideTrainingMenu(View.TrainingMenu.CurrentIdx.Value);
				}).AddTo(setEventsDisposable);
			// 戻る
			InputManager.Instance.OnBKeyDownAsObservable
				.Subscribe(_ =>
				{
					View.TrainingMenu.Hide();
					OnReturnFarmTopMenu();
				}).AddTo(setEventsDisposable);
			// カーソルを上に移動
			InputManager.Instance.OnUpKeyDownAsObservable
				.Subscribe(_ => View.TrainingMenu.SelectUp()).AddTo(setEventsDisposable);
			// カーソルを下に移動
			InputManager.Instance.OnDownKeyDownAsObservable
				.Subscribe(_ => View.TrainingMenu.SelectDown()).AddTo(setEventsDisposable);
			// カーソルを左に移動
			InputManager.Instance.OnLeftKeyDownAsObservable
				.Subscribe(_ => View.TrainingMenu.SelectLeft()).AddTo(setEventsDisposable);
			// カーソルを右に移動
			InputManager.Instance.OnRightKeyDownAsObservable
				.Subscribe(_ => View.TrainingMenu.SelectRight()).AddTo(setEventsDisposable);
		}

		/// <summary>
		/// モンスターパラム
		/// </summary>
		public void SetEventsMonsterParam()
		{
			// 戻る
			InputManager.Instance.OnBKeyDownAsObservable
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
					FarmCalendarManager.Instance.NextWeek();
					break;
				case FarmTopModel.FarmTopMenu.Training:
					// トレーニングに移動
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
					});
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
