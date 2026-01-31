using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClockScrollControl : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public MainClockControl mainClockControl;

    private RectTransform rectTransform;
    private Canvas canvas;
    private float startAngle;
    private float initialRotation;
    
    public int index;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        
        // 开启精确点击判定（图片需开启 Read/Write）
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 计算初始角度
        startAngle = GetAngle(eventData.position);
        // 记录按下时的旋转角度
        initialRotation = rectTransform.localEulerAngles.z;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float currentAngle = GetAngle(eventData.position);
        float angleDifference = currentAngle - startAngle;

        // 更新 UI 旋转
        rectTransform.localEulerAngles = new Vector3(0, 0, initialRotation + angleDifference);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 自动对齐逻辑 (例如每 30 度一格)
        float finalAngle = rectTransform.localEulerAngles.z;
        float snappedAngle = Mathf.Round(finalAngle / 30f) * 30f;
        rectTransform.localEulerAngles = new Vector3(0, 0, snappedAngle);

        // 检测谜题是否解开
        CheckSolution();
    }

    private float GetAngle(Vector2 mousePos)
    {
        // 获取 UI 中心点的屏幕坐标
        Vector2 centerPos = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
        Vector2 dir = mousePos - centerPos;
        // 计算鼠标相对于中心的角度
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    private void CheckSolution()
    {
        mainClockControl.CheckMultiPointPuzzle();
    }
}
