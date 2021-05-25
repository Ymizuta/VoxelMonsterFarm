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
			this.yearText.text = $"{GameCommonModel.Instance.Year.Value}�N";
			this.monthWeekText.text = $"{GameCommonModel.Instance.Month.Value}���@{GameCommonModel.Instance.Week.Value}�T";
			Bind();
		}

		private void Bind()
		{
			GameCommonModel.Instance.Year
				.Subscribe(year => 
				{
					this.yearText.text = $"{year}�N";
				}).AddTo(this);
			GameCommonModel.Instance.Week
				.Subscribe(week => 
				{
					this.monthWeekText.text = $"{GameCommonModel.Instance.Month.Value}���@{week}�T";
				}).AddTo(this);
		}
	}
}
