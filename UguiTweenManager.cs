using UnityEngine;
using System.Collections.Generic;
using System;
using Barracuda;
using UnityEngine.UI;

namespace Barracuda.UISystem
{
	class TweenData
	{
		public float Offset { get; set; }

		public float Duration { get; private set; }

		public EasingMode EasingMode  { get; private set; }

		public Action OnFinish  { get; private set; }

		private readonly Action<float>[] animations;
		private readonly Action<float> animation;

		public TweenData(float duration, Action<float>[] animations, EasingMode easingMode, Action onFinish, float offset)
		{
			if (duration.CompareTo(0) != 1) {
				throw new ArgumentException("[TweenData] duration must be over 0");
			}

			Duration = duration;
			Offset = offset;

			if (animations.Length == 1) {
				animation = animations[0];
			} else {
				this.animations = animations;
			}

			EasingMode = easingMode ?? Easing.Linear;
			OnFinish = onFinish;
		}

		public TweenData(float duration, Action<float> animation, EasingMode easingMode, Action onFinish, float offset)
		{
			Duration = duration;
			this.animation = animation;
			Offset = offset;

			EasingMode = easingMode ?? Easing.Linear;
			OnFinish = onFinish;
		}

		public void Execute(float v)
		{
			if (animation != null) {
				animation(v);
			} else {
				foreach (Action<float> a in animations) {
					a(v);
				}
			}
		}
	}

	public class TweenSet
	{
		private TweenData targetAnimation;
		private readonly Queue<Action> onCompletes;

		private RectTransform ui;
		private bool isAnimation;
		private float time;

		public TweenSet(RectTransform ui, float duration, Action<float>[] animations, EasingMode easingMode, Action onFinish, float offset)
		{
			onCompletes = new Queue<Action>();
			targetAnimation = new TweenData(duration, animations, easingMode, onFinish, offset);
			this.ui = ui;
		}

		public TweenSet Animate(TweenProperty[] parameters, float duration, EasingMode easingMode = null, Action onFinish = null, float offset = 0.0f)
		{
			onCompletes.Enqueue(() => {
				var actions = new Action<float>[parameters.Length];
				for (int i = 0; i < parameters.Length; i++) {
					actions[i] = parameters[i].GetTweener(ui.GetComponent<Graphic>());
				}
				targetAnimation = new TweenData(duration, actions, easingMode, onFinish, offset);
			});
			return this;
		}

		public TweenSet Animate(TweenProperty parameter, float duration, EasingMode easingMode = null, Action onFinish = null, float offset = 0.0f)
		{
			onCompletes.Enqueue(() => {
				var action = parameter.GetTweener(ui.GetComponent<Graphic>());
				targetAnimation = new TweenData(duration, action, easingMode, onFinish, offset);
			});
			return this;
		}

		public void Execute()
		{
			isAnimation = true;
			time = 0.0f;
		}

		public void Kill()
		{
			UguiTweenManager.Remove(this);
		}

		public void Finish()
		{
			time = targetAnimation.Duration;
		}

		public bool Invoke()
		{
			if (targetAnimation != null) {
				if (isAnimation && targetAnimation.Offset > 0) {
					targetAnimation.Offset -= Time.deltaTime;
					return true;
				}

				if (isAnimation) {
					if (targetAnimation.Offset < time) {
						var now = targetAnimation.EasingMode(time, time, 0, 1, targetAnimation.Duration);
						targetAnimation.Execute(now);
					}
					time += Time.deltaTime;
				}

				if (time <= targetAnimation.Duration) {
					var now = targetAnimation.EasingMode(time, time, 0, 1, targetAnimation.Duration);
					targetAnimation.Execute(now);
				} else if (time > targetAnimation.Duration) {
					targetAnimation.Execute(targetAnimation.EasingMode(targetAnimation.Duration, targetAnimation.Duration, 0, 1, targetAnimation.Duration));

					if (targetAnimation.OnFinish != null) {
						targetAnimation.OnFinish();
					}

					if (onCompletes == null) {
						return false;
					}

					if (onCompletes.Count > 0) {
						time = 0.0f;
						onCompletes.Dequeue()();
					} else {
						return false;
					}
				}
			}
			return true;
		}

	}

	public class UguiTweenManager : MonoBehaviour
	{
		private static UguiTweenManager instance;
		private LinkedList<TweenSet> animationSets;
		private LinkedList<TweenSet> removeCandidates;

		public static UguiTweenManager GetInstance()
		{
			return instance;
		}

		public bool IsAnimating {
			get {
				return animationSets.Count > 0;
			}
		}

		public static void Init()
		{
			var obj = new GameObject();
			obj.name = "UGUIAnimationManager";
			instance = obj.AddComponent<UguiTweenManager>();
			instance.animationSets = new LinkedList<TweenSet>();
			instance.removeCandidates = new LinkedList<TweenSet>();
		}

		public static void Clear()
		{
			if (instance != null) {
				instance.animationSets.Clear();
			}
		}

		public static void Remove(TweenSet animationSet)
		{
			instance.animationSets.Remove(animationSet);
		}

		public static void Add(TweenSet animationSet)
		{
			if (instance == null) {
				Init();
			}
			instance.animationSets.AddLast(animationSet);
			animationSet.Execute();
		}

		void Update()
		{
			for (LinkedListNode<TweenSet> node = animationSets.First; node != null; node = node.Next) {
				if (!node.Value.Invoke()) {
					removeCandidates.AddFirst(node.Value);
				}
			}

			if (removeCandidates.Count > 0) {
				for (LinkedListNode<TweenSet> node = removeCandidates.First; node != null; node = node.Next) {
					animationSets.Remove(node.Value);
				}

				removeCandidates.Clear();
			}

		}
	}
}