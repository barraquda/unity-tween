using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Barracuda;
using UnityEngine.UI;

namespace Barracuda.UISystem
{

	public class Multiple : TweenProperty
	{
		public Multiple(PropKey key, float val) : base(key, val)
		{
		}

		public override Action<float> GetTweener(Graphic ui)
		{
			/*
			switch (prop.Key) {
			case PropKey.Scale:
				{
					var prevScale = ui.localScale;
					var diffScale = prop.Value * ui.localScale - prevScale;
					return degree => {
						ui.localScale = prevScale + diffScale * degree;
					};
				}
			case PropKey.ScaleX:
				{
					var prevScaleX = ui.localScale.x;
					var diffScaleX = prop.Value * ui.localScale.x - prevScaleX;
					return degree => {
						ui.localScale = new Vector2(prevScaleX + diffScaleX * degree, ui.localScale.y);
					};
				}
			case PropKey.ScaleY:
				{
					var prevScaleY = ui.localScale.y;
					var diffScaleY = prop.Value * ui.localScale.y - prevScaleY;
					return degree => {
						ui.localScale = new Vector2(ui.localScale.x, prevScaleY + diffScaleY * degree);
					};
				}
			}
			*/
			Debug.LogWarningFormat("PropKey `{0}` to `Multiple` is not catched", Key);
			return Empty;
		}
	}
	
}