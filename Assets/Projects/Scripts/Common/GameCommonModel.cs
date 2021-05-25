using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Voxel
{
	public class GameCommonModel : SingletonMonoBehaviour<GameCommonModel>
	{
		public ReactiveProperty<int> Year { get; set; } = new ReactiveProperty<int>();
		public ReactiveProperty<int> Month { get; set; } = new ReactiveProperty<int>();
		public ReactiveProperty<int> Week { get; set; } = new ReactiveProperty<int>();

		protected override void Awake()
		{
			base.Awake();
			Year.Value = 1;
			Month.Value = 4;
			Week.Value = 1;
		}
	}
}
