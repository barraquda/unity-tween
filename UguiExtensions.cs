using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Barracuda;
using UnityEngine.UI;

namespace Barracuda.UISystem
{

	public enum PropKey
	{
		Scale,
		ScaleX,
		ScaleY,
		RotationX,
		RotationY,
		RotationZ,
		Opacity,
		OpacityChildren,
		X,
		Y,
		GlobalX,
		GlobalY,
		Width,
		Height,
		Size,
		Brightness,
		Alpha,
		CanvasGroupAlpha
	}
		
	/// <summary>
	/// UI Extensions
	/// </summary>
	public static class UIBehaviourExtensions
	{
		public static void Fix(this Graphic ui, params TweenProperty[] parameters)
		{
			foreach (TweenProperty prop in parameters) {
				prop.GetTweener(ui)(1.0f);
			}
		}

		public static TweenSet Animate(this RectTransform ui, TweenProperty[] parameters, float duration, EasingMode easingMode = null, Action onFinish = null, float offset = 0.0f)
		{
			var actions = new Action<float>[parameters.Length];
			for (int i = 0; i < parameters.Length; i++) {
				actions[i] = parameters[i].GetTweener(ui.GetComponent<Graphic>());
			}
			var animationSet = new TweenSet(ui, duration, actions, easingMode, onFinish, offset);
			UguiTweenManager.Add(animationSet);
			return animationSet;
		}

		public static TweenSet Animate(this RectTransform ui, TweenProperty parameter, float duration, EasingMode easingMode = null, Action onFinish = null, float offset = 0.0f)
		{
			var action = new Action<float>[]{ parameter.GetTweener(ui.GetComponent<Graphic>()) };
			var animationSet = new TweenSet(ui, duration, action, easingMode, onFinish, offset);
			UguiTweenManager.Add(animationSet);
			return animationSet;
		}

		public static TweenSet Alpha(this RectTransform ui, float alpha, float duration, Action onFinish = null, float offset = 0.0f)
		{
			return ui.Animate(new Absolute(PropKey.Alpha, alpha), duration, Easing.EaseOut, onFinish, offset);
		}

		public static TweenSet Opacity(this RectTransform ui, float opacity, float duration, Action onFinish = null, float offset = 0.0f)
		{
			return ui.Animate(new Absolute(PropKey.Opacity, opacity), duration, Easing.EaseOut, onFinish, offset);
		}

		private static Graphic GetGraphicFromGameObject(GameObject target)
		{
			var image = target.GetComponent<Image>();
			if (image != null) {
				return image;
			}

			var text = target.GetComponent<Text>();
			return text;
		}

		private static void CollectGraphics(List<Graphic> list, GameObject target)
		{
			var g = GetGraphicFromGameObject(target);
			if (g != null) {
				list.Add(g);
			}
			for (var i = 0; i < target.transform.childCount; i++) {
				var element = target.transform.GetChild(i);
				CollectGraphics(list, element.gameObject);
			}
		}
	}
}