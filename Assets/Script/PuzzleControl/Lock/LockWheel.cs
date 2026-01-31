using UnityEngine;
using UnityEngine.EventSystems;

public class LockWheel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform content;
    public float itemHeight = 100f;
    public int digitCount = 10;
    public LockManager manager;

    private float initialY; 
    public int CurrentValue { get; private set; } = 0;

    void Awake()
    {
        // 记录面板静止在数字 '0' 时的初始坐标
        initialY = content.anchoredPosition.y;
    }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        // 允许自由拖动
        Vector2 pos = content.anchoredPosition;
        pos.y += eventData.delta.y;

        content.anchoredPosition = pos;
        LoopContent();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapToNearest();
    }
    
    void SnapToNearest()
    {
        // 1️⃣ 当前相对偏移
        float relativeY = content.anchoredPosition.y - initialY;

        // 2️⃣ 计算最近的格子索引（允许无限）
        int rawIndex = Mathf.RoundToInt(relativeY / itemHeight);

        // 3️⃣ 吸附到该格子
        float targetY = initialY + rawIndex * itemHeight;
        content.anchoredPosition = new Vector2(
            content.anchoredPosition.x,
            targetY
        );

        // 4️⃣ 映射为 0-9 的数值
        CurrentValue = ((rawIndex % digitCount) + digitCount) % digitCount;
    }

    
    void LoopContent()
    {
        float totalHeight = itemHeight * digitCount;
        Vector2 pos = content.anchoredPosition;

        // 5. 循环判断也要基于初始偏移量
        if (-(initialY - pos.y) > totalHeight) 
            pos.y -= totalHeight;
        else if (-(initialY - pos.y) < 0)
            pos.y += totalHeight;

        content.anchoredPosition = pos;
    }
}