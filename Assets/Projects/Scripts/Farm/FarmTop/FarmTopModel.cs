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
			Domino, // �h�~�m�|��
			Shooting, // �˓I
			Studying, // �׋�
			AvoidRock, // �����
			LogShock, // �ۑ���
		}

		public ReactiveProperty<CommandType> Command = new ReactiveProperty<CommandType>();

		public readonly string[] FarmTopMenuStrs = new string[] { "�琬", "�x�{", "���", "�\�͒l"};
		public readonly string[] TrainingMenuStrs = new string[] { "���荞��", "�h�~�m�|��", "����Ă�", "�ҕ׋�", "���Δ���", "�ۑ���" };

		public MonsterParam MonsterParam { get; private set; }
		public bool IsOperatable { get
			{
				return !GameCommonModel.Instance.IsPopupShowed;
			} }

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
