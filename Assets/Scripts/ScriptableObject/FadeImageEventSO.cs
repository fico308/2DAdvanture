using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeImageEventSO")]
public class FadeImageEventSO : ScriptableObject {
    public UnityAction<Color, float, bool> OnEventRaised;

    /// <summary>
    /// 颜色逐渐变黑
    /// </summary>
    /// <param name="duration"></param>
    public void FaedIn(float duration)
    {
        RaiseEvent(Color.black, duration, true);
    }

    /// <summary>
    /// 逐渐变透明
    /// </summary>
    /// <param name="duration"></param>
    public void FaedOut(float duration)
    {
        RaiseEvent(Color.clear, duration, false);
    }

    private void RaiseEvent(Color color, float duration, bool fadeIn)
    {
        OnEventRaised?.Invoke(color, duration, fadeIn);
    }

}