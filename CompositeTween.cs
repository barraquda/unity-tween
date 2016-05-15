using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Barracuda.UISystem
{
	public class CompositeTween : TweenBase, ICollectionTween
	{
		[SerializeField] TweenBase[] tweens;
		public TweenBase[] Tweens {
			get { return tweens; }
			set { tweens = value; }
		}

		public override IStreamee<Unit> Streamee {
			get {
				return Merge().ToStreamee();
			}
		}

		public override void Revert()
		{
			foreach (var tween in tweens) {
				if (tween != null) {
					tween.Revert();
				}
			}
		}

		IEnumerable<IStreamee<Unit>> Merge()
		{
			var tweenEnumerators = new List<IEnumerator<IStreamee<Unit>>>();
			foreach (var tween in tweens) {
				tweenEnumerators.Add(tween.Streamee.GetEnumerator());
			}

			bool allFinish = false;
			while (!allFinish) {
				foreach (var enumerator in tweenEnumerators) {
					allFinish &= !enumerator.MoveNext();
				}
				yield return Barracuda.Streamee.UnitEmpty;
			}

			foreach (var e in tweenEnumerators) {
				e.Dispose();
			}
		}
	}
}