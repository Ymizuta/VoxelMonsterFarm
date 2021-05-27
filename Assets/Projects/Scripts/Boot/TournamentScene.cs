using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.SceneManagement;

namespace Voxel.Tournament
{
	public class TournamentScene : SceneBase
	{
		[SerializeField] private TournamentPresenter tournamentPresenter;

		public override void Initialize(SceneData data = null)
		{
			base.Initialize(data);
			StartCoroutine(tournamentPresenter.Run(data));
		}
	}
}
