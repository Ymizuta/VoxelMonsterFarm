using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Voxel.SceneManagement;
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
			MonsterParam = new MonsterParam();
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

		/// <summary>
		/// �g���[�j���O���j���[�����肵���Ƃ��ɌĂ΂�A�Y���V�[���֑J�ڂ���
		/// </summary>
		/// <param name="selectIdx"></param>
		public void OnDecideTrainingMenu(int selectIdx)
		{
			var type = (TrainingMenu)selectIdx;
			var sceneName = SceneLoader.SceneName.Running;
			switch (type)
			{
				case TrainingMenu.Running:
					sceneName = SceneLoader.SceneName.Running;
					break;
				case TrainingMenu.Swimming:
					break;
				case TrainingMenu.ObstacleCourse:
					break;
				case TrainingMenu.Meditation:
					break;
				case TrainingMenu.DestroyObstacle:
					break;
				default:
					Debug.LogWarning("�z�肵�Ă��Ȃ��^�C�v���I������܂��� type =" + type);
					break;
			}
			FadeManager.Instance.PlayFadeOut(() => SceneLoader.ChangeScene(sceneName));
		}

		protected override void OnBack()
		{
			base.OnBack();
		}
	}
}
