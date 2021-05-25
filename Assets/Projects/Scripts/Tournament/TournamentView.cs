using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Tournament
{
	public class TournamentView : ViewBase
	{
		private TournamentBoard board;
		private TournamentMonsterList tournamentMonsterList;
		private bool isLoaded;

		public TournamentBoard Board => board;
		private TournamentMonsterList TournamentMonsterList => tournamentMonsterList;

		public override IEnumerator Initialize()
		{
			yield return base.Initialize();
			Load(() => 
			{
				isLoaded = true;
			});
			yield return new WaitUntil(() => isLoaded);
		}

		public void OnBeforeMoveIn(TournamentMonsterParam[] monsterParams)
		{
			board.Initialize(monsterParams.Length);
			tournamentMonsterList.Initialize(monsterParams.Select(x => x.MonsterName).ToArray());
		}

		protected override void OnBack()
		{
			base.OnBack();
		}

		private async void Load(Action action = null)
		{
			// ボード
			var result = await AddressableManager.Instance.Load("Assets/Projects/AddressableAssets/Prefabs/Tournament/TournamentBoard.prefab");
			board = Instantiate(result, CanvasManager.Instance.FrontCanvas.transform).GetComponent<TournamentBoard>();
			// リスト
			result = await AddressableManager.Instance.Load("Assets/Projects/AddressableAssets/Prefabs/Tournament/TournamentMonsterList.prefab");
			tournamentMonsterList = Instantiate(result, CanvasManager.Instance.FrontCanvas.transform).GetComponent<TournamentMonsterList>();
			action?.Invoke();
		}
	}
}
