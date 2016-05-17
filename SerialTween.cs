using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Barracuda.UISystem
{
	public class SerialTween : CollectionTweenBase
	{
		protected override IEnumerable<Unit> TweenStreamee {
			get { return Concat(); }
		}

		IEnumerable<Unit> Concat()
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