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

		protected override IStreamee<Unit> TweenStreamee {
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
				tweenEnumerators.Add(tween.GetTweenStreamee().GetEnumerator());
			}

			while (true) {
				bool allFinish = true;
				foreach (var enumerator in tweenEnumerators) {
					allFinish &= !enumerator.MoveNext();
				}
				if (allFinish) {
					yield return Barracuda.Streamee.UnitEmpty;
					break;
				} else {
					yield return Barracuda.Streamee.None<Unit>();
				}
			}

			foreach (var e in tweenEnumerators) {
				e.Dispose();
			}
		}
	}
}