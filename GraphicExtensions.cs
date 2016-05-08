using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Barracuda;
using UnityEngine.UI;

namespace Barracuda.UISystem
{
	/// <summary>
	/// UI Extensions
	/// </summary>
	public static class GraphicExtensions
	{
		public static void Fix(this Graphic ui, params TweenProperty[] parameters)
		{
			foreach (TweenProperty prop in parameters) {
				prop.GetTweener(ui)(1.0f);
			}
		}

		public static IStreamee<Unit> Animate(this Graphic ui, TweenProperty[] properties, float duration, EasingMode easingMode = null)
		{
			if (ui == null) {
				throw new NullReferenceException("Null is invalid");
			}
			if (easingMode == null) {
				easingMode = Easing.EaseOut;
			}
			return Streamee.Branch(AnimateEnumerable(ui, properties, duration, easingMode));
		}

		public static IStreamee<Unit> Animate(this Graphic ui, TweenProperty property, float duration, EasingMode easingMode = null)
		{
			return Animate(ui, new TweenProperty[] { property }, duration, easingMode);
		}

		private static IEnumerable<IStreamee<Unit>> AnimateEnumerable(Graphic ui, TweenProperty[] properties, float duration, EasingMode easingMode)
		{
			var startTime = Time.time;
			var tweeners = new Action<float>[properties.Length];

			for (var i = 0; i < tweeners.Length; i++) {
				tweeners[i] = properties[i].GetTweener(ui);
			}

			float elapsedTime = 0;
			while ((elapsedTime += Time.deltaTime) < duration) {
				var degree = elapsedTime / duration;
				foreach (var tweener in tweeners) {
					tweener.Invoke(degree);
					yield return Streamee.UnitEmpty;
				}
			}
			foreach (var tweener in tweeners) {
				tweener.Invoke(1);
				yield return Streamee.UnitEmpty;
			}
		}
	}
}