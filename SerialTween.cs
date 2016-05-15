using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Barracuda.UISystem
{
	public class SerialTween : TweenBase, ICollectionTween
	{
		[SerializeField] TweenBase[] tweens;
		public TweenBase[] Tweens {
			get { return tweens; }
			set { tweens = value; }
		}

		protected override IStreamee<Unit> TweenStreamee {
			get {
				return Concat().ToStreamee();
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

		IEnumerable<IStreamee<Unit>> Concat()
		{
			var i = 0;
			foreach (var tween in tweens) {
				using (var enumerator = tween.GetTweenStreamee().GetEnumerator()) {
					while (enumerator.MoveNext()) {
						yield return enumerator.Current;
					}
				}
			}
		}
	}
}