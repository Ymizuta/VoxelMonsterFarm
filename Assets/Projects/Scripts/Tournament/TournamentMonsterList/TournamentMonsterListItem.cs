using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Tournament
{
	public class TournamentMonsterListItem : MonoBehaviour
	{
		[SerializeField] Text numberText = null;
		[SerializeField] Text nameText = null;
		[SerializeField] Image faceImg = null;
		[SerializeField] Sprite[] faceSprits;

		public void SetData(int number, string name, int faceTypeId)
		{
			numberText.text = number.ToString();
			nameText.text = name;
			faceImg.sprite = faceSprits[faceTypeId];
		}
	}
}
