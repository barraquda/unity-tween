using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Barracuda.UISystem
{
	public interface ICollectionTween
	{
		TweenBase[] Tweens { get; }
		bool Loop { get; set; }
	}
	
}