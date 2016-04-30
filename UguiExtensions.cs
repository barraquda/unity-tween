using UnityEngine;
using System;
using System.Collections.Generic;
using Barracuda;
using UnityEngine.UI;

namespace Barracuda.UISystem
{

    public enum PropKey
    {
        Left, Bottom, Scale, ScaleX, ScaleY, RotationX, RotationY, RotationZ, Opacity, OpacityChildren, X, Y, GlobalX, GlobalY, Width, Height, Size, Brightness, Alpha, CanvasRendererAlpha
    }

    public abstract class UGUIProp
    {
        public PropKey Key { get; private set; }
        public float Value { get; private set; }

        protected UGUIProp (PropKey key, float val)
        {
            this.Key = key;
            this.Value = val;
        }
    }

    public class Relative : UGUIProp
    {
        public Relative (PropKey key, float val) : base(key, val) {}
    }

    public class Absolute : UGUIProp
    {
        public Absolute (PropKey key, float val) : base(key, val) {}
    }

    public class Multiple : UGUIProp
    {
        public Multiple (PropKey key, float val) : base(key, val) {}
    }

    public static class UguiExtensions
    {
        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/ Ugui Extension  /_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_//

        public static void Fix(this RectTransform ui, params UGUIProp[] parameters)
        {
            foreach (UGUIProp prop in parameters)
            {
                MatchProp(ui, prop)(1.0f);
            }
        }

        public static TweenSet Animate(this RectTransform ui, UGUIProp[] parameters , float duration, EasingMode easingMode = null, Action onFinish = null, float offset = 0.0f)
        {
            var actions = new Action<float>[parameters.Length];
            for (int i=0; i<parameters.Length; i++)
            {
                actions[i] = MatchProp(ui, parameters[i]);
            }
            var animationSet = new TweenSet(ui, duration, actions, easingMode, onFinish, offset);
            UguiTweenManager.Add(animationSet);
            return animationSet;
        }

        public static TweenSet Animate(this RectTransform ui, UGUIProp parameter , float duration, EasingMode easingMode = null, Action onFinish = null, float offset = 0.0f)
        {
            var action = new Action<float>[]{ MatchProp(ui, parameter) };
            var animationSet = new TweenSet(ui, duration, action, easingMode, onFinish, offset);
            UguiTweenManager.Add(animationSet);
            return animationSet;
        }

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/ Ugui Optional Extension /_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_//

        public static TweenSet Alpha(this RectTransform ui, float alpha, float duration, Action onFinish = null, float offset = 0.0f)
        {
            return ui.Animate(new Absolute(PropKey.Alpha, alpha), duration, Easing.EaseOut, onFinish, offset);
        }

