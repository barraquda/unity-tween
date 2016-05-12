using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Barracuda.UISystem
{
	[System.Serializable]
	[RequireComponent(typeof(Graphic))]
	public class Tween : MonoBehaviour
	{
		[SerializeField] private TweenKey target;
		public TweenKey Target {
			get { return target; }
			set { target = value; }
		}

		[SerializeField] private TweenPropertyType propertyType;
		public TweenPropertyType PropertyType {
			get { return propertyType; }
			set { propertyType = value; }
		}

		[SerializeField] private float value;
		public float Value {
			get { return value; }
			set { this.value = value; }
		}

		[SerializeField] private AnimationCurve easing;
		public AnimationCurve Easing {
			get { return easing; }
			set { easing = value; }
		}

		[SerializeField] private float duration;
		public float Duration {
			get { return duration; }
			set { duration = value; }
		}

		public IStreamee<Unit> Streamee
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
				return null;
			}
		}

		void Start()
		{
		
		}
		
		void Update()
		{
		
		}

		public enum TweenPropertyType
		{
			Absolute,
			Relative,
			Multiple
		}
	}
}