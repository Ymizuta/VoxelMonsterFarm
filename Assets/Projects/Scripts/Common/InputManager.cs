using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Voxel
{
	public class InputManager : SingletonMonoBehaviour<InputManager>
	{
		private Subject<Unit> onSpaceKeyDown = new Subject<Unit>();
		private Subject<Unit> onBKeyDown = new Subject<Unit>();
		private Subject<Unit> onUpKeyDown = new Subject<Unit>();
		private Subject<Unit> onDownKeyDown = new Subject<Unit>();
		private Subject<Unit> onLeftKeyDown = new Subject<Unit>();
		private Subject<Unit> onRightKeyDown = new Subject<Unit>();

		public IObservable<Unit> OnSpaceKeyDownAsObservable => onSpaceKeyDown;
		public IObservable<Unit> OnBKeyDownAsObservable => onBKeyDown;
		public IObservable<Unit> OnUpKeyDownAsObservable => onUpKeyDown;
		public IObservable<Unit> OnDownKeyDownAsObservable => onDownKeyDown;
		public IObservable<Unit> OnLeftKeyDownAsObservable => onLeftKeyDown;
		public IObservable<Unit> OnRightKeyDownAsObservable => onRightKeyDown;

		protected override void Awake()
		{
			base.Awake();
			Bind();
		}

		private void Bind()
		{
			this.UpdateAsObservable()
				.Where(_ => Input.GetKeyDown(KeyCode.Space))
				.Subscribe(_ => onSpaceKeyDown.OnNext(Unit.Default)).AddTo(this);

			this.UpdateAsObservable()
				.Where(_ => Input.GetKeyDown(KeyCode.B))
				.Subscribe(_ => onBKeyDown.OnNext(Unit.Default)).AddTo(this);

			this.UpdateAsObservable()
				.Where(_ => Input.GetKeyDown(KeyCode.UpArrow))
				.Subscribe(_ => onUpKeyDown.OnNext(Unit.Default)).AddTo(this);

			this.UpdateAsObservable()
				.Where(_ => Input.GetKeyDown(KeyCode.DownArrow))
				.Subscribe(_ => onDownKeyDown.OnNext(Unit.Default)).AddTo(this);

			this.UpdateAsObservable()
				.Where(_ => Input.GetKeyDown(KeyCode.LeftArrow))
				.Subscribe(_ => onLeftKeyDown.OnNext(Unit.Default)).AddTo(this);

			this.UpdateAsObservable()
				.Where(_ => Input.GetKeyDown(KeyCode.RightArrow))
				.Subscribe(_ => onRightKeyDown.OnNext(Unit.Default)).AddTo(this);
		}
	}
}
