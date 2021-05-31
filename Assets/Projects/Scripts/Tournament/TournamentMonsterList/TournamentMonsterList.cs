using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel.Tournament
{
	public class TournamentMonsterList : MonoBehaviour
	{
		[SerializeField] GameObject itemPrefab;
		[SerializeField] Transform itemRoot;
		private TournamentMonsterListItem[] items; 

		public void Initialize(string[] names)
		{
			Create(names);
		}

		public void Create(string[] names)
		{
			items = new TournamentMonsterListItem[name.Length];
			for (int i = 0; i < names.Length; i++)
			{
				var item = Instantiate(itemPrefab, itemRoot).GetComponent<TournamentMonsterListItem>();
				item.SetData(i+1, names[i], TournamentCommonModel.Instance.MonsterParams[i].MonsterModelId);
				items[i] = item;
 			}
		}
	}
}
