using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Barracuda;
using UnityEngine.UI;

namespace Barracuda.UISystem
{

	public abstract class TweenProperty
	{
		public PropKey Key { get; private set; }

		public float Value { get; private set; }

		protected TweenProperty(PropKey key, float val)
		{
			this.Key = key;
			this.Value = val;
		}

		public abstract Action<float> GetTweener(Graphic ui);

		private Action<float> empty;
		protected Action<float> Empty {
			get { return empty = empty ?? ((degree) => {}); }
		}
	}
	
}