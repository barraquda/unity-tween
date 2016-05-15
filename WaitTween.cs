using UnityEngine;
using System;

namespace Barracuda.UISystem
{
	public class WaitTween : TweenBase
	{
		[SerializeField] float duration;

		public override void Revert()
		{
			/* Nothing to do */
		}

		public override IStreamee<Unit> Streamee {
			get {
				return Barracuda.Streamee.Wait(TimeSpan.FromSeconds(duration));
			}
		}
	}
}