using MaloProduction.Tween;
using MaloProduction.Tween.DoTween.Module;
using UnityEngine;
using UnityEngine.UI;
using MaloProduction.Tween.Ease;

public class TweenExperience : MonoBehaviour
{
    public Vector2 test = Vector2.zero;
    public Image image;
    public RectTransform rectTransform;

    void Start()
    {
        if (image != null)
        {
            image.DoFade(0f, 5f)
                .OnComplete(() => Debug.Log("Complete"))
                .OnUpdate(() => Debug.Log("Update"))
                .OnPlay(() => Debug.Log("Play"))
                .SetEase(Easing.InOutBounce);
        }

        if (rectTransform != null)
        {
            rectTransform
                .DoAnchorMove(new Vector2(-920f, 0f), 2f)
                .OnComplete(
                    () => rectTransform.DoAnchorMove(new Vector2(0f, 0f), 2f)
                                            .AddDelay(3f))
                .OnUpdate(() => Debug.Log("Update"))
                .OnPlay(() => Debug.Log("Play"));
        }

        test.DoMove(Vector2.one * 20, 20f, () => test, (x) => test = x);
    }
}