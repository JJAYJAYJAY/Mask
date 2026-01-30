using System.Collections;
using UnityEngine;

public class DoorOpen : BaseInteractable
{
    public float openDuration = 1.0f; // 开门持续时间
    public float openAngle = -45f;   // 目标旋转角度
    private bool isOpen = false;      // 状态开关
    private Coroutine animationCoroutine;

    protected override void OnInteract()
    {
        // 如果动画正在运行，先停止它（防止连续点击导致冲突）
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        // 切换开关状态
        isOpen = !isOpen;

        // 计算目标：基于当前状态决定是到 openAngle 还是回到 0
        float targetY = isOpen ? openAngle : 0f;
        
        // 开启协程处理动画
        animationCoroutine = StartCoroutine(AnimateDoor(targetY));
    }

    private IEnumerator AnimateDoor(float targetY)
    {
        Transform doorParent = transform.parent;
        Quaternion startRot = doorParent.localRotation;
        Quaternion endRot = Quaternion.Euler(0, targetY, 0);
        
        float elapsed = 0;
        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;
            // 使用 Lerp 实现平滑插值
            doorParent.localRotation = Quaternion.Slerp(startRot, endRot, elapsed / openDuration);
            yield return null; // 等待下一帧
        }

        // 确保最终角度精准
        doorParent.localRotation = endRot;
        animationCoroutine = null;
    }
}