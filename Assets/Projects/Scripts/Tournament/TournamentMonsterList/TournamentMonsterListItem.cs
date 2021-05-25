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

		public void SetData(int number, string name)
		{
			numberText.text = number.ToString();
			nameText.text = name;
		}
	}
}
