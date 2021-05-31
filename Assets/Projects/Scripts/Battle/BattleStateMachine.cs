using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.UI;
using UniRx;
using DG.Tweening;
using Voxel.Tournament;
using Voxel.Common;

namespace Voxel.Battle
{
	public class BattleStateMachine : MonoBehaviour
	{
		[SerializeField] BattleMonsterStatusUI[] statusUis;
		[SerializeField] BattleCommandMenu menu;
		[SerializeField] Camera mainCamera;
		[SerializeField] Camera subCamera;
		// モンスターのView
		[SerializeField] private GameObject myMonsterObject;
		[SerializeField] private GameObject enemyMonsterObject;
		[SerializeField] private Transform myMonsterRoot;
		[SerializeField] private Transform enemyMonsterRoot;
		// モンスターのパラム
		BattleMonsterParam currentMonster;
		BattleMonsterParam counterMonster;
		// プロセスを管理するクラスインスタンス
		PlayerCommandProcess commandProcess;
		ExcuteBattleProcess excuteBattleProcess;

		public void Initialize(SceneManagement.SceneData data)
		{
			var battleData = data as BattleSceneData;
			var converter = new TournamentBattleParamConvertor();
			currentMonster = converter.ConvertTournamentParamToBattleParam(battleData.CurrentMonsterIdx, TournamentCommonModel.Instance.GetMonsterParam(battleData.CurrentMonsterIdx));
			counterMonster = converter.ConvertTournamentParamToBattleParam(battleData.CounterMonsterIdx, TournamentCommonModel.Instance.GetMonsterParam(battleData.CounterMonsterIdx));
			CreateMonsterObject();

			// プロセス管理クラスの初期化
			commandProcess = this.gameObject.AddComponent<PlayerCommandProcess>();
			menu.Initialize(new string[] { "攻撃", "溜める", "防御" });
			commandProcess.Initialize(menu);
			excuteBattleProcess = this.gameObject.GetComponent<ExcuteBattleProcess>();
			excuteBattleProcess.Initialize(myMonsterObject, enemyMonsterObject);
			statusUis[0].SetData(currentMonster);
			statusUis[1].SetData(counterMonster);
			// メインループを開始
			FadeManager.Instance.PlayFadeIn(() => 
			{
				StartCoroutine(Process());
			});
		}

		/// <summary>
		/// パラメータからモンスターのオブジェクトを出し分け
		/// </summary>
		private void CreateMonsterObject()
		{
			var myMonsterObj = Instantiate(CommonMasterManager.Instance.MonsterModelMaster.GetMonsterObject(currentMonster.MonsterModelId), myMonsterRoot);
			myMonsterObj.gameObject.transform.Rotate(new Vector3(0f, 180f, 0f));
			var enemyMonsterObj = Instantiate(CommonMasterManager.Instance.MonsterModelMaster.GetMonsterObject(counterMonster.MonsterModelId), enemyMonsterRoot);
			enemyMonsterObj.gameObject.transform.Rotate(new Vector3(0f, 180f, 0f));
		}

		private IEnumerator Process()
		{
			yield return BeforeBattleProcess();
			yield return BattleProcess(currentMonster, counterMonster);
			yield return EndBattleProcess();
		}

		/// <summary>
		/// 試合開始前の演出等を行う
		/// </summary>
		private IEnumerator BeforeBattleProcess()
		{
			var isDone = false;
			mainCamera.gameObject.SetActive(false);
			subCamera.gameObject.SetActive(true);
			subCamera.transform.DOMove(subCamera.transform.position + new Vector3(0f, 0f, -4f), 2f)
				.OnComplete(() =>
				{
					isDone = true;
				});
			yield return new WaitUntil(() => isDone);
			yield return new WaitForSeconds(2f);
			Comment.Instance.Show("試合開始！");
			yield return new WaitForSeconds(1f);
			subCamera.gameObject.SetActive(false);
			mainCamera.gameObject.SetActive(true);
		}

