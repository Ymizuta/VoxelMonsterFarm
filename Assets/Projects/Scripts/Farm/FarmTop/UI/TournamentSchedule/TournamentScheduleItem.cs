using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Farm
{
	public class TournamentScheduleItem : MonoBehaviour
	{
		[SerializeField] private RectTransform rtf;
		[SerializeField] private Sprite[] gradeSprits;
		[SerializeField] private Sprite emptySprit;
		[SerializeField] private Text tournamentText;
		[SerializeField] private Image panelImg;

		public RectTransform Rtf => rtf;

		public void SetData(TournamentGrade grade)
		{
			if(grade == TournamentGrade.None)
			{
				panelImg.sprite = emptySprit;
				tournamentText.text = "-";
			}
			else
			{
				panelImg.sprite = gradeSprits[(int)grade];
				tournamentText.text = grade.ToString();
			}
		}

		public void ResetData()
		{
			tournamentText.text = "";
		}

		public void Select()
		{
		}

		public void UnSelect()
		{
		}
	}
}
