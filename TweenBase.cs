using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Barracuda.UISystem
{
	public abstract class TweenBase : MonoBehaviour
	{
		public abstract IStreamee<Unit> Streamee { get; }
	}
	
}