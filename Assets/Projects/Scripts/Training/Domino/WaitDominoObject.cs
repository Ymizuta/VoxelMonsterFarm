using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Voxel.Training
{
	public class WaitDominoObject : MonoBehaviour
	{
		[SerializeField] Collider finishCollider;

		public bool IsFinished { get; private set; }

		private void Awake()
		{
			finishCollider.OnTriggerEnterAsObservable()
				.Subscribe(collision => 
				{
					if (collision.gameObject.tag == "Ground")
					{
						IsFinished = true;
					}				
				}).AddTo(this);
		}
	}
}
