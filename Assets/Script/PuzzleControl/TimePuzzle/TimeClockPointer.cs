using UnityEngine;
using UnityEngine.EventSystems;

public class TimeClockPointer : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public enum PointerType { Hour, Minute }
    public PointerType pointerType;

    public RectTransform clockCenter; // 闹钟中心点
    private Vector2 centerScreenPos;
    
    public TimePuzzleManager timePuzzleManager;
    private void Start()
    {
        centerScreenPos = RectTransformUtility.WorldToScreenPoint(null, clockCenter.position);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        UpdatePointer(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdatePointer(eventData);
    }

    private void UpdatePointer(PointerEventData eventData)
    {
        Vector2 pointerPos = eventData.position;
        Vector2 dir = pointerPos - centerScreenPos;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 90f; // 因为指针初始是竖直向上
        if (angle < 0) angle += 360f;

        // 转针
        transform.localEulerAngles = new Vector3(0, 0, angle);

        // 更新时间
        if (timePuzzleManager == null) return;

        int hour = timePuzzleManager.hour;
        int minute = timePuzzleManager.minute;

        if (pointerType == PointerType.Minute)
        {
            minute = Mathf.RoundToInt(angle / 360f * 60f) % 60;
        }
        else if (pointerType == PointerType.Hour)
        {
            hour = Mathf.RoundToInt(angle / 360f * 12f) % 12;
        }

        timePuzzleManager.SetTime(hour, minute);
    }
}