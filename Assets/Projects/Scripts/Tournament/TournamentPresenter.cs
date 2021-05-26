using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Voxel.UI;

namespace Voxel.Tournament
{
	[RequireComponent(typeof(TournamentModel))]
	[RequireComponent(typeof(TournamentView))]
	public class TournamentPresenter : PresenterBase
	{
		new TournamentModel Model => base.Model as TournamentModel;
		new TournamentView View => base.View as TournamentView;

		private void Awake()
		{
			StartCoroutine(Initialize());
		}

		public override IEnumerator Initialize()
		{
			if (isInit) yield break;
			yield return base.Initialize();

			// 遷移前処理
			// シーン間をやり取りするモデルを用意
			if(TournamentCommonModel.Instance == null)
			{
				var commonModel = new GameObject("TournamentCommonModel").AddComponent<TournamentCommonModel>();
				commonModel.Initialize();
				DontDestroyOnLoad(commonModel.gameObject);
			}
			View.OnBeforeMoveIn(Model.MonsterParams, Model.MenuStrs, TournamentCommonModel.Instance.Results);

			isInit = true;

			View.TopMenu.Show();
			FadeManager.Instance.PlayFadeIn(() =>
			{
				Bind();
				Model.Command.Value = TournamentModel.CommandType.TopMenu;
			});
		}

		public override IEnumerator Run()
		{
			yield return base.Run();			
		}

		public override void OnBack()
		{
			base.OnBack();
			setEventsDisposable.Dispose();
		}

		private void Bind()
		{
			Model.Command
				.Subscribe(x => OnChangeCommand(x)).AddTo(this);
		}

		private void OnChangeCommand(TournamentModel.CommandType type)
		{
			if (setEventsDisposable != null) setEventsDisposable.Dispose();
			setEventsDisposable = new CompositeDisposable();
			switch (type)
			{
				case TournamentModel.CommandType.TopMenu:
					SetEventsTopMenu();
					break;
				case TournamentModel.CommandType.TournamentBoard:
					SetEventsTournamentBoard();
					break;
				default:
					break;
			}
		}

		private void SetEventsTopMenu()
		{
			// メニュー選択
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Subscribe(_ =>
				{
					switch ((TournamentModel.TopMenuType)View.TopMenu.CurrentIdx.Value)
					{
						// 試合
						case TournamentModel.TopMenuType.Match:
							SceneManagement.SceneLoader.ChangeScene(SceneManagement.SceneLoader.SceneName.Battle);
							OnBack();
							break;
						// 試合を棄権
						case TournamentModel.TopMenuType.AbstentionNextMatch:
							break;
						// 大会を棄権
						case TournamentModel.TopMenuType.AbstentionTournament:
							break;
						default:
							break;
					}
				}).AddTo(setEventsDisposable);
			// トーナメント表へ
			InputManager.Instance.OnBKeyDownAsObservable
				.Subscribe(_ =>
				{
					View.TopMenu.Hide();
					Model.Command.Value = TournamentModel.CommandType.TournamentBoard;
				}).AddTo(setEventsDisposable);
			// カーソルを上に移動
			InputManager.Instance.OnUpKeyDownAsObservable
				.Subscribe(_ => View.TopMenu.SelectUp()).AddTo(setEventsDisposable);
			// カーソルを下に移動
			InputManager.Instance.OnDownKeyDownAsObservable
				.Subscribe(_ => View.TopMenu.SelectDown()).AddTo(setEventsDisposable);
		}

		private void SetEventsTournamentBoard()
		{
			// トップメニューを開く
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Subscribe(_ =>
				{
					Model.Command.Value = TournamentModel.CommandType.TopMenu;
					View.TopMenu.Show();
				}).AddTo(setEventsDisposable);
		}
	}
}
