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
			/*
			switch (prop.Key) {
			case PropKey.X:
				{
					var prev = ui.anchoredPosition.x;
					var diff = prop.Value;
					return degree => {
						ui.anchoredPosition = new Vector2(prev + diff * degree, ui.anchoredPosition.y);
					};
				}
			case PropKey.Y:
				{
					var prev = ui.anchoredPosition.y;
					var diff = prop.Value;
					return degree => {
						ui.anchoredPosition = new Vector2(ui.anchoredPosition.x, prev + diff * degree);
					};
				}
			case PropKey.RotationZ:
				{
					var r = prop.Value;
					var prevRotation = ui.localRotation.eulerAngles;
					return degree => {
						ui.localRotation = Quaternion.Euler(new Vector3(
							ui.localRotation.x,
							ui.localRotation.y,
							prevRotation.z + r * degree));
					};
				}
			}
			*/

			Debug.LogWarningFormat("PropKey `{0}` to `Relative` is not catched", Key);
			return Empty;
		}
	}
	
}