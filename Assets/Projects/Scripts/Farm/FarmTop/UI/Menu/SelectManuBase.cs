/*********************************************
* FileName :SelectManuBase.cs
* Summary : メニューUIのベースクラス
*********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Voxel.Farm
{
	public class SelectManuBase : MonoBehaviour
	{
		[SerializeField] private MenuItemBase itemPrefab;
		[SerializeField, Range(1, 3)] private int columnCount;
		[SerializeField] private Transform itemParent;
		private MenuItemBase[] items;
		private string[] itemStrs; // 選択項目の文字列
		private bool isCreate;
		private SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();

		public ReactiveProperty<int> CurrentIdx { get; private set; } = new ReactiveProperty<int>();

		public void Initialize(string[] menuItemStrs)
		{
			this.itemStrs = menuItemStrs;
		}

		private void Bind()
		{
			disposable = new SingleAssignmentDisposable();
			disposable.Disposable = CurrentIdx
				.Where(_ => isCreate)
				.Subscribe(x => 
				{
					for (int i = 0; i < items.Length; i++) items[i].UnSelect();
					items[x].Select();
				}).AddTo(this);
		}

		public void Show()
		{
			if (!isCreate) Create();
			itemParent.gameObject.SetActive(true);
			Bind();
		}

		public void Hide()
		{
			itemParent.gameObject.SetActive(false);
			disposable.Dispose();
		}

		public void SelectUp()
		{
			var nextIdx = CurrentIdx.Value - columnCount;
			if (nextIdx < 0) return;
			CurrentIdx.Value = nextIdx;
		}

		public void SelectDown()
		{
			var nextIdx = CurrentIdx.Value + columnCount;
			if (items.Length <= nextIdx) return;
			CurrentIdx.Value = nextIdx;
		}

		public void SelectLeft()
		{
			if (CurrentIdx.Value % columnCount == 0) return;
			var nextIdx = CurrentIdx.Value;
			nextIdx--;
			if (nextIdx < 0) return;
			CurrentIdx.Value = nextIdx;
		}

		public void SelectRight()
		{
			if (CurrentIdx.Value % columnCount == columnCount - 1) return;
			var nextIdx = CurrentIdx.Value;
			nextIdx++;
			if (items.Length <= nextIdx) return;
			CurrentIdx.Value = nextIdx;
		}

		private void Create()
		{
			items = new MenuItemBase[itemStrs.Length];
			for (int i = 0; i < itemStrs.Length; i++)
			{
				var item = Instantiate(itemPrefab, itemParent);
				item.Initialize(itemStrs[i]);
				items[i] = item.GetComponent<MenuItemBase>();
			}
			isCreate = true;
		}
	}
}
