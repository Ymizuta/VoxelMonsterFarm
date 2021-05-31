using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Voxel.UI
{
	public class ConfirmPopup : SingletonMonoBehaviour<ConfirmPopup>
	{
		[SerializeField] private RectTransform popupRoot;
		[SerializeField] Text popupComment;
		private CompositeDisposable disposable;

		public void Show(Action yesAction, string comment = "")
		{
			GameCommonModel.Instance.IsPopupShowed = true;
			popupComment.text = comment;
			popupRoot.gameObject.SetActive(true);
			// 入力イベント等を受付
			Bind(yesAction);
		}

		private void Hide()
		{
			popupRoot.gameObject.SetActive(false);
			OnHide();
		}

		private void Bind(Action action)
		{
			disposable = new CompositeDisposable();
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Subscribe(_ => 
				{
					action?.Invoke();
					Hide();
				}).AddTo(disposable);
			InputManager.Instance.OnBKeyDownAsObservable
				.Subscribe(_ =>
				{
					action?.Invoke();
					Hide();
				}).AddTo(disposable);
		}

		private void OnHide()
		{
			GameCommonModel.Instance.IsPopupShowed = false;
			disposable.Dispose();
		}
	}
}