        public static TweenSet Opacity(this RectTransform ui, float opacity, float duration, Action onFinish = null, float offset = 0.0f)
        {
            return ui.Animate(new Absolute(PropKey.Opacity, opacity), duration, Easing.EaseOut, onFinish, offset);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/ Ugui Extension Utility /_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_//

        private static Graphic GetGraphicFromGameObject(GameObject target)
        {
            var image = target.GetComponent<Image>();
            if (image != null)
            {
                return image;
            }

            var text = target.GetComponent<Text>();
            return text;
        }

        private static void SetOpacity(Graphic g, float opacity)
        {
            if (g != null)
            {
                g.color = new Color(g.color.r, g.color.g, g.color.b, opacity);
            }
        }

        private static void CollectGraphics(List<Graphic> list, GameObject target)
        {
            var g = GetGraphicFromGameObject(target);
            if (g != null)
            {
                list.Add(g);
            }
            for(var i = 0; i < target.transform.childCount; i++)
            {
                var element = target.transform.GetChild(i);
                CollectGraphics(list, element.gameObject);
            }
        }


        internal static Action<float> MatchProp(RectTransform ui, UGUIProp prop)
        {
            if (prop is Absolute)
            {
                switch (prop.Key)
                {
                    case PropKey.Width:
                    {
                        var prevWidth = ui.sizeDelta.x;
                        var diff = prop.Value - prevWidth;
                        return degree => {
                            ui.sizeDelta = new Vector2(prevWidth + diff * degree, ui.sizeDelta.y);
                        };
                    }
                    case PropKey.X:
                    {
                        var prevX = ui.anchoredPosition.x;
                        var diffX = prop.Value - prevX;
                        return degree => {
                            ui.anchoredPosition = new Vector2(prevX + diffX * degree, ui.anchoredPosition.y);
                        };
                    }
                    case PropKey.Y:
                    {
                        var prevY = ui.anchoredPosition.y;
                        var diffY = prop.Value - prevY;
                        return degree => {
                            ui.anchoredPosition = new Vector2(ui.anchoredPosition.x, prevY + diffY * degree);
                        };
                    }
                    case PropKey.GlobalX:
                    {
                        var prevX = ui.position.x;
                        var diffX = prop.Value - prevX;
                        return degree => {
                            ui.position = new Vector2(prevX + diffX * degree, ui.position.y);
                        };
                    }
                    case PropKey.GlobalY:
                    {
                        var prevY = ui.position.y;
                        var diffY = prop.Value - prevY;
                        return degree => {
                            ui.position = new Vector2(ui.position.x, prevY + diffY * degree);
                        };
                    }
                    case PropKey.Left:
                    {
                        var targetLeft = prop.Value;
                        var prevMinX = ui.anchorMin.x;
                        var prevMaxX = ui.anchorMax.x;
                        var diffMinX = targetLeft - prevMinX;
                        var diffMaxX = targetLeft - prevMaxX + (ui.anchorMax.x - ui.anchorMin.x);
                        return degree => {
                            var nowMax = ui.anchorMax;
                            var nowMin = ui.anchorMin;
                            ui.anchorMax = new Vector2(prevMaxX + (diffMaxX * degree), nowMax.y);
                            ui.anchorMin = new Vector2(prevMinX + (diffMinX * degree), nowMin.y);
                        };
                    }
                    case PropKey.Bottom:
                    {
                        var targetLeft = prop.Value;
                        var prevMinY = ui.anchorMin.y;
                        var prevMaxY = ui.anchorMax.y;
                        var diffMinY = targetLeft - prevMinY;
                        var diffMaxY = targetLeft - prevMaxY + (ui.anchorMax.y - ui.anchorMin.y);
                        return degree => {
                            var nowMax = ui.anchorMax;
                            var nowMin = ui.anchorMin;
                            ui.anchorMax = new Vector2(nowMax.x, prevMaxY + (diffMaxY * degree));
                            ui.anchorMin = new Vector2(nowMin.x, prevMinY + (diffMinY * degree));
                        };
                    }
                    case PropKey.RotationZ:
                    {
                        var rotationZ = prop.Value;
                        var prevRotation = ui.localRotation.eulerAngles;
                        var diff = (rotationZ - prevRotation.z) % 360;
                        return degree => {
                            var v = new Vector3(prevRotation.x, prevRotation.y, prevRotation.z + diff * degree);
                            ui.localRotation = Quaternion.Euler(v);
                        };
                    }
                    case PropKey.RotationY:
                    {
                        var rotationY = prop.Value;
                        var prevRotation = ui.localRotation.eulerAngles;
                        var diff = (rotationY - prevRotation.y) % 360;
                        return degree => {
                            var v = new Vector3(prevRotation.x, prevRotation.y + diff * degree, prevRotation.y);
                            ui.localRotation = Quaternion.Euler(v);
                        };
                    }
                    case PropKey.RotationX:
                    {
                        var rotationX = prop.Value;
                        var prevRotation = ui.localRotation.eulerAngles;
                        var diff = (rotationX - prevRotation.x) % 360;
                        return degree => {
                            var v = new Vector3(prevRotation.x + diff * degree, prevRotation.y, prevRotation.z);
                            ui.localRotation = Quaternion.Euler(v);
                        };
                    }
                    case PropKey.Scale:
                    {
                        var prevScale = ui.localScale;
                        var diffScale = new Vector3(1, 1, 1) * prop.Value - prevScale;
                        return degree => {
                            ui.localScale = prevScale + diffScale * degree;
                        };
                    }
                    case PropKey.ScaleX:
                    {
                        var prevScaleX = ui.localScale.x;
                        var diffScaleX = prop.Value - prevScaleX;
                        return degree => {
                            ui.localScale = new Vector3(prevScaleX + diffScaleX * degree, ui.localScale.y, ui.localScale.z);
                        };
                    }
                    case PropKey.ScaleY:
                    {
                        var prevScaleY = ui.localScale.y;
                        var diffScaleY = prop.Value - prevScaleY;
                        return degree => {
                            ui.localScale = new Vector3(ui.localScale.x, prevScaleY + diffScaleY * degree, ui.localScale.z);
                        };
                    }
                    case PropKey.Brightness:
                    {
                        var g = GetGraphicFromGameObject(ui.gameObject);
                        var bright = g.color.grayscale;
                        var diff = prop.Value - bright;
                        return degree => {
                            var d = bright + diff * degree;
                            g.color = new Color(d, d, d, g.color.a);
                        };
                    }
                    case PropKey.Opacity:
                    {
                        var g = GetGraphicFromGameObject(ui.gameObject);
                        if (g == null)
                        {
                            return degree => {};
                        }
                        var prevOpacity = g.color.a;
                        var diff = prop.Value - prevOpacity;
                        return degree => SetOpacity(g, prevOpacity + diff * degree);
                    }
                    case PropKey.OpacityChildren:
                    {
                        var gs = new List<Graphic>();
                        CollectGraphics(gs, ui.gameObject);
                        var opacity = prop.Value;
                        var prevOpacities = new float[gs.Count];
                        var index = 0;
                        gs.ForEach(element => {
                            prevOpacities[index] = element.color.a;
                            index++;
                        });
                        return degree => {
                            index = 0;
                            gs.ForEach(element => {
                                SetOpacity(element, prevOpacities[index] + (opacity - prevOpacities[index]) * degree);
                                index++;
                            });
                        };
                    }
                    case PropKey.Alpha:
                    {
                        var cg = ui.GetComponent<CanvasGroup>();
                        var prev = cg.alpha;
                        var diff = prop.Value - prev;
                        return degree => { cg.alpha = prev + diff * degree; };
                    }
                    case PropKey.CanvasRendererAlpha:
                    {
                        var cr = ui.GetComponent<CanvasRenderer>();
                        var prev = cr.GetAlpha();
                        var diff = prop.Value - prev;
                        return degree => cr.SetAlpha(prev + diff * degree);
                    }
                }
            }
            else if (prop is Relative)
            {
                switch (prop.Key)
                {
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
                    case PropKey.Left:
                    {
                        var left = prop.Value;
                        var prevMinX = ui.anchorMin.x;
                        var prevMaxX = ui.anchorMax.x;
                        return degree => {
                            var nowMax = ui.anchorMax;
                            var nowMin = ui.anchorMin;
                            ui.anchorMax = new Vector2(prevMaxX + left * degree, nowMax.y);
                            ui.anchorMin = new Vector2(prevMinX + left * degree, nowMin.y);
                        };
                    }
                    case PropKey.Bottom:
                    {
                        var bottom = prop.Value;
                        var prevMinY = ui.anchorMin.y;
                        var prevMaxY = ui.anchorMax.y;
                        return degree => {
                            var nowMax = ui.anchorMax;
                            var nowMin = ui.anchorMin;
                            ui.anchorMax = new Vector2(nowMax.x, prevMaxY + bottom * degree);
                            ui.anchorMin = new Vector2(nowMin.x, prevMinY + bottom * degree);
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
            }
            else if (prop is Multiple)
            {
                switch (prop.Key)
                {
                    case PropKey.Scale:
                    {
                        var prevScale = ui.localScale;
                        var diffScale = prop.Value * ui.localScale - prevScale;
                        return degree => {
                            ui.localScale = prevScale + diffScale * degree;
                        };
                    }
                    case PropKey.ScaleX:
                    {
                        var prevScaleX = ui.localScale.x;
                        var diffScaleX = prop.Value * ui.localScale.x - prevScaleX;
                        return degree => {
                            ui.localScale = new Vector2(prevScaleX + diffScaleX * degree, ui.localScale.y);
                        };
                    }
                    case PropKey.ScaleY:
                    {
                        var prevScaleY = ui.localScale.y;
                        var diffScaleY = prop.Value * ui.localScale.y - prevScaleY;
                        return degree => {
                            ui.localScale = new Vector2(ui.localScale.x ,prevScaleY + diffScaleY * degree);
                        };
                    }
                }
            }
            else
            {
                throw new ArgumentException("[UguiExtensions] properties is illegal");
            }

            return null;
        }
    }
}