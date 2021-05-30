using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Common
{
	public class CommonMasterManager : SingletonMonoBehaviour<CommonMasterManager>
	{
		[SerializeField] private MonsterModelMaster monsterModelMaster;
		public MonsterModelMaster MonsterModelMaster => monsterModelMaster;
	}
}
