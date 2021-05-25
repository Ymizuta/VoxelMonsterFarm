using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.UI;

namespace Voxel.Tournament
{
	[RequireComponent(typeof(TournamentModel))]
	[RequireComponent(typeof(TournamentView))]
	public class TournamentPresenter : PresenterBase
	{
		new TournamentModel Model => base.Model as TournamentModel;
		new TournamentView View => base.View as TournamentView;

		private void Awake()
		{
			StartCoroutine(Initialize());
		}

		public override IEnumerator Initialize()
		{
			if (isInit) yield break;
			yield return base.Initialize();

			// ‘JˆÚ‘Oˆ—
			View.OnBeforeMoveIn();

			isInit = true;

			FadeManager.Instance.PlayFadeIn(() =>
			{
				// ƒCƒxƒ“ƒg“o˜^
			});
		}

		public override IEnumerator Run()
		{
			yield return base.Run();			
		}
	}
}
