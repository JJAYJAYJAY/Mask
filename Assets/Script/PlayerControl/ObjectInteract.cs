using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    public GameObject detailPanel;      // 拖入刚才创建的 Detail_View
    private CanvasGroup panelGroup;
    private Material mat;
    private Color originalColor;
    // 初始面板大小
    private Vector3 originScale;
    public Color hoverColor = Color.yellow; // 悬停时的颜色
    public float animSpeed = 5f;        // 展开速度

    void Start()
    {
        originScale = detailPanel.transform.localScale;
        mat = GetComponent<Renderer>().material;
        originalColor = mat.color;
        panelGroup = detailPanel.GetComponent<CanvasGroup>();
        detailPanel.transform.localScale = Vector3.zero;
        panelGroup.alpha = 0;
        detailPanel.SetActive(false);
    }
    void OnMouseEnter()
    {
        // 只有当鼠标没被 UI 挡住时才触发
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            mat.color = hoverColor;
            // 你也可以在这里加入改变鼠标指针形状的代码
            // Cursor.SetCursor(hoverTexture, Vector2.zero, CursorMode.Auto);
        }
    }

    void OnMouseExit()
    {
        mat.color = originalColor; // 恢复原色
    }
    
    void OnMouseDown()
    {
        if (GameState.IsInDetailView) return;
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            StartCoroutine(OpenPanel());
        }
    }

    IEnumerator OpenPanel()
    {
        GameState.IsInDetailView = true;
        GameManager.Instance.SetBlocker(true); // 启用全屏遮罩，阻止其他交互
        // 1. 准备坐标数据
        Vector3 startScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 centerScreenPos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

        // 2. 初始化 UI 状态
        detailPanel.SetActive(true);
        detailPanel.transform.position = startScreenPos; // 初始位置在物体处
        detailPanel.transform.localScale = Vector3.zero;  // 初始大小为 0
        panelGroup.alpha = 0f;

        float elapsed = 0f;
        float duration = 0.5f; // 动画持续时间（秒），可以根据需求调整

        // 3. 动画循环 (推荐使用百分比平滑插值)
        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime * animSpeed;
            float percent = Mathf.Clamp01(elapsed); 

            // 核心：三合一插值
            // 位置：从物体位置移动到屏幕中心
            detailPanel.transform.position = Vector3.Lerp(startScreenPos, centerScreenPos, percent);
        
            // 缩放：从 0 放大到 1
            detailPanel.transform.localScale = Vector3.Lerp(Vector3.zero, originScale, percent);
        
            // 透明度：从 0 增加到 1
            panelGroup.alpha = Mathf.Lerp(0f, 1f, percent);

            yield return null;
        }

        // 4. 确保最终状态准确
        detailPanel.transform.position = centerScreenPos;
        detailPanel.transform.localScale = originScale;
        panelGroup.alpha = 1f;
    }

    // 记得在 UI 上加一个“返回”按钮调用这个
    public void ClosePanel()
    {
        StartCoroutine(HidePanel());
    }

    public void OnBackButtonClicked()
    {
        // 防止重复点击导致协程叠加
        StopAllCoroutines(); 
        StartCoroutine(HidePanel());
    }

    IEnumerator HidePanel()
    {
        // 1. 记录动画起始状态（当前在画面中央的状态）
        Vector3 startScreenPos = detailPanel.transform.position;
        Vector3 startScale = detailPanel.transform.localScale;
        float startAlpha = panelGroup.alpha;

        float elapsed = 0f;

        // 2. 动画循环
        // 使用 while (elapsed < 1.0f) 配合动画曲线比 Distance 判断更精准、丝滑
        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime * animSpeed;
            // 使用 SmoothStep 让收回动作更有质感（先慢后快再慢）
            float t = Mathf.SmoothStep(0, 1, elapsed);

            // 3. 实时计算物体当前的屏幕位置 (防止动画期间摄像机微动)
            Vector3 targetObjectPos = Camera.main.WorldToScreenPoint(transform.position);

            // 4. 执行“三合一”逆向插值
            // 位置：从中央收回到物体点
            detailPanel.transform.position = Vector3.Lerp(startScreenPos, targetObjectPos, t);
        
            // 缩放：从当前大小缩小到 0
            detailPanel.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
        
            // 透明度：从当前透明度淡出到 0
            panelGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);

            yield return null;
        }

        // 5. 确保状态彻底归零并关闭
        detailPanel.transform.localScale = Vector3.zero;
        panelGroup.alpha = 0f;
        detailPanel.SetActive(false);
        GameState.IsInDetailView = false;
        GameManager.Instance.SetBlocker(false); // 关闭全屏遮罩，恢复交互
    }
}
