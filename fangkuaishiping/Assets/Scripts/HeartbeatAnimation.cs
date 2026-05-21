using UnityEngine;
using DG.Tweening;

/// <summary>
/// DOTween心跳动画脚本
/// 让物体以缩放的形式模拟心跳效果，支持自定义缩放范围、跳动时间和间隔时间
/// </summary>
public class HeartbeatAnimation : MonoBehaviour
{
    [Header("缩放参数")]
    [Tooltip("原始大小（心跳间隔期间的大小）")]
    public float minScale = 1f;

    [Tooltip("心跳峰值时的最大大小")]
    public float maxScale = 1.2f;

    [Header("时间参数")]
    [Tooltip("从min缩放到max（或从max缩放回min）的持续时间")]
    public float beatDuration = 0.15f;

    [Tooltip("两次心跳之间的间隔时间")]
    public float interval = 1f;

    [Header("可选设置")]
    [Tooltip("使用的缓动曲线类型")]
    public Ease easeType = Ease.OutBounce;

    [Tooltip("是否在Start时自动开始动画")]
    public bool autoStart = true;

    [Tooltip("是否可以重复播放（如果为false，播放一次后停止）")]
    public bool loop = true;

    private Sequence heartbeatSequence;
    private Tween currentScaleTween;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        if (autoStart)
        {
            StartHeartbeatAnimation();
        }
    }

    void OnDisable()
    {
        // 当对象禁用时，暂停或杀死动画
        StopHeartbeatAnimation();
    }

    /// <summary>
    /// 开始心跳动画
    /// </summary>
    public void StartHeartbeatAnimation()
    {
        // 如果已经有动画在运行，先停止
        StopHeartbeatAnimation();

        // 创建新的心跳动画序列
        CreateHeartbeatSequence();

        // 开始动画
        if (heartbeatSequence != null)
        {
            heartbeatSequence.Play();
        }
    }

    /// <summary>
    /// 停止心跳动画
    /// </summary>
    public void StopHeartbeatAnimation()
    {
        if (heartbeatSequence != null)
        {
            heartbeatSequence.Kill();
            heartbeatSequence = null;
        }

        if (currentScaleTween != null)
        {
            currentScaleTween.Kill();
            currentScaleTween = null;
        }

        // 重置缩放
        transform.localScale = originalScale;
    }

    /// <summary>
    /// 重新开始心跳动画
    /// </summary>
    public void RestartHeartbeatAnimation()
    {
        StopHeartbeatAnimation();
        CreateHeartbeatSequence();
        heartbeatSequence?.Restart();
    }

    /// <summary>
    /// 创建心跳动画序列
    /// </summary>
    private void CreateHeartbeatSequence()
    {
        heartbeatSequence = DOTween.Sequence();

        // 收缩到最小值 -> 弹起到最大值 -> 收缩回最小值
        Sequence beatSequence = CreateSingleBeat();

        // 添加一个心跳动画到序列
        heartbeatSequence.Append(beatSequence);

        // 如果启用循环，添加间隔并无限循环
        if (loop)
        {
            heartbeatSequence.AppendInterval(interval);
            heartbeatSequence.SetLoops(-1, LoopType.Restart);
        }

        // 设置动画链接，确保物体销毁时动画也被清除
        heartbeatSequence.SetLink(gameObject);

        // 可以添加其他设置
        heartbeatSequence.SetAutoKill(true);
    }

    /// <summary>
    /// 创建单次心跳的动画序列
    /// </summary>
    private Sequence CreateSingleBeat()
    {
        Sequence singleBeat = DOTween.Sequence();

        // 设置当前缩放为最小值
        transform.localScale = originalScale * minScale;

        // 从最小值缩放到最大值（心跳峰值）
        currentScaleTween = transform.DOScale(originalScale * maxScale, beatDuration)
            .SetEase(easeType)
            .SetAutoKill(false)
            .Pause(); // 暂停，等待序列控制

        // 添加到序列
        singleBeat.Append(currentScaleTween);

        // 从最大值缩放回最小值（恢复）
        singleBeat.Append(transform.DOScale(originalScale * minScale, beatDuration)
            .SetEase(Ease.OutBack));

        return singleBeat;
    }

    /// <summary>
    /// 更新心跳参数（运行时更新）
    /// </summary>
    public void UpdateParameters(float newMinScale, float newMaxScale, float newDuration, float newInterval)
    {
        minScale = newMinScale;
        maxScale = newMaxScale;
        beatDuration = newDuration;
        interval = newInterval;

        // 如果动画正在运行，重启它
        if (heartbeatSequence != null)
        {
            RestartHeartbeatAnimation();
        }
    }
}