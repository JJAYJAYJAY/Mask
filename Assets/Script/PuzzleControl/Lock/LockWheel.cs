using UnityEngine;
using UnityEngine.EventSystems;

public class LockWheel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform content;
    public float itemHeight = 100f;
    public int digitCount = 9;

    private float initialY; // 记录初始偏移
    public int CurrentValue { get; private set; } = 1;

    void Awake()
    {
        // 1. 初始化时记录下你说的那个 50 的偏移
        initialY = 50;
    }

    public void OnBeginDrag(PointerEventData eventData) { /* 保持不变 */ }

    public void OnDrag(PointerEventData eventData)
    {
        // 直接增加 delta 没问题
        content.anchoredPosition += Vector2.up * eventData.delta.y;
        LoopContent();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapToNearest();
    }
    
    void SnapToNearest()
    {
        // 2. 计算时减去初始偏移，得到“纯净”的位移
        float relativeY = content.anchoredPosition.y - initialY;
        
        // 注意：如果你向上滚，y 变大，index 应该是负值，所以这里用 -relativeY
        int index = Mathf.RoundToInt(-relativeY / itemHeight);

        // 3. 回弹时要把初始偏移加回来
        float targetY = initialY - (index * itemHeight);
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, targetY);

        // 4. 计算当前数值
        int finalValue = index % digitCount;
        if (finalValue < 0) finalValue += digitCount;
        
        // 如果你的滚轮是从 1 开始而不是 0，这里可能需要 +1
        CurrentValue = finalValue; 
    }

    void LoopContent()
    {
        float totalHeight = itemHeight * digitCount;
        Vector2 pos = content.anchoredPosition;

        // 5. 循环判断也要基于初始偏移量
        if (pos.y - initialY > totalHeight / 2f) 
            pos.y -= totalHeight;
        else if (pos.y - initialY < -totalHeight / 2f)
            pos.y += totalHeight;

        content.anchoredPosition = pos;
    }
}