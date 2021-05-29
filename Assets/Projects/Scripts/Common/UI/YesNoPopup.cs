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
		private Color unSelectedColor = new Color(132f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
		private Color selectedColor = Color.white;

		private ReactiveProperty<bool> isYesSelected; // Yes�{�^����I�����Ă��邩
		private CompositeDisposable disposable;

		/// <summary>
		/// YES/NO�I���̃|�b�v�A�b�v��\������
		/// </summary>
		/// <param name="yesAction"></param>
		/// <param name="noAction"></param>
		/// <param name="comment"></param>
		public void Show(Action yesAction, Action noAction, string comment = "")
		{
			GameCommonModel.Instance.IsPopupShowed = true;
			popupComment.text = comment;
			popupRoot.gameObject.SetActive(true);
			// ���̓C�x���g������t
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

			// YES/NO �I�����Ă���{�^���̏��������s
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
			// B�{�^��
			InputManager.Instance.OnBKeyDownAsObservable
				.Subscribe(_ => 
				{
					Hide();
					noAction?.Invoke();
				}).AddTo(disposable);
			// ���E�L�[�̑����YES/NO���؂�ւ��ƕ\�����X�V
			isYesSelected.
				Subscribe(x => 
				{
					if (x)
					{
						yesButton.GetComponentInChildren<Text>().color = selectedColor;
						noButton.GetComponentInChildren<Text>().color = unSelectedColor;
					}
					else
					{
						yesButton.GetComponentInChildren<Text>().color = unSelectedColor;
						noButton.GetComponentInChildren<Text>().color = selectedColor;
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
