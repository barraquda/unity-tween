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
		public static void Fix(this GameObject ui, params TweenProperty[] parameters)
		{
			foreach (TweenProperty prop in parameters) {
				prop.GetTweener(ui)(1.0f);
			}
		}

		public static IEnumerable<Unit> Animate(this GameObject ui, TweenProperty[] properties, float duration, EasingMode easingMode = null)
		{
			if (ui == null) {
				throw new NullReferenceException("Null is invalid");
			}
			if (easingMode == null) {
				easingMode = Easing.EaseOut;
			}
			return AnimateEnumerable(ui, properties, duration, easingMode);
		}

		public static IEnumerable<Unit> Animate(this GameObject ui, TweenProperty property, float duration, EasingMode easingMode = null)
		{
			return Animate(ui, new TweenProperty[] { property }, duration, easingMode);
		}

		private static IEnumerable<Unit> AnimateEnumerable(GameObject ui, TweenProperty[] properties, float duration, EasingMode easingMode)
		{
			var tweeners = new Action<float>[properties.Length];

			for (var i = 0; i < tweeners.Length; i++) {
				tweeners[i] = properties[i].GetTweener(ui);
			}

			float elapsedTime = 0;
			while ((elapsedTime += Time.deltaTime) < duration) {
				var degree = easingMode.Invoke(elapsedTime, elapsedTime, 0, 1, duration);
				foreach (var tweener in tweeners) {
					tweener.Invoke(degree);
				}
				yield return null;
			}
			var lastDegree = easingMode.Invoke(duration, duration, 0, 1, duration);
			foreach (var tweener in tweeners) {
				tweener.Invoke(lastDegree);
				yield return Unit.Default;
			}
		}
	}
}