using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Farm
{
	public class MenuItemBase : MonoBehaviour
	{
		[SerializeField] private Text itemNameText = null;

		public virtual void Initialize(string itemName)
		{
			this.itemNameText.text = itemName;
		}

		public virtual void Select()
		{
			itemNameText.color = Color.yellow;
		}

		public virtual void UnSelect()
		{
			itemNameText.color = Color.white;
		}
	}
}
