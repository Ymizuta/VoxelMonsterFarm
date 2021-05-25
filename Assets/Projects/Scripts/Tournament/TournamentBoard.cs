using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Tournament
{
	public enum ResultType
	{
		Win = 0,
		Lose,
		Self,
		None,
	}

	public class TournamentBoard : MonoBehaviour
	{
		private ResultType[][] results;
		private TournamentPanel[][] panels;

		[SerializeField] private GameObject panelPrefab;
		[SerializeField] private GameObject headerPanelPrefab;
		[SerializeField] private Transform panelRoot;

		private void Awake()
		{
			Initialize();
		}

		public void Initialize()
		{
			var length = 8;

			//SetBackBoard(length);

			results = new ResultType[length][];
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = new ResultType[results.Length];
				for (int j = 0; j < length; j++) results[i][j] = ResultType.None;
				// 自分の枠にはスラッシュを入れる
				results[i][i] = ResultType.Self;
			}
			// オブジェクトを保持する配列の初期化
			panels = new TournamentPanel[length][];
			for (int i = 0; i < length; i++) panels[i] = new TournamentPanel[length];

			Create();
		}

		public void SetWin(int winnerIdx, int loserIdx)
		{
			results[winnerIdx][loserIdx] = ResultType.Win;
			results[loserIdx][winnerIdx] = ResultType.Lose;

			for (int i = 0; i < panels.Length; i++)
			{
				for (int j = 0; j < panels[0].Length; j++)
				{
					panels[i][j].SetData(results[i][j]);
				}
			}
		}

		/// <summary>
		/// パネルの親クラスのサイズをパネル数に合わせて拡張
		/// </summary>
		/// <param name="monsterCount"></param>
		private void SetBackBoard(int monsterCount)
		{
			var panelRtf = panelPrefab.GetComponent<RectTransform>();
			panelRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(panelRtf.sizeDelta.x * monsterCount, panelRtf.sizeDelta.y * monsterCount);
		}

		private void Create()
		{
			var dist = panelPrefab.GetComponent<TournamentPanel>().Rtf.sizeDelta.y;
			var yPos = dist * ((float)results.Length / 2.0f - 0.5f );
			for (int rowIdx = 0; rowIdx < results.Length; rowIdx++)
			{
				var xPos = dist * (results.Length / 2.0f - 0.5f) * -1.0f;
				for (int columnIdx = 0; columnIdx < results[rowIdx].Length; columnIdx++)
				{
					var panel = Instantiate(panelPrefab, panelRoot).GetComponent<TournamentPanel>();
					panel.Rtf.localPosition = new Vector3(xPos, yPos);
					panel.SetData(results[rowIdx][columnIdx]);
					panels[rowIdx][columnIdx] = panel;
					xPos += dist;
				}
				yPos -= dist;
			}
			// ヘッダー
			for (int i = 0; i < panels[0].Length; i++)
			{
				var headerPanel = Instantiate(headerPanelPrefab, panelRoot);
				var pos = panels[0][i].GetComponent<RectTransform>().position;
				pos.y += panels[0][i].GetComponent<RectTransform>().sizeDelta.y;
				headerPanel.GetComponent<RectTransform>().position = pos;
				headerPanel.GetComponentInChildren<Text>().text = (i + 1).ToString();
			}
		}
	}
}
