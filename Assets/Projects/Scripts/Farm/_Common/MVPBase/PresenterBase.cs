using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Voxel
{
	public class PresenterBase : MonoBehaviour
	{
		protected ModelBase Model;
		protected ViewBase View;
		protected bool isInit;

		protected CompositeDisposable setEventsDisposable = new CompositeDisposable();

		public virtual IEnumerator Initialize()
		{
			this.Model = GetComponent<ModelBase>();
			this.View = GetComponent<ViewBase>();
			Model.Initialize();
			yield return View.Initialize();
		}

		public virtual IEnumerator Run(SceneManagement.SceneData data = null)
		{
			yield return Initialize();
		}

		public virtual void OnBack()
		{
		}

		public virtual void Pause()
		{
		}

		public virtual void Restart()
		{
		}
	}
}
