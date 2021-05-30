using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Voxel.Farm
{
	public class FarmMonster : MonoBehaviour
	{
		[SerializeField] private Transform root;

		private void Awake()
		{
			var obj = Instantiate(Common.CommonMasterManager.Instance.MonsterModelMaster.GetMonsterObject(SaveDataManager.SaveData.CurrentMonster.MonsterModelId), root);
			obj.transform.Rotate(new Vector3(0f, 180f, 0f));
			this.gameObject.transform.position = new Vector3(5f, 0f, 0f);
			MoveLeft();
		}

		private void MoveLeft()
		{
			var target = new Vector3(-3f, 0f, 0f);
			this.gameObject.transform.LookAt(target);
			DOTween.Sequence()
				.Append(this.gameObject.transform.DOJump(target, 2f, 5, 5f).SetEase(Ease.Linear).OnComplete(() =>
				{
					this.gameObject.transform.LookAt(new Vector3(-3f, 0f, -1f));
				}))
				.AppendInterval(2)
				.OnComplete(MoveRight);				
		}

		private void MoveRight()
		{
			var target = new Vector3(3f, 0f, 0f);
			this.gameObject.transform.LookAt(target);
			this.gameObject.transform
				.DOMove(target, 5f)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					MoveLeft();
				});
		}
	}
}
