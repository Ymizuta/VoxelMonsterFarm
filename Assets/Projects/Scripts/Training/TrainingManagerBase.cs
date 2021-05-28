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
		public virtual IEnumerator Run(TrainingResult result)
		{
			yield return null;
		}
	}
}
