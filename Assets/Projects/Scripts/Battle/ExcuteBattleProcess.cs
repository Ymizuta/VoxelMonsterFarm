using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.UI;

namespace Voxel.Battle
{
	public class ExcuteBattleProcess : MonoBehaviour
	{
		private int[] orderArray = new int[2];
		private GameObject[] monsterObjects;

		public void Initialize(GameObject myMonster, GameObject enemyMonster)
		{
			monsterObjects = new GameObject[]
			{
				myMonster,
				enemyMonster,
			};
		}

		public IEnumerator StartProcess(BattleMonsterParam[] monsters, CommandParam[] commands)
		{
			// todo �s���������߂�
			var myMonster = monsters[0];
			var enemy = monsters[1];
			// todo ��������Ō�����
			// �f������
			if (myMonster.Speed.Value >= enemy.Speed.Value) orderArray = new int[] { 0,1};
			else if(myMonster.Speed.Value < enemy.Speed.Value) orderArray = new int[] { 1, 0 };
			// �h�䂵�Ă���Ȃ��U
			if (commands[orderArray[0]].command != BattleCommand.Guard && commands[orderArray[1]].command == BattleCommand.Guard)
			{
				// ����ւ�
				var later = orderArray[0];
				orderArray[0] = orderArray[1];
				orderArray[1] = later;
			}

			// �L���[���s
			for (int i = 0; i < orderArray.Length; i++)
			{
				var idx = orderArray[i];
				var targetIdx = idx == 0 ? 1 : 0;
				yield return ExecCoroutine(
					monsters[idx], monsterObjects[idx], commands[idx],
					monsters[targetIdx], monsterObjects[targetIdx], commands[targetIdx]);
				if (monsters[targetIdx].IsDown()) yield break;
			}
		}

		private IEnumerator ExecCoroutine(
			BattleMonsterParam param, 
			GameObject monsterObj, 
			CommandParam command, 
			BattleMonsterParam targetParam, 
			GameObject enemyObj, 
			CommandParam targetCommand)
		{
			switch (command.command)
			{
				case BattleCommand.Attack:
					yield return ExecuteAttackQueueCoroutine(monsterObj, enemyObj);
					break;
				case BattleCommand.Guard:
					yield return ExecuteGuardCoroutine();
					break;
				case BattleCommand.Charge:
					yield return ExecuteChargeCoroutine(monsterObj);
					break;
			}
			yield return ExcuteAction(param, command, targetParam, targetCommand);
			if (targetParam.IsDown())
			{
				yield return DownAnimationCoroutine(enemyObj);
				yield break;
			}
		}

		private IEnumerator ExcuteAction(BattleMonsterParam monster, CommandParam command, BattleMonsterParam target, CommandParam targetCommand)
		{
			switch (command.command)
			{
				case BattleCommand.Attack:
					monster.ReduceGuts();
					var damage = CalcDamage(monster, command, target, targetCommand);
					target.AddDamage(damage);
					Comment.Instance.Show($"{target.MonsterName}��<color=#ff0000>{damage}</color>�̃_���[�W! �c��̗͂�{target.HP.Value}");
					break;
				case BattleCommand.Charge:
					monster.Charge();
					Comment.Instance.Show($"{monster.MonsterName}�̓K�b�c�𗭂߂Ă���! �K�b�c��{monster.Guts.Value}�ɂȂ����I");
					break;
				case BattleCommand.Guard:
					// �ꎞ�I�ɖh��͂��グ��Ƃ��H
					Comment.Instance.Show($"{monster.MonsterName}�͐g������Ă���");
					break;
			}
			yield return new WaitForSeconds(1f);
			yield return null;
		}

		private int CalcDamage(BattleMonsterParam monster, CommandParam command, BattleMonsterParam target, CommandParam targetCommand)
		{
			// �T�u�N���X�������������Čv�Z�Ƃ�����������������
			// �h��ђʂƂ����邩��
			// �����U���̓}�X�^�Ń_���[�W�{���Ƃ��ݒ肵��������������
			// �U�ꕝ������B������}�X�^�Łi��
			var damage = (int)(monster.Attack.Value * Random.Range(0.8f, 1.2f));
			var adjustVal = 1.0f;
			switch (targetCommand.command)
			{
				case BattleCommand.Attack:
					adjustVal = 1.0f;
					break;
				case BattleCommand.Charge:
					adjustVal = 2.0f;
					break;
				case BattleCommand.Guard:
					// todo �{���͂����Ŗh��͂Ƃ���������������������
					adjustVal = 0.8f;
					break;
			}
			damage = (int)(damage * adjustVal) - target.Diffence.Value;
			if (damage < 0) damage = 0;
			return damage;
		}

		private IEnumerator DownAnimationCoroutine(GameObject downMonster)
		{
			var animator = downMonster.GetComponent<Animator>();
			animator.Play("Down");
			yield return null;
			yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f );
		}

		private IEnumerator ExecuteAttackQueueCoroutine(GameObject attacker, GameObject receiver)
		{			
			var queue = new Queue<Content>();
			Content attackContent = new Content()
			{
				action = new Action { characterObject = attacker, animationStr = "Attack"}
			};
			queue.Enqueue(attackContent);
			Content damageContent = new Content()
			{
				damage = new Damage { receiverObject = receiver}
			};
			queue.Enqueue(damageContent);

			// ���s
			while (queue.Count > 0)
			{
				var content = queue.Dequeue();
				if(content.action != null)
				{
					content.action.Excute();
					yield return null;
					yield return new WaitUntil(() => content.action.IsFinishAnimation);
				}
				if (content.damage != null)
				{
					content.damage.Excute();
				}
			}
		}

		private IEnumerator ExecuteGuardCoroutine()
		{
			yield return null;
		}

		private IEnumerator ExecuteChargeCoroutine(GameObject monsterObj)
		{
			monsterObj.GetComponent<BattleMonsterEffects>().PlayCharge();
			yield return null;
		}
	}

	/// <summary>
	/// ���o�L���[�ɋl�߂鉉�o�\����
	/// </summary>
	public struct Content
	{
		public Action action;
		public Damage damage;
	}

	/// <summary>
	/// �U�����̉��o�N���X
	/// </summary>
	public class Action
	{
		public GameObject characterObject;
		public string animationStr;
		public bool IsFinishAnimation { get
			{
				// ���݂̃A�j���[�V�������������Ă��邩
				return characterObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
			} }

		/// <summary>
		/// ���o���s
		/// </summary>
		public void Excute()
		{
			characterObject.GetComponent<Animator>().Play(animationStr);
		}
	}

	/// <summary>
	/// �_���[�W�̌v�Z
	/// </summary>
	public class Damage
	{
		public GameObject receiverObject;

		public void Excute()
		{
			receiverObject.GetComponent<Animator>().Play("Damaged");
			receiverObject.GetComponent<BattleMonsterEffects>().PlayAttacked();
		}
	}
}
