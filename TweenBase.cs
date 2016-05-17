using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Barracuda.UISystem
{
	public abstract class TweenBase : MonoBehaviour
	{
		[SerializeField] string description;
		public string Description {
			get { return description; }
			set { description = value; }
		}

		[SerializeField] bool loop;

		public bool Loop {
			get { return loop; }
			set { loop = value; }
		}

		protected abstract IEnumerable<Unit> TweenStreamee { get; }

		public IEnumerable<Unit> GetTweenStreamee()
		{
			return loop ? GetForeverEnumerable() : TweenStreamee;
		}

		private IEnumerable<Unit> GetForeverEnumerable()
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