using UnityEngine;
using System;

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

		protected override IStreamee<Unit> TweenStreamee {
			get {
				return Barracuda.Streamee.Wait(TimeSpan.FromSeconds(duration));
			}
		}
	}
}