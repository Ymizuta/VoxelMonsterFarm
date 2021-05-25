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

		public ReactiveProperty<CommandType> Command = new ReactiveProperty<CommandType>();

		private TournamentMonsterParam[] monsterParams = new TournamentMonsterParam[] 
		{
			new TournamentMonsterParam("�C�b�k", 100, 30, 30, 25, 20, 45),
			new TournamentMonsterParam("�l�b�R", 100, 30, 30, 25, 20, 45),
			new TournamentMonsterParam("�E�b�V", 100, 30, 30, 25, 20, 45),
			new TournamentMonsterParam("�g�b��", 100, 30, 30, 25, 20, 45),
			new TournamentMonsterParam("�u�b�^", 100, 30, 30, 25, 20, 45),
			new TournamentMonsterParam("�q�c�b�W", 100, 30, 30, 25, 20, 45),
			new TournamentMonsterParam("���b�M", 100, 30, 30, 25, 20, 45),
			new TournamentMonsterParam("�h���S��", 100, 30, 30, 25, 20, 45),
		};

		public TournamentMonsterParam[] MonsterParams => monsterParams;
		public string[] MenuStrs { get; private set; } = new string[] {"����", "����", "������", };

		public override void Initialize()
		{
			base.Initialize();
		}
	}
}
