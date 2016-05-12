using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

using Barracuda;

namespace Barracuda.UISystem
{

	public class Multiple : TweenProperty
	{
		public Multiple(TweenKey key, float val) : base(key, val)
		{
		}

		public override Action<float> GetTweener(GameObject gameObject)
		{
			var rectTransform = gameObject.GetComponent<RectTransform>();
			if (rectTransform == null) {
				Debug.LogWarningFormat("{0} is not UI object", gameObject.name);
				return Empty;
			}
			switch (Key) {
			case TweenKey.Scale:
				{
					var prevScale = rectTransform.localScale;
					var diffScale = Value * rectTransform.localScale - prevScale;
					return degree => {
						rectTransform.localScale = prevScale + diffScale * degree;
					};
				}
			case TweenKey.ScaleX:
				{
					var prevScaleX = rectTransform.localScale.x;
					var diffScaleX = Value * rectTransform.localScale.x - prevScaleX;
					return degree => {
						rectTransform.localScale = new Vector2(prevScaleX + diffScaleX * degree, rectTransform.localScale.y);
					};
				}
			case TweenKey.ScaleY:
				{
					var prevScaleY = rectTransform.localScale.y;
					var diffScaleY = Value * rectTransform.localScale.y - prevScaleY;
					return degree => {
						rectTransform.localScale = new Vector2(rectTransform.localScale.x, prevScaleY + diffScaleY * degree);
					};
				}
			}

			Debug.LogWarningFormat("PropKey `{0}` to `Multiple` is not catched", Key);
			return Empty;
		}
	}
	
}