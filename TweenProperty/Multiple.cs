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

		public override Action<float> GetTweener(Graphic ui)
		{
			switch (Key) {
			case TweenKey.Scale:
				{
					var prevScale = ui.transform.localScale;
					var diffScale = Value * ui.transform.localScale - prevScale;
					return degree => {
						ui.transform.localScale = prevScale + diffScale * degree;
					};
				}
			case TweenKey.ScaleX:
				{
					var prevScaleX = ui.transform.localScale.x;
					var diffScaleX = Value * ui.transform.localScale.x - prevScaleX;
					return degree => {
						ui.transform.localScale = new Vector2(prevScaleX + diffScaleX * degree, ui.transform.localScale.y);
					};
				}
			case TweenKey.ScaleY:
				{
					var prevScaleY = ui.transform.localScale.y;
					var diffScaleY = Value * ui.transform.localScale.y - prevScaleY;
					return degree => {
						ui.transform.localScale = new Vector2(ui.transform.localScale.x, prevScaleY + diffScaleY * degree);
					};
				}
			}

			Debug.LogWarningFormat("PropKey `{0}` to `Multiple` is not catched", Key);
			return Empty;
		}
	}
	
}