using MaloProduction.Tween.DoTween.Module;
using UnityEngine;

public class TweenExperience : MonoBehaviour
{
    Vector2 test = Vector2.one;
    void Start()
    {
        test.DoMove(Vector2.one * 15, 15f, () => test, (x) => test = x);
    }

    private void Update()
    {
        Debug.Log(test);
    }
}