# Unity Tween System

All function return `IEnumerable<ITween>`.
If you can use Linq or UniLinq (C# Linq implementation in Unity), let's opperate like below!  
The following script will execute 2 tweens subsequently.

```cs
public class Tester : MonoBehaviour
{
    [SerializeField] Image image1;
    [SerializeField] Image image2;

    void Start()
    {
        ITween tween1 = image1.Animate(/* TODO */);
        ITween tween2 = image2.Animate(/* TODO */);

        IEnumerable<ITween> concatenated = tween1.Concat(tween2);

        concatenated.Run();
    }
}
```

```cs
void Start()
{
    IEnumerable<ITween> tween1 = image1.Animate(/* TODO */);
    IEnumerable<ITween> tween2 = image2.Animate(/* TODO */);

    ITween merged = tween1.Merge(tween2);

    merged.Run();
}
```

```cs
void Start()
{
    IEnumerable<ITween> tween = GetTween(image1);
    tween.Run();
}

IEnumerable<ITween> GetTween(Graphic image)
{
    yield return image.Animate(/* TODO */);

    image.sprite = Resources.Load<Sprite>("...");

    yield return image.Animate(/* TODO */);

    yield return image.Animate(/* TODO */);
}
```