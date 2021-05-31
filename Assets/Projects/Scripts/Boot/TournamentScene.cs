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
			// �V�[���Ԃ�����肷�郂�f����p�ӁE������
			var tData = data as TournamentSceneData;
			if (tData.IsInitialize)
			{
				var commonModel = new GameObject("TournamentCommonModel").AddComponent<TournamentCommonModel>();
				commonModel.Initialize(tData.TournamentId);
				DontDestroyOnLoad(commonModel.gameObject);
			}
			StartCoroutine(tournamentPresenter.Run(data));
		}
	}
}
