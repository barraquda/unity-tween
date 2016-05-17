using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Barracuda.UISystem
{
	public class CompositeTween : CollectionTweenBase
	{
		protected override IEnumerable<Unit> TweenStreamee {
			get { return Merge(); }
		}

		IEnumerable<Unit> Merge()
		{
			var tweenEnumerators = new List<IEnumerator<Unit>>();
			foreach (var tween in Tweens) {
				tweenEnumerators.Add(tween.GetTweenStreamee().GetEnumerator());
			}

			while (true) {
				bool allFinish = true;
				foreach (var enumerator in tweenEnumerators) {
					allFinish &= !enumerator.MoveNext();
				}
				if (allFinish) {
					yield return Unit.Default;
					break;
				} else {
					yield return null;
				}
			}

			foreach (var e in tweenEnumerators) {
				e.Dispose();
			}
		}
	}
}