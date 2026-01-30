using UnityEngine;
using System.Collections.Generic;

public class BalanceControl : MonoBehaviour
{
    [Header("References")]
    public Transform beam;         // 横架

    [Header("Rotation Settings")]
    public float maxAngle = 20f;   // 最大倾斜角度
    public float rotateSpeed = 5f; // 旋转平滑速度

    [Header("Debug")]
    public float leftWeight;
    public float rightWeight;

    private float targetAngle;

    void Update()
    {
        CalculateTargetAngle();
        RotateBeam();
    }

    void CalculateTargetAngle()
    {
        float diff = rightWeight - leftWeight;

        // 权重差映射到角度
        targetAngle = Mathf.Clamp(diff, -1f, 1f) * maxAngle;
    }

    void RotateBeam()
    {
        Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle);
        beam.localRotation = Quaternion.Lerp(
            beam.localRotation,
            targetRot,
            Time.deltaTime * rotateSpeed
        );
    }

    public void SetLeftWeight(float value)
    {
        leftWeight = Mathf.Max(0, value);
    }

    public void SetRightWeight(float value)
    {
        rightWeight = Mathf.Max(0, value);
    }

    public void AddLeftWeight(float value)
    {
        leftWeight += value;
    }

    public void AddRightWeight(float value)
    {
        rightWeight += value;
    }

    public bool IsBalanced(float tolerance = 0.1f)
    {
        return Mathf.Abs(leftWeight - rightWeight) <= tolerance;
    }
}