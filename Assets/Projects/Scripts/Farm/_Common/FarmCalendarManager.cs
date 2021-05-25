using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.UI;

namespace Voxel.Farm
{
	public class FarmCalendarManager : SingletonMonoBehaviour<FarmCalendarManager>
	{
		public void NextWeek()
		{
			FadeManager.Instance.PlayFadeOut(() => 
			{
				CalendarManager.Instance.NextWeek();
				FarmPresenterManager.Instance.ChangePresenter(FarmPresenterManager.FarmPresenterType.FarmTop);
			});
		}
	}
}
