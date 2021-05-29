using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Common
{
	public class MenuItemBase : MonoBehaviour
	{
		[SerializeField] private Text itemNameText = null;
		protected Color unSelectedColor= new Color(132f/255f, 0f / 255f, 0f / 255f, 255f / 255f);
		protected Color selectedColor = Color.white;

		public virtual void Initialize(string itemName)
		{
			this.itemNameText.text = itemName;
		}

		public virtual void Select()
		{
			itemNameText.color = selectedColor;
		}

		public virtual void UnSelect()
		{
			itemNameText.color = unSelectedColor;
		}
	}
}
