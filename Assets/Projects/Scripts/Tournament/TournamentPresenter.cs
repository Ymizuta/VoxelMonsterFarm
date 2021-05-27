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

			// �J�ڑO����
			// �V�[���Ԃ�����肷�郂�f����p��
			if(TournamentCommonModel.Instance == null)
			{
				var commonModel = new GameObject("TournamentCommonModel").AddComponent<TournamentCommonModel>();
				commonModel.Initialize();
				DontDestroyOnLoad(commonModel.gameObject);
			}
			isInit = true;
		}

		public override IEnumerator Run(SceneData data = null)
		{
			yield return base.Run();
			View.OnBeforeMoveIn(Model.MonsterParams, Model.MenuStrs, TournamentCommonModel.Instance.Results);
			FadeManager.Instance.PlayFadeIn(() =>
			{
				if (data != null)
				{
					// �o�g��Scene����߂��Ă���
					var resultData = (TournamentSceneData)data;
					Model.SetResult(resultData.WinMonsterIdx, resultData.LoseMonsterIdx);
					Debug.Log($"{TournamentCommonModel.Instance.MonsterParams[resultData.WinMonsterIdx].MonsterName}�������I{TournamentCommonModel.Instance.MonsterParams[resultData.LoseMonsterIdx].MonsterName}�s��܂����I");
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
					StartCoroutine(TournamentProcess());
				}
			});
		}

		public override void OnBack()
		{
			base.OnBack();
			setEventsDisposable.Dispose();
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
				Debug.Log($"{Model.GetTournamentWinner().MonsterName}�@���D�����܂���!!");
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
			//Comment.Instance.Show($"{TournamentCommonModel.Instance.MonsterParams[winnerIdx].MonsterName}�������I{TournamentCommonModel.Instance.MonsterParams[loserIdx].MonsterName}�s��܂����I");
			Debug.Log($"{TournamentCommonModel.Instance.MonsterParams[winnerIdx].MonsterName}�������I{TournamentCommonModel.Instance.MonsterParams[loserIdx].MonsterName}�s��܂����I");
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
							OnBack();
							View.TopMenu.Hide();
							var data = new Battle.BattleSceneData(Model.CurrentMonsterIdx, Model.CounterMonsterIdx);
							SceneLoader.Instance.ChangeScene(SceneLoader.SceneName.Battle, data);
							break;
						// ����������
						case TournamentModel.TopMenuType.AbstentionNextMatch:
							Model.IsAbstentionNextMatch = true;
							View.TopMenu.Hide();
							yield break;
						// ��������
						case TournamentModel.TopMenuType.AbstentionTournament:
							Model.IsAbstentionTournament = true;
							View.TopMenu.Hide();
							yield break;
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
