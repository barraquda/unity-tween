using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

using Barracuda;

namespace Barracuda.UISystem
{

	public class Absolute : TweenProperty
	{
		public Absolute(PropKey key, float val) : base(key, val)
		{
		}

		public override Action<float> GetTweener(Graphic ui)
		{
			switch (Key) {
			case PropKey.Width:
				{
					var prevWidth = ui.rectTransform.sizeDelta.x;
					var diff = Value - prevWidth;
					return degree => {
						ui.rectTransform.sizeDelta = new Vector2(prevWidth + diff * degree, ui.rectTransform.sizeDelta.y);
					};
				}
			case PropKey.X:
				{
					var prevX = ui.rectTransform.anchoredPosition.x;
					var diffX = Value - prevX;
					return degree => {
						ui.rectTransform.anchoredPosition = new Vector2(prevX + diffX * degree, ui.rectTransform.anchoredPosition.y);
					};
				}
			case PropKey.Y:
				{
					var prevY = ui.rectTransform.anchoredPosition.y;
					var diffY = Value - prevY;
					return degree => {
						ui.rectTransform.anchoredPosition = new Vector2(ui.rectTransform.anchoredPosition.x, prevY + diffY * degree);
					};
				}
			case PropKey.GlobalX:
				{
					var prevX = ui.rectTransform.position.x;
					var diffX = Value - prevX;
					return degree => {
						ui.rectTransform.position = new Vector2(prevX + diffX * degree, ui.rectTransform.position.y);
					};
				}
			case PropKey.GlobalY:
				{
					var prevY = ui.rectTransform.position.y;
					var diffY = Value - prevY;
					return degree => {
						ui.rectTransform.position = new Vector2(ui.rectTransform.position.x, prevY + diffY * degree);
					};
				}
			case PropKey.RotationZ:
				{
					var rotationZ = Value;
					var prevRotation = ui.rectTransform.localRotation.eulerAngles;
					var diff = (rotationZ - prevRotation.z) % 360;
					return degree => {
						var v = new Vector3(prevRotation.x, prevRotation.y, prevRotation.z + diff * degree);
						ui.rectTransform.localRotation = Quaternion.Euler(v);
					};
				}
			case PropKey.RotationY:
				{
					var rotationY = Value;
					var prevRotation = ui.rectTransform.localRotation.eulerAngles;
					var diff = (rotationY - prevRotation.y) % 360;
					return degree => {
						var v = new Vector3(prevRotation.x, prevRotation.y + diff * degree, prevRotation.y);
						ui.rectTransform.localRotation = Quaternion.Euler(v);
					};
				}
			case PropKey.RotationX:
				{
					var rotationX = Value;
					var prevRotation = ui.rectTransform.localRotation.eulerAngles;
					var diff = (rotationX - prevRotation.x) % 360;
					return degree => {
						var v = new Vector3(prevRotation.x + diff * degree, prevRotation.y, prevRotation.z);
						ui.rectTransform.localRotation = Quaternion.Euler(v);
					};
				}
			case PropKey.Scale:
				{
					var prevScale = ui.rectTransform.localScale;
					var diffScale = new Vector3(1, 1, 1) * Value - prevScale;
					return degree => {
						ui.rectTransform.localScale = prevScale + diffScale * degree;
					};
				}
			case PropKey.ScaleX:
				{
					var prevScaleX = ui.rectTransform.localScale.x;
					var diffScaleX = Value - prevScaleX;
					return degree => {
						ui.rectTransform.localScale = new Vector3(prevScaleX + diffScaleX * degree, ui.rectTransform.localScale.y, ui.rectTransform.localScale.z);
					};
				}
			case PropKey.ScaleY:
				{
					var prevScaleY = ui.rectTransform.localScale.y;
					var diffScaleY = Value - prevScaleY;
					return degree => {
						ui.rectTransform.localScale = new Vector3(ui.rectTransform.localScale.x, prevScaleY + diffScaleY * degree, ui.rectTransform.localScale.z);
					};
				}
			case PropKey.Brightness:
				{
					var bright = ui.color.grayscale;
					var diff = Value - bright;
					return degree => {
						var d = bright + diff * degree;
						ui.color = new Color(d, d, d, ui.color.a);
					};
				}
			case PropKey.Opacity:
				{
					var prevOpacity = ui.color.a;
					var diff = Value - prevOpacity;
					return degree => ui.color = new Color(ui.color.r, ui.color.g, ui.color.b, diff * degree);
				}
			case PropKey.CanvasGroupAlpha:
				{
					var cg = ui.GetComponent<CanvasGroup>();
					if (cg == null) {
						Debug.LogWarningFormat("`{0}` is not attached CanvasGroup component", ui);
						return Empty;
					}
					var prev = cg.alpha;
					var diff = Value - prev;
					return degree => cg.alpha = prev + diff * degree;
				}
			case PropKey.Alpha:
				{
					var cr = ui.GetComponent<CanvasRenderer>();
					var prev = cr.GetAlpha();
					var diff = Value - prev;
					return degree => cr.SetAlpha(prev + diff * degree);
				}
			}
			Debug.LogWarningFormat("PropKey `{0}` to `Absolute` is not catched", Key);
			return Empty;
		}
	}
	
}