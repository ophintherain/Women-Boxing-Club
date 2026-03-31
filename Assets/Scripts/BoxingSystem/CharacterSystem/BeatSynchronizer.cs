using UnityEngine;

public class BeatSynchronizer : MonoBehaviour
{
    private Animator anim;

    [Header("同步设置")]
    public float hitThreshold = 0.15f; // 判定窗口

    void Start()
    {
        anim = GetComponent<Animator>();

        // 核心：订阅输入事件
        InputManager.Instance.OnAttackInput += HandleAttack;
        InputManager.Instance.OnJumpInput += HandleJump;
    }

    private void HandleAttack() => TryExecuteAction("Attack", 0.3f); // 假设攻击点在动画30%处
    private void HandleJump() => TryExecuteAction("Jump", 0.1f);    // 假设起跳点在动画10%处

    void TryExecuteAction(string triggerName, float impactPoint)
    {
        float offset = BeatManager.Instance.GetBeatOffset();
        bool isPerfect = Mathf.Abs(offset) < hitThreshold;

        if (isPerfect)
        {
            Debug.Log($"<color=cyan>PERFECT!</color> 偏移: {offset}");
        }
        else
        {
            Debug.Log($"<color=red>MISS</color> 偏移: {offset}");
        }

        SyncAnimation(triggerName, impactPoint, offset);
    }

    void SyncAnimation(string trigger, float impactPoint, float offset)
    {
        // 先触发动画
        anim.SetTrigger(trigger);

        // Hi-Fi Rush 插值算法核心：
        // 如果玩家按早了（offset为负），我们需要让动画加速，赶上节拍
        // 如果玩家按晚了（offset为正），我们需要让动画减速，等待节拍
        // 缩放系数 = 1 / (1 + offset)
        float playbackSpeed = 1f / (1f + offset);

        // 限制速度范围在 0.8 到 1.5 之间，避免视觉过于突兀
        anim.speed = Mathf.Clamp(playbackSpeed, 0.8f, 1.5f);
    }

    // 重要：动画结束时必须由 Animation Event 调用此函数恢复正常速度
    public void ResetAnimSpeed()
    {
        anim.speed = 1.0f;
    }

    // 脚本销毁时记得取消订阅，防止内存泄漏
    void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnAttackInput -= HandleAttack;
            InputManager.Instance.OnJumpInput -= HandleJump;
        }
    }
}