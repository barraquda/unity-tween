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
		public abstract Action<float> GetTweener(GameObject gameObject);

		private Action<float> empty;

		protected Action<float> Empty {
			get { return empty = empty ?? ((degree) => {
				}); }
		}

		public float GetCurrentValue(GameObject gameObject)
		{
			var rectTransform = gameObject.GetComponent<RectTransform>();
			if (rectTransform == null) {
				Debug.LogWarningFormat("{0} is not UI object", gameObject.name);
				return 0;
			}
			switch (Key) {
			case TweenKey.Width:
				return rectTransform.sizeDelta.x;
			case TweenKey.Height:
				return rectTransform.sizeDelta.y;
			case TweenKey.X:
				return rectTransform.anchoredPosition.x;
			case TweenKey.Y:
				return rectTransform.anchoredPosition.y;
			case TweenKey.GlobalX:
				return rectTransform.position.x;
			case TweenKey.GlobalY:
				return rectTransform.position.y;
			case TweenKey.RotationX:
				return rectTransform.localRotation.eulerAngles.x;
			case TweenKey.RotationY:
				return rectTransform.localRotation.eulerAngles.y;
			case TweenKey.RotationZ:
				return rectTransform.localRotation.eulerAngles.z;
			case TweenKey.Scale:
				return rectTransform.localScale.x;
			case TweenKey.ScaleX:
				return rectTransform.localScale.x;
			case TweenKey.ScaleY:
				return rectTransform.localScale.y;
			case TweenKey.Brightness:
				{
					var ui = gameObject.GetComponent<Graphic>();
					return ui.color.grayscale;
				}
			case TweenKey.Opacity:
				{
					var ui = gameObject.GetComponent<Graphic>();
					return ui.color.a;
				}
			case TweenKey.CanvasGroupAlpha:
				{
					var cg = gameObject.GetComponent<CanvasGroup>();
					if (cg == null) {
						Debug.LogWarningFormat("`{0}` is not attached CanvasGroup component", gameObject);
						return 0f;
					}
					return cg.alpha;
				}
			case TweenKey.Alpha:
				{
					var cr = gameObject.GetComponent<CanvasRenderer>();
					if (cr == null) {
						Debug.LogWarningFormat("`{0}` is not attached CanvasRenderer component", gameObject);
						return 0f;
					}
					return cr.GetAlpha();
				}
			}
			Debug.LogWarningFormat("PropKey `{0}` to `Absolute` is not catched", Key);
			return 0;
		}
	}
}