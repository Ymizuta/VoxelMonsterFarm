using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Training
{
	public enum TrainingResult
	{
		Success,
		Failure,
		GreatSuccess,
	}

	public abstract class TrainingManagerBase : MonoBehaviour
	{
		[SerializeField] private Transform monsterObjectRoot;

		public virtual void Initialize(int monsterModelId)
		{
			var obj = Instantiate(Common.CommonMasterManager.Instance.MonsterModelMaster.GetMonsterObject(monsterModelId), monsterObjectRoot);
			obj.transform.Rotate(new Vector3(0f, 180f, 0f));
		}

		public virtual IEnumerator Run(TrainingResult result)
		{
			yield return null;
		}
	}
}
