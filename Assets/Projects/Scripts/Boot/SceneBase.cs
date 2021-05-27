using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.SceneManagement
{
	public class SceneBase : MonoBehaviour
	{
		public virtual void Initialize(SceneData data = null)
		{
		}
	}

	public class SceneData
	{
	}
}
