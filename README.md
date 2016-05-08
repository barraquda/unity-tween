# Unity Tween System

All function return `IEnumerable<IStreamee<Unit>>`.  
(`IStreamee<T>` is our framework using IEnumerable<T>. The detail is not discribed here)


## Usage

```cs
using Barracuda.UISystem;
using Barracuda;

public class Tester : MonoBehaviour
{
	[SerializeField] private Image image1;
	[SerializeField] private Image image2;
	[SerializeField] private Text text;

	Streamer<Unit> streamer;

	void Start ()
	{
		streamer = new Streamer<Unit>(AnimateSubsequently().ToStreamee());
	}

	void Update ()
	{
		streamer.Feed();
	}

	private IEnumerable<IStreamee<Unit>> AnimateSubsequently()
	{
		yield return image1.Animate(new Absolute(PropKey.X, 0), 2f);
		yield return image1.Animate(new Absolute(PropKey.Y, 0), 1f);

		yield return Streamee.Wait(TimeSpan.FromSeconds(2f));

		yield return Streamee.MergeToUnit(
			image1.Animate(new Absolute(PropKey.Alpha, 0), 2f),
			image2.Animate(new Absolute(PropKey.Alpha, 0), 2f)
		);
	}
}
```
