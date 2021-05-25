using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Tournament
{
	public class TournamentView : ViewBase
	{
		private TournamentBoard board;
		private bool isLoaded;

		public TournamentBoard Board => board;	

		public override IEnumerator Initialize()
		{
			yield return base.Initialize();
			Load(() => { isLoaded = true; });
			yield return new WaitUntil(() => isLoaded);
		}

		public void OnBeforeMoveIn()
		{
		}

		protected override void OnBack()
		{
			base.OnBack();
		}

		private async void Load(Action action = null)
		{
			// �{�[�h
			var result = await AddressableManager.Instance.Load("Assets/Projects/AddressableAssets/Prefabs/Tournament/TournamentBoard.prefab");
			board = Instantiate(result, CanvasManager.Instance.FrontCanvas.transform).GetComponent<TournamentBoard>();
			action?.Invoke();
		}
	}
}
