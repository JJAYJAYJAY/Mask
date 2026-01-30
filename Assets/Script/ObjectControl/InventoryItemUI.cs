using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData item;
    public Image icon;

    RectTransform rect;
    Canvas canvas;
    RectTransform inventoryRect;

    Vector2 startPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        inventoryRect = transform.parent.GetComponent<RectTransform>();
    }

    public void Init(ItemData data)
    {
        item = data;
        icon.sprite = data.icon;
        
        RectTransform rt = icon.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(data.width, data.height);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rect.anchoredPosition;
        transform.SetAsLastSibling(); // 拖到最上层
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        ClampToInventory();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ClampToInventory();
    }

    void ClampToInventory()
    {
        Vector2 min = inventoryRect.rect.min - rect.rect.min;
        Vector2 max = inventoryRect.rect.max - rect.rect.max;

        Vector2 pos = rect.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        rect.anchoredPosition = pos;
    }
}