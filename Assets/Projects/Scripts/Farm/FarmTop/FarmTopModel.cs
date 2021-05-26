using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Voxel.UI;

namespace Voxel.Farm
{
	public class FarmTopModel : ModelBase
	{
		/// <summary>
		/// ������
		/// </summary>
		public enum CommandType
		{
			None,
			FarmTopMenu,
			TrainingMenu,
			MonsterParam,
		}

		public enum FarmTopMenu
		{
			Training = 0, // �琬
			TakeRest, // �x�{
			Tournament, // ���
			Params,
		}

		public enum TrainingMenu
		{
			Running = 0, // ���荞��
			ObstacleCourse, // ��Q���R�[�X
			Swimming, // ���j
			Meditation, // �ґz
			DestroyObstacle, // ��Q���j��
		}

		public ReactiveProperty<CommandType> Command = new ReactiveProperty<CommandType>();

		public readonly string[] FarmTopMenuStrs = new string[] { "�琬", "�x�{", "���", "�\�͒l"};
		public readonly string[] TrainingMenuStrs = new string[] { "���荞��", "��Q��", "���j", "�ґz", "�Ō��P��"};

		public MonsterParam MonsterParam { get; private set; }

		public override void Initialize()
		{
			base.Initialize();
			MonsterParam = SaveDataManager.SaveData.CurrentMonster;
		}

		public string GetInitComment()
		{
			var comments = new string[] 
			{
				"�C�b�k �͌��C����I",
				"�C�b�k �͂����������C����I",
				"�C�b�k �͏������Ă�݂����c",
			};
			return comments[Random.Range(0,3)];
		}

		protected override void OnBack()
		{
			base.OnBack();
		}
	}
}