		private IEnumerator BattleProcess(BattleMonsterParam myMonster, BattleMonsterParam enemyMonster)
		{
			// プレイヤーの操作→行動を決める
			var myCommandParam = new CommandParam();
			yield return commandProcess.PlayerProcess(myMonster, myCommandParam);

			// 敵の行動を決める
			var enemyCommandParam = new CommandParam();
			yield return commandProcess.StartProcess(enemyMonster, myMonster, enemyCommandParam);

			// 実行
			yield return excuteBattleProcess.StartProcess(new BattleMonsterParam[] { myMonster, enemyMonster} , new CommandParam[] { myCommandParam, enemyCommandParam});

			// 決着判定
			if (IsAnyDown())
			{
				yield break;
			}

			// プロセスの頭に戻る
			yield return BattleProcess(myMonster, enemyMonster);
		}

		private IEnumerator EndBattleProcess()
		{
			yield return new WaitForSeconds(1f);
			var winner = counterMonster.IsDown() ? currentMonster: counterMonster;
			var loser = currentMonster.IsDown() ? currentMonster : counterMonster;
			Comment.Instance.SetComment($"{loser.MonsterName}は倒れた！");
			yield return new WaitForSeconds(1f);
			FadeManager.Instance.PlayFadeOut(() => 
			{
				Comment.Instance.Hide();
				var data = new TournamentSceneData(winner.MonsterIdx, loser.MonsterIdx);
				SceneManagement.SceneLoader.Instance.ChangeScene(SceneManagement.SceneLoader.SceneName.Tournament, data);
			});
		}

		/// <summary>
		/// 決着しているか
		/// </summary>
		/// <returns></returns>
		private bool IsAnyDown()
		{
			return currentMonster.IsDown() || counterMonster.IsDown();
		}
	}

	public enum BattleCommand
	{
		Attack = 0, // 攻撃
		Charge, // 溜める
		Guard, // 防御
	}

	public class CommandParam
	{
		public BattleCommand command;
	}

	public class BattleMonsterParam
	{
		private string monsterName;
		private ReactiveProperty<int> hp = new ReactiveProperty<int>();
		private ReactiveProperty<int> attack = new ReactiveProperty<int>();
		private ReactiveProperty<int> guts = new ReactiveProperty<int>(); // ガッツ
		private ReactiveProperty<int> diffence = new ReactiveProperty<int>(); // 防御力
		private ReactiveProperty<int> speed = new ReactiveProperty<int>(); // 速度
		private ReactiveProperty<int> luck = new ReactiveProperty<int>(); // 運

		public BattleMonsterParam(int monsterIdx, string monsterName, int hp, int attack, int guts, int diffence, int speed, int luck, int modelId)
		{
			this.MonsterModelId = modelId;
			this.MonsterIdx = monsterIdx;
			this.monsterName = monsterName;
			this.hp.Value = hp;
			this.attack.Value = attack;
			this.MaxGuts = guts / 10 > 0 ? guts / 10 : 1;
			this.guts.Value = 0;
			this.diffence.Value = diffence;
			this.speed.Value = speed;
			this.luck.Value = luck;
		}

		public int MonsterModelId { get; private set; }
		public int MonsterIdx { get; private set; }
		public string MonsterName => monsterName;
		public IReadOnlyReactiveProperty<int> HP => hp;
		public IReadOnlyReactiveProperty<int> Attack => attack;
		public IReadOnlyReactiveProperty<int> Guts => guts;
		public IReadOnlyReactiveProperty<int> Diffence => diffence;
		public IReadOnlyReactiveProperty<int> Speed => speed;
		public IReadOnlyReactiveProperty<int> Luck => luck;
		public int MaxGuts { get; private set; }

		public void AddDamage(int damage)
		{
			hp.Value -= damage;
			if (hp.Value < 0) hp.Value = 0;
		}

		public void Charge()
		{
			// todo 引数でガッツを指定
			var val = guts.Value + 1;
			if (MaxGuts < val) return;
			guts.Value += 1;
		}

		public void ReduceGuts()
		{
			// todo 引数でガッツを指定
			guts.Value = 0;
		}

		public bool IsDown()
		{
			return hp.Value == 0;
		}
	}
}
