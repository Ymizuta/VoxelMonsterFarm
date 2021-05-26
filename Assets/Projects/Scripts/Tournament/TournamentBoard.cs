using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voxel.Tournament
{
	public class TournamentBoard : MonoBehaviour
	{
		[SerializeField] private GameObject panelPrefab;
		[SerializeField] private GameObject headerPanelPrefab;
		[SerializeField] private Transform panelRoot;

		private ResultType[][] results;
		private TournamentPanel[][] panels;

		public void Initialize(int monsterCount, ResultType[][] results)
		{
			SetBackBoard(monsterCount);

			this.results = results;
			// オブジェクトを保持する配列の初期化
			panels = new TournamentPanel[monsterCount][];
			for (int i = 0; i < monsterCount; i++) panels[i] = new TournamentPanel[monsterCount];

			Create(monsterCount);
		}

		/// <summary>
		/// 勝敗情報を受け取ってトーナメント表を更新
		/// </summary>
		/// <param name="winnerIdx"></param>
		/// <param name="loserIdx"></param>
		public void UpdateBoard()
		{
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
			var boardRtf = panelRoot.GetComponent<RectTransform>();
			boardRtf.sizeDelta = new Vector2(panelRtf.sizeDelta.x * monsterCount, panelRtf.sizeDelta.y * monsterCount);
		}

		private void Create(int mosterCount)
		{
			// ヘッダー
			var headerPanelSize = headerPanelPrefab.GetComponent<RectTransform>().sizeDelta.x;
			for (int i = 0; i < mosterCount; i++)
			{
				var headerPanel = Instantiate(headerPanelPrefab, panelRoot);
				headerPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(10f + (headerPanelSize * i) + ( i * 20f), 0f);
				headerPanel.GetComponentInChildren<Text>().text = (i + 1).ToString();
			}

			var dist = panelPrefab.GetComponent<TournamentPanel>().Rtf.sizeDelta.y;
			var yPos = -1f * dist;
			for (int rowIdx = 0; rowIdx < results.Length; rowIdx++)
			{
				var xPos = 0f;
				for (int columnIdx = 0; columnIdx < results[rowIdx].Length; columnIdx++)
				{
					var panel = Instantiate(panelPrefab, panelRoot).GetComponent<TournamentPanel>();
					panel.Rtf.anchoredPosition = new Vector3(xPos, yPos);
					panel.SetData(results[rowIdx][columnIdx]);
					panels[rowIdx][columnIdx] = panel;
					xPos += dist;
				}
				yPos -= dist;
			}
		}
	}
}
