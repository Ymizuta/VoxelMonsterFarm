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
					// �o�g��Scene����߂��Ă���
					Model.SetResult(tData.WinMonsterIdx, tData.LoseMonsterIdx);
					Comment.Instance.Show($"{TournamentCommonModel.Instance.MonsterParams[tData.WinMonsterIdx].MonsterName}�������I{TournamentCommonModel.Instance.MonsterParams[tData.LoseMonsterIdx].MonsterName}�s��܂����I");
					View.Board.UpdateBoard();
					// ���[�e�[�V�������X�V
					Model.IncrementMatchIdx();
					Observable.Timer(System.TimeSpan.FromSeconds(2f)).Subscribe(_ => 
					{
						StartCoroutine(TournamentProcess());
					});
				}
				else
				{
					// �g�[�i�����g�̊J�n
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
			Comment.Instance.Show($"������{GameCommonModel.Instance.Month}��{GameCommonModel.Instance.Month}�T��{TournamentCommonModel.Instance.TournamentName}���J�Â��܂��I");
			yield return new WaitForSeconds(2f);
			Comment.Instance.Show($"��ʃO���[�h�ւ̏��i��ڎw���Ċ撣���Ă��������I�I");
			yield return new WaitForSeconds(2f);
			yield return TournamentProcess();
			// ���I��
			OnFinishTournament();
		}

		/// <summary>
		/// �g�[�i�����g�̃��C�����[�v
		/// </summary>
		/// <returns></returns>
		private IEnumerator TournamentProcess()
		{
			// �S�����I���̃`�F�b�N
			if (Model.IsEndTournament())
			{
				// �D������
				Comment.Instance.Show($"{Model.GetTournamentWinner().MonsterName}�@�I�肪�D�����܂���!!");
				yield return new WaitForSeconds(2f);
				yield break;
			}

			// ����
			var winnerIdx = -1;
			var loserIdx = -1;
			if (Model.IsMyMonsterTurn())
			{
				// �v���C���[�̃����X�^�[�̎�����
				Model.IsAbstentionNextMatch = false;
				if (!Model.IsAbstentionTournament)
				{
					Comment.Instance.Show($"������{Model.CurrentMonsterParam.MonsterName}�I���{Model.CounterMonsterParam.MonsterName}�I��̎����ł�");
					// �v���C���[�̓��͑҂�
					yield return TopMenuProcess();
				}
				if (Model.IsAbstentionNextMatch || Model.IsAbstentionTournament)
				{
					// ����
					loserIdx = Model.CurrentMonsterIdx;
					winnerIdx = Model.CounterMonsterIdx;
				}
			}
			else
			{
				// NPC���m�̏��s�v�Z
				var result = CalcNPCMatch();
				winnerIdx = result.Item1;
				loserIdx = result.Item2;
			}

			// ���s���X�V�i�v���C���[������������ꍇ�̓V�[���ɖ߂��Ă������̖`���ōs���j
			Model.SetResult(winnerIdx, loserIdx);

			// ���[�e�[�V�������X�V
			Model.IncrementMatchIdx();

			// ���ʔ��f
			View.Board.UpdateBoard();
			Comment.Instance.Show($"{TournamentCommonModel.Instance.MonsterParams[winnerIdx].MonsterName}�������I{TournamentCommonModel.Instance.MonsterParams[loserIdx].MonsterName}�s��܂����I");
			//Debug.Log($"{TournamentCommonModel.Instance.MonsterParams[winnerIdx].MonsterName}�������I{TournamentCommonModel.Instance.MonsterParams[loserIdx].MonsterName}�s��܂����I");
			yield return new WaitForSeconds(1f);

			// �ċN����
			yield return TournamentProcess();
		}

		/// <summary>
		/// �v���C���[�̃����X�^�[�̎������ł̃��j���[�I��
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
						// ����
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
							}, () => { }, "�������J�n���܂����H");
							yield return new WaitUntil(() => Model.IsOperatable);
							break;
						// ����������
						case TournamentModel.TopMenuType.AbstentionNextMatch:
							bool isYes = false;
							YesNoPopup.Instance.Show(() => { isYes = true;}, ()=> { isYes = false;}, "���̎������������܂����H");
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
						// ��������
						case TournamentModel.TopMenuType.AbstentionTournament:
							isYes = false;
							YesNoPopup.Instance.Show(() => { isYes = true; }, () => { isYes = false; }, "�����������܂����H");
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
		/// ����������Ƃ��ɌĂ΂��
		/// </summary>
		private void OnFinishTournament()
		{
			// �T��i�߂Ėq��ɖ߂�
			// ���ʃ��f�����폜���Ă���
			Destroy(TournamentCommonModel.Instance.gameObject);
			CalendarManager.Instance.NextWeek();
			SceneLoader.Instance.ChangeScene(SceneLoader.SceneName.Farm);
		}

		/// <summary>
		/// NPC���m�̏��s���v�Z���Č��ʂ�Ԃ�
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
