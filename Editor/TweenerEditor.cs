using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using Barracuda.UISystem;

namespace Barracuda.Editor
{
	[CustomEditor(typeof(Tweener))]
	public class MonoStreamerEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			var serializedTween = serializedObject.FindProperty("tween");

			serializedTween.objectReferenceValue = EditorGUILayout.ObjectField(
				serializedTween == null ? null : serializedTween.objectReferenceValue,
				typeof(TweenBase),
				allowSceneObjects: true);
			
			if (serializedTween == null || serializedTween.objectReferenceValue == null) {
				EditorGUILayout.HelpBox("No Tween is registered!", MessageType.Warning);
			} else {
				var tween = (TweenBase)serializedTween.objectReferenceValue;
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Run")) {
					((Tweener)target).Begin();
				}
				if (GUILayout.Button("Stop")) {
					((Tweener)target).Stop();
				}
				EditorGUILayout.EndHorizontal();
				InspectTween(tween);
			}

			serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(target);
		}

		private void InspectTween(TweenBase tweenBase)
		{
			var defaultColor = GUI.backgroundColor;
			GUI.backgroundColor = GetColorByTween(tweenBase);
			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUI.backgroundColor = defaultColor;
			{
				var tween = tweenBase as Tween;
				if (tween != null) {
					EditorGUILayout.BeginHorizontal();
					{
						tween.Easing = EditorGUILayout.CurveField(tween.Easing, GUILayout.Height(72));

						EditorGUILayout.BeginVertical();
						{
							tween.Target = (TweenKey)EditorGUILayout.EnumPopup("Target", tween.Target);
							tween.PropertyType = (Tween.TweenPropertyType)EditorGUILayout.EnumPopup("Type", tween.PropertyType);
							tween.Value = EditorGUILayout.FloatField("Value", tween.Value);
							tween.Duration = EditorGUILayout.FloatField("Duration", tween.Duration);
						}
						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndHorizontal();
				}
				var waitTween = tweenBase as WaitTween;
				if (waitTween != null) {
					waitTween.Duration = EditorGUILayout.FloatField("Wait for", waitTween.Duration);
				}

				var subsequentTween = tweenBase as SubsequentTween;
				if (subsequentTween != null) {
					subsequentTween.Interval = EditorGUILayout.FloatField("Interval", subsequentTween.Interval);
				}

				var collectionTween = tweenBase as CollectionTweenBase;
				if (collectionTween != null) {
					if (collectionTween.Tweens == null || collectionTween.Tweens.Length == 0) {
						EditorGUILayout.HelpBox("No Tween is registered!", MessageType.Warning);
					} else {
						foreach (var t in collectionTween.Tweens) {
							if (t != null) {
								InspectTween(t);
							} else {
								EditorGUILayout.HelpBox("Null Element", MessageType.Error);
							}
						}
					}
				}
			}

			EditorGUILayout.EndVertical();
		}

		private Color GetColorByTween(TweenBase tween)
		{
			if (tween is Tween) {
				return Color.black;
			}
			if (tween is WaitTween) {
				return Color.red;
			}
			if (tween is SerialTween) {
				return Color.blue;
			}
			if (tween is CompositeTween) {
				return Color.green;
			}
			if (tween is SubsequentTween) {
				return Color.magenta;
			}
			return Color.black;
		}
	}
}