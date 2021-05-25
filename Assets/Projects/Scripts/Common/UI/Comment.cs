using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.UI
{
	public class Comment : SingletonMonoBehaviour<Comment>
	{
		[SerializeField] private Text commentText;
		[SerializeField] private Transform commentTransform;

		public void Show()
		{
			commentTransform.gameObject.SetActive(true);
		}

		public void Show(string comment)
		{
			SetComment(comment);
			Show();
		}

		public void Hide()
		{
			commentTransform.gameObject.SetActive(false);
		}

		public void SetComment(string comment)
		{
			commentText.text = comment;
		}
	}
}
