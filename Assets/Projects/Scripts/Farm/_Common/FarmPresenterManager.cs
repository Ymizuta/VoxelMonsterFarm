using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Farm
{
	public  class FarmPresenterManager : SingletonMonoBehaviour<FarmPresenterManager>
	{
		public enum FarmPresenterType
		{
			FarmTop = 0,
		}

		[SerializeField] private PresenterBase[] presenters;

		private PresenterBase currentPresenter;
		private PresenterBase pausePresenter;

		protected override void Awake()
		{
			base.Awake();
			Initialize();
		}

		private void Initialize()
		{
			Calendar.Instance.Initialize();
			ChangePresenter(FarmPresenterType.FarmTop);
		}

		public void ChangePresenter(FarmPresenterType type)
		{
			if(currentPresenter != null) currentPresenter.OnBack();
			if (pausePresenter != null) pausePresenter.OnBack();
			currentPresenter = presenters[(int)type];
			StartCoroutine(currentPresenter.Run());
		}

		/// <summary>
		/// ���݂̃v���[���^�[���ێ����ăv���[���^�[��ǉ��\��
		/// </summary>
		/// <param name="type"></param>
		public void AddPresenter(FarmPresenterType type)
		{
			pausePresenter = currentPresenter;
			pausePresenter.Pause();
			currentPresenter = presenters[(int)type];
			StartCoroutine(currentPresenter.Run());
		}

		/// <summary>
		/// AddPresenter����Ē��O�̃v���[���^�[�ɖ߂�
		/// </summary>
		public void BackPresenter()
		{
			currentPresenter.OnBack();
			currentPresenter = pausePresenter;
			pausePresenter = null;
			currentPresenter.Restart();
		}
	}
}
