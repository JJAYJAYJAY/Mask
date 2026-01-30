using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;      // 拖拽前父物体
    public SudokuManager sudokuManager;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private GameObject placeholder;       // 占位符

    private DetectCell originCell;              // 如果是棋盘内拖拽，记录原格子
    public int number;                     // 碎片数字
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

public void OnBeginDrag(PointerEventData eventData)
{
    
    sudokuManager.PlayPickupSound();
    
    originalParent = transform.parent;
    originCell = originalParent.GetComponent<DetectCell>();

    // 👉 从棋盘内拖拽
    if (originCell != null)
    {
        // 清空数独数据
        sudokuManager.modifyCurrentState(originCell.index, 0);
        originCell.currentPiece = null;

        // 创建占位符（稳住 GridLayout）
        placeholder = new GameObject("Placeholder");
        placeholder.transform.SetParent(originalParent);

        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        LayoutElement selfLE = GetComponent<LayoutElement>();
        if (selfLE != null)
        {
            le.preferredWidth = selfLE.preferredWidth;
            le.preferredHeight = selfLE.preferredHeight;
        }
        else
        {
            le.preferredWidth = rectTransform.rect.width;
            le.preferredHeight = rectTransform.rect.height;
        }

        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
    }

    transform.SetParent(transform.root);
    canvasGroup.blocksRaycasts = false;

    // 高亮可放置格子
    foreach (DetectCell cell in FindObjectsOfType<DetectCell>())
        cell.ShowHighlight(cell.canPlace);
}

public void OnDrag(PointerEventData eventData)
{
    rectTransform.position = eventData.position;
}

public void OnEndDrag(PointerEventData eventData)
{
    sudokuManager.PlayDropSound();
    canvasGroup.blocksRaycasts = true;

    foreach (DetectCell cell in FindObjectsOfType<DetectCell>())
        cell.HideHighlight();

    GameObject target = eventData.pointerEnter;

    // =========================
    // 1️⃣ 拖到棋盘格子
    // =========================
    DetectCell cellTarget = target ? target.GetComponent<DetectCell>() : null;
    if (cellTarget != null && cellTarget.canPlace)
    {
        // 👉 有碎片，交换
        if (cellTarget.currentPiece != null)
        {
            DragCell other = cellTarget.currentPiece;

            if (originCell != null)
            {
                // 棋盘 ↔ 棋盘 交换
                other.transform.SetParent(originCell.transform);
                other.transform.localPosition = Vector3.zero;
                originCell.currentPiece = other;

                sudokuManager.modifyCurrentState(
                    originCell.index,
                    other.number
                );
            }
            else
            {
                // 碎片区 ↔ 棋盘
                other.transform.SetParent(originalParent);
                other.transform.localPosition = Vector3.zero;
            }
        }

        // 放置当前碎片
        transform.SetParent(cellTarget.transform);
        transform.localPosition = Vector3.zero;
        cellTarget.currentPiece = this;

        sudokuManager.modifyCurrentState(
            cellTarget.index,
            number
        );

        if (placeholder != null) Destroy(placeholder);
        sudokuManager.printMatrix();
        return;
    }

    // =========================
    // 2️⃣ 拖到碎片区（任意位置）
    // =========================
    PieceArea area = null;
    Transform t = target ? target.transform : null;
    while (t != null)
    {
        area = t.GetComponent<PieceArea>();
        if (area != null) break;
        t = t.parent;
    }

    if (area != null)
    {
        transform.SetParent(area.transform);

        // ✅ 使用鼠标落点
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            area.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );
        transform.localPosition = localPoint;

        // 👉 如果来自棋盘，占位符已经存在，直接销毁
        if (placeholder != null)
            Destroy(placeholder);
        sudokuManager.printMatrix();
        return;
    }

    // =========================
    // 3️⃣ 无效位置 → 回到原位
    // =========================
    transform.SetParent(originalParent);
    transform.localPosition = Vector3.zero;

    // 👉 回到棋盘，恢复数据
    if (originCell != null)
    {
        originCell.currentPiece = this;
        sudokuManager.modifyCurrentState(
            originCell.index,
            number
        );
    }

    if (placeholder != null)
        Destroy(placeholder);
    
}
}
