using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Common
{
	public static class MonsterParamMasterManager
	{
		public static MonsterParam GetParam(int monsterId)
		{
			var data = CommonMasterManager.Instance.MonsterParamMaster.sheets[0].list.Find(x => x.id == monsterId);
			return GetMonsterParam(data);
		}

		/// <summary>
		/// �}�X�^����g�[�i�����g�p�������擾
		/// </summary>
		/// <param name="monsterId"></param>
		/// <returns></returns>
		public static Tournament.TournamentMonsterParam GetTournamentParam(int monsterId)
		{
			var conv = new Tournament.MonsterTournamentParamConvertor();
			return conv.ConvertMonsterParamToTournamentParam(GetParam(monsterId));
		}

		// todo �F��ȏ����łЂ���������悤�ɂ��������悢
		public static List<MonsterParam> GetMonsterParams(TournamentGrade grade)
		{
			var datas = CommonMasterManager.Instance.MonsterParamMaster.sheets[0].list.FindAll(x => x.grade == (int)grade);
			var list = new List<MonsterParam>();
			for (int i = 0; i < datas.Count; i++)
			{
				list.Add(GetMonsterParam(datas[i]));
			}
			return list;
		}

		/// <summary>
		/// �n���ꂽ�}�X�^�f�[�^���烂���X�^�[�p������Ԃ�
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		private static MonsterParam GetMonsterParam(Entity_MonsterParamMaster.Param data)
		{
			return new MonsterParam()
			{
				MonseterId = data.id,
				MonsterModelId = data.modelId,
				MonsterName = data.monsterName,
				Hp = data.hp,
				Guts = data.guts,
				Power = data.power,
				Hit = data.hit,
				Speed = data.speed,
				Deffence = data.deffence,
			};
		}
	}
}
