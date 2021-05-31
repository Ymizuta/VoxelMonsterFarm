using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Voxel.UI;
using Voxel.SceneManagement;

namespace Voxel.Tournament
{
	[RequireComponent(typeof(TournamentModel))]
	[RequireComponent(typeof(TournamentView))]
	public class TournamentPresenter : PresenterBase
	{
		new TournamentModel Model => base.Model as TournamentModel;
		new TournamentView View => base.View as TournamentView;

		public override IEnumerator Initialize()
		{
			if (isInit) yield break;
			yield return base.Initialize();
			isInit = true;
		}

		public override IEnumerator Run(SceneData data = null)
		{
			yield return base.Run();
			View.OnBeforeMoveIn(Model.MonsterParams, Model.MenuStrs, TournamentCommonModel.Instance.Results);
			FadeManager.Instance.PlayFadeIn(() =>
			{
				var tData = (TournamentSceneData)data;
				if (!tData.IsInitialize)
				{
					// バトルSceneから戻ってきた
					Model.SetResult(tData.WinMonsterIdx, tData.LoseMonsterIdx);
					Comment.Instance.Show($"{TournamentCommonModel.Instance.MonsterParams[tData.WinMonsterIdx].MonsterName}が勝利！{TournamentCommonModel.Instance.MonsterParams[tData.LoseMonsterIdx].MonsterName}敗れました！");
					View.Board.UpdateBoard();
					// ローテーションを更新
					Model.IncrementMatchIdx();
					Observable.Timer(System.TimeSpan.FromSeconds(2f)).Subscribe(_ => 
					{
						StartCoroutine(TournamentProcess());
					});
				}
				else
				{
					// トーナメントの開始
					StartCoroutine(StartTournamentCoroutine());
				}
			});
		}

		public override void OnBack()
		{
			base.OnBack();
			setEventsDisposable.Dispose();
			Comment.Instance.Hide();
		}

		private IEnumerator StartTournamentCoroutine()
		{
			yield return null;
			yield return new WaitForSeconds(2f);
			Comment.Instance.Show($"これより{GameCommonModel.Instance.Month}月{GameCommonModel.Instance.Month}週の{TournamentCommonModel.Instance.TournamentName}を開催します！");
			yield return new WaitForSeconds(2f);
			Comment.Instance.Show($"上位グレードへの昇格を目指して頑張ってください！！");
			yield return new WaitForSeconds(2f);
			yield return TournamentProcess();
			// 大会終了
			OnFinishTournament();
		}

		/// <summary>
		/// トーナメントのメインループ
		/// </summary>
		/// <returns></returns>
		private IEnumerator TournamentProcess()
		{
			// 全試合終了のチェック
			if (Model.IsEndTournament())
			{
				// 優勝判定
				Comment.Instance.Show($"{Model.GetTournamentWinner().MonsterName}　選手が優勝しました!!");
				yield return new WaitForSeconds(2f);
				yield break;
			}

			// 試合
			var winnerIdx = -1;
			var loserIdx = -1;
			if (Model.IsMyMonsterTurn())
			{
				// プレイヤーのモンスターの試合順
				Model.IsAbstentionNextMatch = false;
				if (!Model.IsAbstentionTournament)
				{
					Comment.Instance.Show($"続いて{Model.CurrentMonsterParam.MonsterName}選手と{Model.CounterMonsterParam.MonsterName}選手の試合です");
					// プレイヤーの入力待ち
					yield return TopMenuProcess();
				}
				if (Model.IsAbstentionNextMatch || Model.IsAbstentionTournament)
				{
					// 棄権
					loserIdx = Model.CurrentMonsterIdx;
					winnerIdx = Model.CounterMonsterIdx;
				}
			}
			else
			{
				// NPC同士の勝敗計算
				var result = CalcNPCMatch();
				winnerIdx = result.Item1;
				loserIdx = result.Item2;
			}

			// 勝敗を更新（プレイヤーが試合をする場合はシーンに戻ってきた時の冒頭で行う）
			Model.SetResult(winnerIdx, loserIdx);

			// ローテーションを更新
			Model.IncrementMatchIdx();

			// 結果反映
			View.Board.UpdateBoard();
			Comment.Instance.Show($"{TournamentCommonModel.Instance.MonsterParams[winnerIdx].MonsterName}が勝利！{TournamentCommonModel.Instance.MonsterParams[loserIdx].MonsterName}敗れました！");
			//Debug.Log($"{TournamentCommonModel.Instance.MonsterParams[winnerIdx].MonsterName}が勝利！{TournamentCommonModel.Instance.MonsterParams[loserIdx].MonsterName}敗れました！");
			yield return new WaitForSeconds(1f);

			// 再起処理
			yield return TournamentProcess();
		}

