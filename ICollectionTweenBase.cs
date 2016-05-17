using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Barracuda.UISystem
{
	abstract public class CollectionTweenBase : TweenBase
	{
		[SerializeField] TweenBase[] tweens;
		public TweenBase[] Tweens {
			get { return tweens; }
			set { tweens = value; }
		}

		public override void Revert()
		{
			foreach (var tween in tweens) {
				if (tween != null) {
					tween.Revert();
				}
			}
		}
	}
}