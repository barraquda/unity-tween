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

		public override Action<float> GetTweener(Graphic ui)
		{
			switch (Key) {
			case TweenKey.X:
				{
					var prev = ui.rectTransform.anchoredPosition.x;
					var diff = Value;
					return degree => {
						ui.rectTransform.anchoredPosition = new Vector2(prev + diff * degree, ui.rectTransform.anchoredPosition.y);
					};
				}
			case TweenKey.Y:
				{
					var prev = ui.rectTransform.anchoredPosition.y;
					var diff = Value;
					return degree => {
						ui.rectTransform.anchoredPosition = new Vector2(ui.rectTransform.anchoredPosition.x, prev + diff * degree);
					};
				}
			case TweenKey.RotationZ:
				{
					var r = Value;
					var prevRotation = ui.rectTransform.localRotation.eulerAngles;
					return degree => {
						ui.rectTransform.localRotation = Quaternion.Euler(new Vector3(
							ui.rectTransform.localRotation.x,
							ui.rectTransform.localRotation.y,
							prevRotation.z + r * degree));
					};
				}
			}

			Debug.LogWarningFormat("PropKey `{0}` to `Relative` is not catched", Key);
			return Empty;
		}
	}
	
}