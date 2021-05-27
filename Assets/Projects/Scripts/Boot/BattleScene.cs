using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.SceneManagement;

namespace Voxel.Battle
{
	public class BattleScene : SceneBase
	{
		public override void Initialize(SceneData data = null)
		{
			base.Initialize(data);
			GetComponent<BattleManager>().Initialize(data);
		}
	}
}