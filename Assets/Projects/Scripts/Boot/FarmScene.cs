using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.SceneManagement;

namespace Voxel.Farm
{
	public class FarmScene : SceneBase
	{
		public override void Initialize(SceneData data)
		{
			base.Initialize(data);
			FarmPresenterManager.Instance.Initialize();
		}
	}
}
