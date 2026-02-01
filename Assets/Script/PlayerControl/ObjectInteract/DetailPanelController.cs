using System;
using System.Collections;
using UnityEngine;

public class DetailPanelController : MonoBehaviour
{
    public float animSpeed = 5f;
    public Vector3 lastOpenPosition;
    private CanvasGroup group;
    private Vector3 originScale;
    public event Action OnClosed;
    public event Action OnOpened;
    public event Action BeforeOpened;
    private static DetailPanelController currentlyOpenPanel = null;
    private Camera mainCamera;
    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("MainCamera not found!");
        }
        originScale = transform.localScale;
        transform.localScale = Vector3.zero;
        SetVisible(false);
    }
    
    public void OpenToDefault()
    {
        OpenFromWorldPos(Vector3.zero); // 或者你需要的默认位置
    }

    public void OpenFromWorldPos(Vector3 worldPos=default)
    {
        // 如果有别的 Panel 已经在开，直接忽略
        if (currentlyOpenPanel != null && currentlyOpenPanel != this)
        {
            Debug.Log("Another detail panel is already open, ignoring.");
            return;
        }

        // 设置当前正在打开的 Panel
        currentlyOpenPanel = this;
        StopAllCoroutines();
        StartCoroutine(Open(worldPos));
    }
    
    public void CloseToLast()
    {
        CloseToWorldPos(lastOpenPosition); // 或者你需要的默认位置
    }
    public void CloseToWorldPos(Vector3 worldPos=default)
    {
        StopAllCoroutines();
        StartCoroutine(Close(worldPos));
    }

    IEnumerator Open(Vector3 worldPos)
    {
        lastOpenPosition=worldPos;
        BeforeOpened?.Invoke();
        GameState.IsInDetailView = true;
        GameManager.Instance.SetBlocker(true);

        Vector3 start = mainCamera.WorldToScreenPoint(worldPos);
        Vector3 center = new(Screen.width / 2f, Screen.height / 2f, 0f);

        transform.position = start;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * animSpeed;
            float p = Mathf.Clamp01(t);

            transform.position = Vector3.Lerp(start, center, p);
            transform.localScale = Vector3.Lerp(Vector3.zero, originScale, p);
            group.alpha = p;

            yield return null;
        }
        SetVisible(true);
        OnOpened?.Invoke();
    }
    
    IEnumerator Close(Vector3 worldPos)
    {
        // 1. 记录动画起始状态（当前在画面中央的状态）
        Vector3 startScreenPos = transform.position;
        Vector3 startScale = transform.localScale;
        float startAlpha = group.alpha;

        float elapsed = 0f;

        // 2. 动画循环
        // 使用 while (elapsed < 1.0f) 配合动画曲线比 Distance 判断更精准、丝滑
        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime * animSpeed;
            // 使用 SmoothStep 让收回动作更有质感（先慢后快再慢）
            float t = Mathf.SmoothStep(0, 1, elapsed);

            // 3. 实时计算物体当前的屏幕位置 (防止动画期间摄像机微动)
            Vector3 targetObjectPos = Camera.main.WorldToScreenPoint(worldPos);

            // 4. 执行“三合一”逆向插值
            // 位置：从中央收回到物体点
            transform.position = Vector3.Lerp(startScreenPos, targetObjectPos, t);
        
            // 缩放：从当前大小缩小到 0
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
        
            // 透明度：从当前透明度淡出到 0
            group.alpha = Mathf.Lerp(startAlpha, 0f, t);

            yield return null;
        }

        // 5. 确保状态彻底归零并关闭
        transform.localScale = Vector3.zero;
        SetVisible(false);
        GameState.IsInDetailView = false;
        GameManager.Instance.SetBlocker(false); // 关闭全屏遮罩，恢复交互
        if (currentlyOpenPanel == this)
            currentlyOpenPanel = null;
        OnClosed?.Invoke();
    }
    public void SetVisible(bool visible)
    {
        group.alpha = visible ? 1f : 0f;
        group.blocksRaycasts = visible;
        group.interactable = visible;
    }
    
}
