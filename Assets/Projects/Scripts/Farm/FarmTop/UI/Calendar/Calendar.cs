using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Voxel.Farm
{
	public class Calendar : SingletonMonoBehaviour<Calendar>
	{
		[SerializeField] private Text yearText;
		[SerializeField] private Text monthWeekText;

		public void Initialize()
		{
			this.yearText.text = $"{GameCommonModel.Instance.Year.Value}年";
			this.monthWeekText.text = $"{GameCommonModel.Instance.Month.Value}月　{GameCommonModel.Instance.Week.Value}週";
			Bind();
		}

		private void Bind()
		{
			GameCommonModel.Instance.Year
				.Subscribe(year => 
				{
					this.yearText.text = $"{year}年";
				}).AddTo(this);
			GameCommonModel.Instance.Week
				.Subscribe(week => 
				{
					this.monthWeekText.text = $"{GameCommonModel.Instance.Month.Value}月　{week}週";
				}).AddTo(this);
		}
	}
}
