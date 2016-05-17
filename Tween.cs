using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

namespace Barracuda.UISystem
{
	public class Tween : TweenBase
	{
		[SerializeField] GameObject targetObject;

		[SerializeField] TweenKey target;
		public TweenKey Target {
			get { return target; }
			set { target = value; }
		}

		[SerializeField] TweenPropertyType propertyType;
		public TweenPropertyType PropertyType {
			get { return propertyType; }
			set { propertyType = value; }
		}

		[SerializeField] float value;
		public float Value {
			get { return value; }
			set { this.value = value; }
		}

		[SerializeField] AnimationCurve easing;
		public AnimationCurve Easing {
			get { return easing; }
			set { easing = value; }
		}

		[SerializeField] float duration;
		public float Duration {
			get { return duration; }
			set { duration = value; }
		}

		bool hasDefaultValue;
		float defaultValue;

		protected override IEnumerable<Unit> TweenStreamee
		{
			get {
				var property = GetProperty(value);

				var streamee = GetTweenEnumerable(property);
				return streamee;
			}
		}

		IEnumerable<Unit> GetTweenEnumerable(TweenProperty property)
		{
			var gameObject = this.targetObject == null ? this.gameObject : this.targetObject;
			if (!hasDefaultValue) {
				hasDefaultValue = true;
				defaultValue = property.GetCurrentValue(gameObject);
			}
			if (duration > 0) {
				var easing = this.easing == null ? Barracuda.Easing.Linear : Barracuda.Easing.FromAnimationCurve(this.easing);
				foreach (var unit in gameObject.Animate(property, duration, easing)) {
					yield return unit;
				}
			} else {
				gameObject.Fix(property);
			}
		}

		public override void Revert()
		{
			var gameObject = this.targetObject == null ? this.gameObject : this.targetObject;
			if (hasDefaultValue) {
				gameObject.Fix(new Absolute(target, defaultValue));
			}
		}

		TweenProperty GetProperty(float value)
		{
			switch (propertyType) {
			case TweenPropertyType.Absolute:
				return new Absolute(target, value);
			case TweenPropertyType.Relative:
				return new Relative(target, value);
			case TweenPropertyType.Multiple:
				return new Multiple(target, value);
			}
			return null;
		}

		public enum TweenPropertyType
		{
			Absolute,
			Relative,
			Multiple
		}
	}
}