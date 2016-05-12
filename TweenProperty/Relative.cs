using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

using Barracuda;

namespace Barracuda.UISystem
{

	public class Relative : TweenProperty
	{
		public Relative(TweenKey key, float val) : base(key, val)
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
			case TweenKey.X:
				{
					var prev = rectTransform.anchoredPosition.x;
					var diff = Value;
					return degree => {
						rectTransform.anchoredPosition = new Vector2(prev + diff * degree, rectTransform.anchoredPosition.y);
					};
				}
			case TweenKey.Y:
				{
					var prev = rectTransform.anchoredPosition.y;
					var diff = Value;
					return degree => {
						rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, prev + diff * degree);
					};
				}
			case TweenKey.RotationZ:
				{
					var r = Value;
					var prevRotation = rectTransform.localRotation.eulerAngles;
					return degree => {
						rectTransform.localRotation = Quaternion.Euler(new Vector3(
							rectTransform.localRotation.x,
							rectTransform.localRotation.y,
							prevRotation.z + r * degree));
					};
				}
			}

			Debug.LogWarningFormat("PropKey `{0}` to `Relative` is not catched", Key);
			return Empty;
		}
	}
	
}