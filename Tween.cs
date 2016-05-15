using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

		public override IStreamee<Unit> Streamee
		{
			get {
				var ui = GetComponent<Graphic>();
				TweenProperty property = null;
				switch (propertyType) {
				case TweenPropertyType.Absolute:
					property = new Absolute(target, value);
					break;
				case TweenPropertyType.Relative:
					property = new Relative(target, value);
					break;
				case TweenPropertyType.Multiple:
					property = new Multiple(target, value);
					break;
				}
				var streamee = gameObject.Animate(property, duration, Barracuda.Easing.FromAnimationCurve(easing));
				if (loop) {
					return AnimateForever(streamee).ToStreamee();
				}
				return streamee;
			}
		}

		IEnumerable<IStreamee<Unit>> AnimateForever(IStreamee<Unit> tween)
		{
			while (true) {
				yield return tween;
			}
		}

		public enum TweenPropertyType
		{
			Absolute,
			Relative,
			Multiple
		}
	}
}