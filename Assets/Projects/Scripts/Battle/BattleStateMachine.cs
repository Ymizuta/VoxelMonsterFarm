using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.UI;
using UniRx;
using Voxel.Tournament;

namespace Voxel.Battle
{
	public class BattleStateMachine : MonoBehaviour
	{
		[SerializeField] BattleMonsterStatusUI[] statusUis;

		BattleMonsterParam myMonster;
		BattleMonsterParam enemyMonster;
		PlayerCommandProcess commandProcess;
		ExcuteBattleProcess excuteBattleProcess;

		private void Awake()
		{
			StartStateMachine();
		}

		public void StartStateMachine()
		{
			StartCoroutine(BattleMainProcess());
		}

		private IEnumerator BattleMainProcess()
		{
			commandProcess = this.gameObject.AddComponent<PlayerCommandProcess>();
			excuteBattleProcess = this.gameObject.GetComponent<ExcuteBattleProcess>();
			this.myMonster = new BattleMonsterParam("ピカチュウ", 100, 25, 0, 10, 30, 30);
			this.enemyMonster = new BattleMonsterParam("コラッタ", 75, 20, 0, 15, 35, 30);
			statusUis[0].SetData(myMonster);
			statusUis[1].SetData(enemyMonster);

			yield return BattleProcess(myMonster, enemyMonster);
			yield return EndBattleProcess();
		}

		private IEnumerator BattleProcess(BattleMonsterParam myMonster, BattleMonsterParam enemyMonster)
		{
			// プレイヤーの操作→行動を決める
			var myCommandParam = new CommandParam();
			yield return commandProcess.StartProcess(myMonster, myCommandParam);

			// 敵の行動を決める
			var enemyCommandParam = new CommandParam();
			yield return commandProcess.StartProcess(enemyMonster, enemyCommandParam);

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
			yield return new WaitForSeconds(2f);
			var loser = myMonster.IsDown() ? myMonster.MonsterName : enemyMonster.MonsterName;
			Comment.Instance.SetComment($"{loser}は倒れた！");
			yield return new WaitForSeconds(3f);
			TournamentCommonModel.Instance.SetResult(winnerIdx : 0, loserIdx: 1);
			FadeManager.Instance.PlayFadeOut(() => 
			{
				Comment.Instance.Hide();
				SceneManagement.SceneLoader.ChangeScene(SceneManagement.SceneLoader.SceneName.Tournament);
			});
		}

		/// <summary>
		/// 決着しているか
		/// </summary>
		/// <returns></returns>
		private bool IsAnyDown()
		{
			return myMonster.IsDown() || enemyMonster.IsDown();
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

		public BattleMonsterParam(string monsterName, int hp, int attack, int guts, int diffence, int speed, int luck)
		{
			this.monsterName = monsterName;
			this.hp.Value = hp;
			this.attack.Value = attack;
			this.guts.Value = guts;
			this.diffence.Value = diffence;
			this.speed.Value = speed;
			this.luck.Value = luck;
		}

		public string MonsterName => monsterName;
		public IReadOnlyReactiveProperty<int> HP => hp;
		public IReadOnlyReactiveProperty<int> Attack => attack;
		public IReadOnlyReactiveProperty<int> Guts => guts;
		public IReadOnlyReactiveProperty<int> Diffence => diffence;
		public IReadOnlyReactiveProperty<int> Speed => speed;
		public IReadOnlyReactiveProperty<int> Luck => luck;

		public void AddDamage(int damage)
		{
			hp.Value -= damage;
			if (hp.Value < 0) hp.Value = 0;
		}

		public void Charge()
		{
			// todo 引数でガッツを指定
			guts.Value += 1;
		}

		public void ReduceGuts()
		{
			// todo 引数でガッツを指定
			guts.Value -= 1;
		}

		public bool IsDown()
		{
			return hp.Value == 0;
		}
	}
}
