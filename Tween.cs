using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

namespace Barracuda.UISystem
{
	[RequireComponent(typeof(Graphic))]
	public class Tween : TweenBase
	{
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

		[SerializeField] bool loop;
		public bool Loop {
			get { return loop; }
			set { loop = value; }
		}

		bool hasDefaultValue;
		float defaultValue;

		public override IStreamee<Unit> Streamee
		{
			get {
				var ui = GetComponent<Graphic>();
				var property = GetProperty(value);

				var streamee = GetTweenEnumerable(property).ToStreamee();
				return streamee;
			}
		}

		IEnumerable<IStreamee<Unit>> GetTweenEnumerable(TweenProperty property)
		{
			if (!hasDefaultValue) {
				hasDefaultValue = true;
				defaultValue = property.GetCurrentValue(gameObject);
				Debug.Log(property.Key + " default : " + defaultValue);
			}
			do {
				yield return gameObject.Animate(property, duration, Barracuda.Easing.FromAnimationCurve(easing));
			} while (loop);
		}

		public override void Revert()
		{
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