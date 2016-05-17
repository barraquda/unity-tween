using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

using Barracuda.Internal;
using Barracuda.UISystem;

namespace Barracuda
{
	public class Tweener : MonoBehaviour, IDisposable
	{
		bool stop;
		public IEnumerator<Unit> Streamer { get; set; }

		[SerializeField] TweenBase streamee;

		void Update()
		{
			if (Streamer != null && !stop) {
				Streamer.MoveNext();
			}
		}

		public void Stop()
		{
			stop = true;
			if (streamee != null) {
				streamee.Revert();
			}
		}

		public void Begin()
		{
			stop = false;
			if (Streamer != null) {
				Streamer.Dispose();
			}
			Streamer = streamee.GetTweenStreamee().GetEnumerator();
		}

		public void Dispose()
		{
			Streamer.Dispose();
			Destroy(gameObject);
		}
	}
}