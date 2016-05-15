using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Barracuda.UISystem
{
	public abstract class TweenBase : MonoBehaviour
	{
		[SerializeField] bool loop;

		public bool Loop {
			get { return loop; }
			set { loop = value; }
		}

		protected abstract IStreamee<Unit> TweenStreamee { get; }

		public IStreamee<Unit> GetTweenStreamee()
		{
			if (loop) {
				return GetForeverEnumerable().ToStreamee();
			} else {
				return TweenStreamee;
			}
		}

		private IEnumerable<IStreamee<Unit>> GetForeverEnumerable()
		{
			while (true) {
				using (var enumerator = TweenStreamee.GetEnumerator()) {
					while (enumerator.MoveNext()) {
						yield return enumerator.Current;
					}
				}
			}
		}

		public abstract void Revert();
	}
}