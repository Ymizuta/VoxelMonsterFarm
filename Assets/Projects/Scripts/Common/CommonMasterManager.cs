using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Common
{
	public class CommonMasterManager : SingletonMonoBehaviour<CommonMasterManager>
	{
		[SerializeField] private MonsterModelMaster monsterModelMaster;
		[SerializeField] private Entity_MonsterParamMaster monsterParamMaster;

		public MonsterModelMaster MonsterModelMaster => monsterModelMaster;
		public Entity_MonsterParamMaster MonsterParamMaster => monsterParamMaster;
	}
}
