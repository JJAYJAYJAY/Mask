using UnityEngine;
using UnityEngine.UI; // 必须引用

public class CameraController : MonoBehaviour
{
    private float targetYAngle = 0f;
    private float currentYAngle = 0f;
    public float smoothTime = 0.3f;
    private float rotationVelocity;

    [Header("UI Settings")]
    public CanvasGroup arrowGroup; // 将包含左右箭头的父物体拖到这里
    public float threshold = 0.1f; // 角度差阈值

    void Update()
    {
        // 1. 平滑旋转逻辑
        currentYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetYAngle, ref rotationVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0, currentYAngle, 0);

        // 2. 核心逻辑：判断是否正在转动
        // 如果当前角度和目标角度的差值大于阈值，说明还在转
        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetYAngle));
        
        if (angleDiff > threshold)
        {
            SetArrowsActive(false); // 隐藏
        }
        else
        {
            SetArrowsActive(true);  // 显示
        }
    }

    void SetArrowsActive(bool isActive)
    {
        // alpha 为 0 隐藏，1 显示
        arrowGroup.alpha = isActive ? 1f : 0f;
        // interactable 决定按钮是否能点，防止隐藏时还能触发点击
        arrowGroup.interactable = isActive;
        arrowGroup.blocksRaycasts = isActive;
    }

    public void RotateLeft() => targetYAngle -= 90f;
    public void RotateRight() => targetYAngle += 90f;
}