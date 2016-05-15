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

		public override IStreamee<Unit> Streamee {
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
			foreach (var tween in tweens) {
				using (var enumerator = tween.Streamee.GetEnumerator()) {
					while (enumerator.MoveNext()) {
						yield return enumerator.Current;
					}
				}
			}
		}
	}
}