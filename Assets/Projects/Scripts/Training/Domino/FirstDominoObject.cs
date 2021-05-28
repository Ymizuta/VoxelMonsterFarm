using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Training
{
	public class FirstDominoObject : MonoBehaviour
	{
		public void Down()
		{
			GetComponent<Rigidbody>().AddTorque(gameObject.transform.right * 100f, ForceMode.Acceleration);
		}
	}
}
