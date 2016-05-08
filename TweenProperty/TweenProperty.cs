using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Barracuda;
using UnityEngine.UI;

namespace Barracuda.UISystem
{
	/// <summary>
	/// Tween Target
	/// </summary>
	public abstract class TweenProperty
	{
		public TweenKey Key { get; private set; }

		public float Value { get; private set; }

		protected TweenProperty(TweenKey key, float val)
		{
			this.Key = key;
			this.Value = val;
		}

		/// <summary>
		/// Get Action that influence target UI
		/// </summary>
		/// <returns>The tweener.</returns>
		/// <param name="ui">User interface.</param>
		public abstract Action<float> GetTweener(Graphic ui);

		private Action<float> empty;
		protected Action<float> Empty {
			get { return empty = empty ?? ((degree) => {}); }
		}
	}
	
}