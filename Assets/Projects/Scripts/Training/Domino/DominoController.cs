using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Training
{
	public class DominoController : MonoBehaviour
	{
		[SerializeField] private FirstDominoObject firstDomino;
		[SerializeField] private WaitDominoObject lastDomino;

		private bool isFinish;

		public IEnumerator Run()
		{
			firstDomino.Down();

			yield return new WaitUntil(() => lastDomino.IsFinished);
		}
	}
}