		/// <summary>
		/// プレイヤーのモンスターの試合順でのメニュー選択
		/// </summary>
		/// <returns></returns>
		private IEnumerator TopMenuProcess()
		{
			View.TopMenu.Show();
			while (true)
			{
				yield return null;
				if (Input.GetKeyDown(KeyCode.Space))
				{
					switch ((TournamentModel.TopMenuType)View.TopMenu.CurrentIdx.Value)
					{
						// 試合
						case TournamentModel.TopMenuType.Match:
							YesNoPopup.Instance.Show(() => 
							{
								FadeManager.Instance.PlayFadeOut(() => 
								{
									OnBack();
									View.TopMenu.Hide();
									var data = new Battle.BattleSceneData(Model.CurrentMonsterIdx, Model.CounterMonsterIdx);
									SceneLoader.Instance.ChangeScene(SceneLoader.SceneName.Battle, data);
								});
							}, () => { }, "試合を開始しますか？");
							yield return new WaitUntil(() => Model.IsOperatable);
							break;
						// 試合を棄権
						case TournamentModel.TopMenuType.AbstentionNextMatch:
							bool isYes = false;
							YesNoPopup.Instance.Show(() => { isYes = true;}, ()=> { isYes = false;}, "次の試合を棄権しますか？");
							yield return new WaitUntil(() => Model.IsOperatable);
							if (isYes)
							{
								Model.IsAbstentionNextMatch = true;
								View.TopMenu.Hide();
								yield break;
							}
							else
							{
								break;
							}
						// 大会を棄権
						case TournamentModel.TopMenuType.AbstentionTournament:
							isYes = false;
							YesNoPopup.Instance.Show(() => { isYes = true; }, () => { isYes = false; }, "大会を棄権しますか？");
							yield return new WaitUntil(() => Model.IsOperatable);
							if (isYes)
							{
								Model.IsAbstentionTournament = true;
								View.TopMenu.Hide();
								yield break;
							}
							else
							{
								break;
							}
						default:
							break;
					}
				}
				else if (Input.GetKeyDown(KeyCode.B))
				{
				}
				else if (Input.GetKeyDown(KeyCode.UpArrow))
				{
					View.TopMenu.SelectUp();
				}
				else if (Input.GetKeyDown(KeyCode.DownArrow))
				{
					View.TopMenu.SelectDown();
				}
			}
		}

		/// <summary>
		/// 大会が完了するときに呼ばれる
		/// </summary>
		private void OnFinishTournament()
		{
			// 週を進めて牧場に戻る
			// 共通モデルを削除しておく
			Destroy(TournamentCommonModel.Instance.gameObject);
			CalendarManager.Instance.NextWeek();
			SceneLoader.Instance.ChangeScene(SceneLoader.SceneName.Farm);
		}

		/// <summary>
		/// NPC同士の勝敗を計算して結果を返す
		/// </summary>
		/// <param name="monsterIdx"></param>
		/// <returns></returns>
		private (int, int) CalcNPCMatch()
		{
			var winnerIdx = Model.CurrentMonsterIdx;
			var loserIdx = Model.CounterMonsterIdx;
			return (winnerIdx, loserIdx);
		}
	}
}
