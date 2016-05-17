using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Barracuda.UISystem
{
	public class SerialTween : CollectionTweenBase
	{
		protected override IStreamee<Unit> TweenStreamee {
			get {
				return Concat().ToStreamee();
			}
		}

		IEnumerable<IStreamee<Unit>> Concat()
		{
			var i = 0;
			foreach (var tween in Tweens) {
				using (var enumerator = tween.GetTweenStreamee().GetEnumerator()) {
					while (enumerator.MoveNext()) {
						yield return enumerator.Current;
					}
				}
			}
		}
	}
}