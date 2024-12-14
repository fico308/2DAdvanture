using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage;

    [Header("Event Listener")]
    public FadeImageEventSO fadeImageEvent;

    private void OnEnable()
    {
        fadeImageEvent.OnEventRaised += FadeImage;
    }

    private void OnDisable()
    {
        fadeImageEvent.OnEventRaised -= FadeImage;
    }

    private void FadeImage(Color color, float duration, bool fadeIn)
    {
        fadeImage.DOBlendableColor(color, duration);
    }

}
