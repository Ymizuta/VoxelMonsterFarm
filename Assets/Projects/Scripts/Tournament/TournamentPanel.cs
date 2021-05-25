using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Tournament
{
	public class TournamentPanel : MonoBehaviour
	{
		[SerializeField] private RectTransform rtf;
		[SerializeField] private Image resultImg;
		[SerializeField] private Sprite[] sprits;

		public RectTransform Rtf => rtf;

		public void SetData(ResultType type)
		{
			if (type == ResultType.None)
			{
				resultImg.enabled = false;
			}
			else
			{
				resultImg.enabled = true;
				resultImg.sprite = sprits[(int)type];
			}
		}
	}
}
