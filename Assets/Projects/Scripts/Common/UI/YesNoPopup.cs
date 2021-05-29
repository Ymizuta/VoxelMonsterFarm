using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Voxel.UI
{
	public class YesNoPopup : SingletonMonoBehaviour<YesNoPopup>
	{
		[SerializeField] private Transform popupRoot;
		[SerializeField] private Transform yesButton;
		[SerializeField] private Transform noButton;
		[SerializeField] private Text popupComment;

		private ReactiveProperty<bool> isYesSelected; // Yesボタンを選択しているか
		private CompositeDisposable disposable;

		/// <summary>
		/// YES/NO選択のポップアップを表示する
		/// </summary>
		/// <param name="yesAction"></param>
		/// <param name="noAction"></param>
		/// <param name="comment"></param>
		public void Show(Action yesAction, Action noAction, string comment = "")
		{
			GameCommonModel.Instance.IsPopupShowed = true;
			popupComment.text = comment;
			popupRoot.gameObject.SetActive(true);
			// 入力イベント等を受付
			Bind(yesAction, noAction);
		}

		private void Hide()
		{
			popupRoot.gameObject.SetActive(false);
			OnHide();
		}

		private void Bind(Action yesAction, Action noAction)
		{
			disposable = new CompositeDisposable();
			isYesSelected = new ReactiveProperty<bool>(true);

			// YES/NO 選択しているボタンの処理を実行
			InputManager.Instance.OnSpaceKeyDownAsObservable
				.Subscribe(_ => 
				{
					Hide();
					if (isYesSelected.Value)
					{
						yesAction?.Invoke();
					}
					else
					{
						noAction?.Invoke();
					}
				}).AddTo(disposable);
			// Bボタン
			InputManager.Instance.OnBKeyDownAsObservable
				.Subscribe(_ => 
				{
					Hide();
					noAction?.Invoke();
				}).AddTo(disposable);
			// 左右キーの操作でYES/NOが切り替わると表示を更新
			isYesSelected.
				Subscribe(x => 
				{
					if (x)
					{
						yesButton.GetComponentInChildren<Text>().color = Color.yellow;
						noButton.GetComponentInChildren<Text>().color = Color.white;
					}
					else
					{
						yesButton.GetComponentInChildren<Text>().color = Color.white;
						noButton.GetComponentInChildren<Text>().color = Color.yellow;
					}
				}).AddTo(disposable);

			InputManager.Instance.OnRightKeyDownAsObservable
				.Subscribe(_ => 
				{
					isYesSelected.Value = !isYesSelected.Value;
				}).AddTo(disposable);

			InputManager.Instance.OnLeftKeyDownAsObservable
				.Subscribe(_ => 
				{
					isYesSelected.Value = !isYesSelected.Value;
				}).AddTo(disposable);
		}

		private void OnHide()
		{
			GameCommonModel.Instance.IsPopupShowed = false;
			disposable.Dispose();
		}
	}
}
