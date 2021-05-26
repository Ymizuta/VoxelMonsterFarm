using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Voxel.Tournament
{
	public class TournamentModel : ModelBase
	{
		public enum CommandType
		{
			None,
			TopMenu,
			TournamentBoard,
		}

		public enum TopMenuType
		{
			Match = 0, // ����
			AbstentionNextMatch, // ���̎���������
			AbstentionTournament, // ������
		}

		public ReactiveProperty<CommandType> Command = new ReactiveProperty<CommandType>();

		public TournamentMonsterParam[] MonsterParams => TournamentCommonModel.Instance.MonsterParams;
		public string[] MenuStrs { get; private set; } = new string[] {"����", "����", "������", };

		public override void Initialize()
		{
			base.Initialize();
		}
	}
}
