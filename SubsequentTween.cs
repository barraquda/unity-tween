using UnityEngine;
using System.Collections.Generic;

namespace Barracuda.UISystem
{
	public class SubsequentTween : CollectionTweenBase
	{
		[SerializeField] float interval;
		public float Interval {
			get { return interval; }
			set { interval = value; }
		}

		protected override IEnumerable<Unit> TweenStreamee {
			get { return Subsequent(); }
		}

		IEnumerable<Unit> Subsequent()
		{
			var offsets = new List<float>();
			var sum = 0.0f;

			for (var i = 0; i < Tweens.Length; i++) {
				offsets.Add(sum);
				sum += interval;
			}

			var tweenEnumerators = new List<IEnumerator<Unit>>();
			foreach (var tween in Tweens) {
				tweenEnumerators.Add(tween.GetTweenStreamee().GetEnumerator());
			}

			while (true) {
				bool allFinish = true;
				for (var i = 0; i < Tweens.Length; i++) {
					var enumerator = tweenEnumerators[i];
					if (offsets[i] > 0) {
						offsets[i] -= Time.deltaTime;
						allFinish = false;
					} else {
						allFinish &= !enumerator.MoveNext();
					}
				}
				if (allFinish) {
					yield return Unit.Default;
					break;
				} else {
					yield return null;
				}
			}
		}
	}
}