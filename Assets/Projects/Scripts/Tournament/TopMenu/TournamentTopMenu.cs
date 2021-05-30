using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.Common;

namespace Voxel.Tournament
{
	public class TournamentTopMenu : SelectManuBase
	{
		[SerializeField] GameObject bgObject;

		public override void Show()
		{
			base.Show();
			//bgObject.SetActive(true);
		}

		public override void Hide()
		{
			base.Hide();
			//bgObject.SetActive(false);
		}
	}
}
