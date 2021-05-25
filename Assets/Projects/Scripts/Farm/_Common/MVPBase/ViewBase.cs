using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel
{
	public class ViewBase : MonoBehaviour
	{
		public virtual IEnumerator Initialize()
		{
			yield return null;
		}

		protected virtual void OnBack()
		{
		}
	}
}
