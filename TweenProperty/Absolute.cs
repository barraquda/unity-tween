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
		public Absolute(TweenKey key, float val) : base(key, val)
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
			case TweenKey.Width:
				{
					var prevWidth = rectTransform.sizeDelta.x;
					var diff = Value - prevWidth;
					return degree => {
						rectTransform.sizeDelta = new Vector2(prevWidth + diff * degree, rectTransform.sizeDelta.y);
					};
				}
			case TweenKey.X:
				{
					var prevX = rectTransform.anchoredPosition.x;
					var diffX = Value - prevX;
					return degree => {
						rectTransform.anchoredPosition = new Vector2(prevX + diffX * degree, rectTransform.anchoredPosition.y);
					};
				}
			case TweenKey.Y:
				{
					var prevY = rectTransform.anchoredPosition.y;
					var diffY = Value - prevY;
					return degree => {
						rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, prevY + diffY * degree);
					};
				}
			case TweenKey.GlobalX:
				{
					var prevX = rectTransform.position.x;
					var diffX = Value - prevX;
					return degree => {
						rectTransform.position = new Vector2(prevX + diffX * degree, rectTransform.position.y);
					};
				}
			case TweenKey.GlobalY:
				{
					var prevY = rectTransform.position.y;
					var diffY = Value - prevY;
					return degree => {
						rectTransform.position = new Vector2(rectTransform.position.x, prevY + diffY * degree);
					};
				}
			case TweenKey.RotationZ:
				{
					var rotationZ = Value;
					var prevRotation = rectTransform.localRotation.eulerAngles;
					var diff = (rotationZ - prevRotation.z) % 360;
					return degree => {
						var v = new Vector3(prevRotation.x, prevRotation.y, prevRotation.z + diff * degree);
						rectTransform.localRotation = Quaternion.Euler(v);
					};
				}
			case TweenKey.RotationY:
				{
					var rotationY = Value;
					var prevRotation = rectTransform.localRotation.eulerAngles;
					var diff = (rotationY - prevRotation.y) % 360;
					return degree => {
						var v = new Vector3(prevRotation.x, prevRotation.y + diff * degree, prevRotation.y);
						rectTransform.localRotation = Quaternion.Euler(v);
					};
				}
			case TweenKey.RotationX:
				{
					var rotationX = Value;
					var prevRotation = rectTransform.localRotation.eulerAngles;
					var diff = (rotationX - prevRotation.x) % 360;
					return degree => {
						var v = new Vector3(prevRotation.x + diff * degree, prevRotation.y, prevRotation.z);
						rectTransform.localRotation = Quaternion.Euler(v);
					};
				}
			case TweenKey.Scale:
				{
					var prevScale = rectTransform.localScale;
					var diffScale = new Vector3(1, 1, 1) * Value - prevScale;
					return degree => rectTransform.localScale = prevScale + diffScale * degree;
				}
			case TweenKey.ScaleX:
				{
					var prevScaleX = rectTransform.localScale.x;
					var diffScaleX = Value - prevScaleX;
					return degree => {
						rectTransform.localScale = new Vector3(prevScaleX + diffScaleX * degree, rectTransform.localScale.y, rectTransform.localScale.z);
					};
				}
			case TweenKey.ScaleY:
				{
					var prevScaleY = rectTransform.localScale.y;
					var diffScaleY = Value - prevScaleY;
					return degree => {
						rectTransform.localScale = new Vector3(rectTransform.localScale.x, prevScaleY + diffScaleY * degree, rectTransform.localScale.z);
					};
				}
			case TweenKey.Brightness:
				{
					var ui = gameObject.GetComponent<Graphic>();
					var bright = ui.color.grayscale;
					var diff = Value - bright;
					return degree => {
						var d = bright + diff * degree;
						ui.color = new Color(d, d, d, ui.color.a);
					};
				}
			case TweenKey.Opacity:
				{
					var ui = gameObject.GetComponent<Graphic>();
					var prevOpacity = ui.color.a;
					var diff = Value - prevOpacity;
					return degree =>  ui.color = new Color(ui.color.r, ui.color.g, ui.color.b, prevOpacity + diff * degree);
				}
			case TweenKey.CanvasGroupAlpha:
				{
					var cg = gameObject.GetComponent<CanvasGroup>();
					if (cg == null) {
						Debug.LogWarningFormat("`{0}` is not attached CanvasGroup component", gameObject);
						return Empty;
					}
					var prev = cg.alpha;
					var diff = Value - prev;
					return degree => cg.alpha = prev + diff * degree;
				}
			case TweenKey.Alpha:
				{
					var cr = gameObject.GetComponent<CanvasRenderer>();
					if (cr == null) {
						Debug.LogWarningFormat("`{0}` is not attached CanvasRenderer component", gameObject);
						return Empty;
					}
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