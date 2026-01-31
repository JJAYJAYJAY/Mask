using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BalanceControl : MonoBehaviour
{
    [Header("References")]
    public Transform beam;         // 横架

    [Header("Rotation Settings")]
    public float maxAngle = 20f;   // 最大倾斜角度
    public float rotateSpeed = 5f; // 旋转平滑速度

    public BalanceSlot rightSlot;
    public BalanceSlot leftSlot;

    [Header("Debug")]
    public float leftWeight;
    public float rightWeight;

    [Header("Consume Visuals")]
    public Animator spitAnimator;     // 吐纸条 Animator
    public float fadeDuration = 0.4f; // 物品淡出时间
    public CanvasGroup textCanvasGroup;

    public ItemData mustItem;
    private float targetAngle;
    private bool isConsuming; // 防止重复触发

    void Update()
    {
        CalculateTargetAngle();
        RotateBeam();
    }

    void CalculateTargetAngle()
    {
        float diff = rightWeight - leftWeight;
        targetAngle = -Mathf.Clamp(diff, -1f, 1f) * maxAngle;
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

    public bool IsBalanced(float tolerance = 0.1f)
    {
        return Mathf.Abs(leftWeight - rightWeight) <= tolerance;
    }

    public bool IsMust()
    {
        for (int i = 0; i < leftSlot.transform.childCount; i++)
        {
            if (leftSlot.transform.GetChild(i).gameObject.GetComponent<SidebarItem>().data == mustItem)
            {
                return true;
            }
        }
        for (int i = 0; i < rightSlot.transform.childCount; i++)
        {
            if (rightSlot.transform.GetChild(i).gameObject.GetComponent<SidebarItem>().data == mustItem)
            {
                return true;
            }
        }
        return false;
    }

    // ===================== 吞噬逻辑 =====================
    public void OnCheckClick()
    {
        if (isConsuming) return;

        if (IsBalanced())
        {
            if (IsMust())
            {
                StartCoroutine(ConsumeRoutine());
            }
        }
        else
        {
            GameManager.Instance.showText("天平还没平衡");
        }
    }

    IEnumerator ConsumeRoutine()
    {
        isConsuming = true;

        // 1️⃣ 左右槽位淡出
        yield return StartCoroutine(FadeOutSlot(leftSlot));
        yield return StartCoroutine(FadeOutSlot(rightSlot));

        // 2️⃣ 清空重量
        leftWeight = 0f;
        rightWeight = 0f;

        // 3️⃣ 播吐纸条动画
        if (spitAnimator != null)
        {
            spitAnimator.Play("BalancePuzzle_PapersOut", 0, 0f);

            // 等待动画播完
            AnimatorStateInfo stateInfo = spitAnimator.GetCurrentAnimatorStateInfo(0);
            float animLength = stateInfo.length;

            yield return new WaitForSeconds(animLength);
        }

        // 4️⃣ UI文字淡入
        if (textCanvasGroup != null)
        {
            textCanvasGroup.alpha = 0f; // 确保从0开始
            yield return StartCoroutine(FadeInCanvasGroup(textCanvasGroup, 0.5f));
        }

        isConsuming = false;
    }


    void PlaySpitAnimation()
    {
        if (spitAnimator == null) return;

        // 直接播放吐纸条动画
        spitAnimator.Play("BalancePuzzle_PapersOut", 0, 0f);
    }
    
    IEnumerator FadeInCanvasGroup(CanvasGroup cg, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }
        cg.alpha = 1f;
    }


    // 淡出整个槽位
    System.Collections.IEnumerator FadeOutSlot(BalanceSlot slot)
    {
        if (slot == null) yield break;

        List<GameObject> toRemove = new List<GameObject>();

        for (int i = 0; i < slot.transform.childCount; i++)
            toRemove.Add(slot.transform.GetChild(i).gameObject);

        foreach (var obj in toRemove)
        {
            yield return StartCoroutine(FadeOutObject(obj));
            Destroy(obj);
        }
    }

    // 淡出单个物体（UI 或 SpriteRenderer）
    System.Collections.IEnumerator FadeOutObject(GameObject obj)
    {
        float t = 0f;

        // UI
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null && obj.GetComponent<UnityEngine.UI.Graphic>() != null)
            cg = obj.AddComponent<CanvasGroup>();

        // Sprite
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t / fadeDuration);

            if (cg) cg.alpha = a;
            if (sr)
            {
                Color c = sr.color;
                c.a = a;
                sr.color = c;
            }

            yield return null;
        }
    }

    // 可选：直接加上重量操作函数
    public void SetLeftWeight(float value) => leftWeight = value;
    public void SetRightWeight(float value) => rightWeight = value;
    public void AddLeftWeight(float value) => leftWeight += value;
    public void AddRightWeight(float value) => rightWeight += value;
}
