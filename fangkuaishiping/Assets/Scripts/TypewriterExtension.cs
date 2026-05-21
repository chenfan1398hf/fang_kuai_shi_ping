using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DOTween 打字机效果扩展方法
/// </summary>
public static class TypewriterExtension
{
    /// <summary>
    /// 为 TextMeshProUGUI 添加打字机效果
    /// </summary>
    /// <param name="textComponent">TextMeshPro 文本组件</param>
    /// <param name="fullText">完整字符串</param>
    /// <param name="charDelay">每个字符显示的时间间隔（秒）</param>
    /// <param name="onComplete">完成时的回调</param>
    public static void DOTypewriter(this TextMeshProUGUI textComponent, string fullText, float charDelay, System.Action onComplete = null)
    {
        if (textComponent == null) return;

        // 停止该组件上正在进行的打字机动画
        DOTween.Kill(textComponent);
        textComponent.text = "";

        Sequence sequence = DOTween.Sequence();
        sequence.SetId(textComponent);   // 用组件作为动画ID，便于后续停止

        for (int i = 0; i < fullText.Length; i++)
        {
            int currentIndex = i;   // 捕获当前索引
            sequence.AppendCallback(() =>
            {
                if (textComponent != null)
                    textComponent.text = fullText.Substring(0, currentIndex + 1);
            });
            if (i < fullText.Length - 1)
                sequence.AppendInterval(charDelay);
        }

        if (onComplete != null)
            sequence.OnComplete(() => onComplete());

        sequence.Play();
    }

    /// <summary>
    /// 为 UGUI Text 添加打字机效果
    /// </summary>
    /// <param name="textComponent">UGUI 文本组件</param>
    /// <param name="fullText">完整字符串</param>
    /// <param name="charDelay">每个字符显示的时间间隔（秒）</param>
    /// <param name="onComplete">完成时的回调</param>
    public static void DOTypewriter(this Text textComponent, string fullText, float charDelay, System.Action onComplete = null)
    {
        if (textComponent == null) return;

        DOTween.Kill(textComponent);
        textComponent.text = "";

        Sequence sequence = DOTween.Sequence();
        sequence.SetId(textComponent);

        for (int i = 0; i < fullText.Length; i++)
        {
            int currentIndex = i;
            sequence.AppendCallback(() =>
            {
                if (textComponent != null)
                    textComponent.text = fullText.Substring(0, currentIndex + 1);
            });
            if (i < fullText.Length - 1)
                sequence.AppendInterval(charDelay);
        }

        if (onComplete != null)
            sequence.OnComplete(() => onComplete());

        sequence.Play();
    }
}