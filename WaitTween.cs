using UnityEngine;
using System;
using System.Collections.Generic;

namespace Barracuda.UISystem
{
	public class WaitTween : TweenBase
	{
		[SerializeField] float duration;
		public float Duration { get { return duration; } }

		public override void Revert()
		{
			/* Nothing to do */
		}

		protected override IEnumerable<Unit> TweenStreamee {
			get {
				// TODO 依存関係
				foreach (var _ in EnumerableAction.Await(duration)) {
					yield return Unit.Default;
				}
			}
		}
	}
}