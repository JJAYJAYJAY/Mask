using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SidebarItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData data;
    private Canvas canvas;
    private RectTransform rt;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Transform originalSlot;
    private bool droppedOnBalance = false;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        
    }

    private void Start()
    {
        originalParent = transform.parent;
        if(data) GetComponent<Image>().sprite = data.icon;
        originalSlot = transform.parent;
    }

    public void Init(ItemData item)
    {
        data = item;
        GetComponent<Image>().sprite = item.icon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        
        transform.SetParent(canvas.transform); // 提到最上层
        canvasGroup.blocksRaycasts = false;
  
    }

    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // 如果没放到任何接收器，回侧边栏
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalSlot);
            rt.anchoredPosition = Vector2.zero;
            if(originalParent!=originalSlot) addToInventory();
            BalanceSlot balanceSlot = originalParent.GetComponent<BalanceSlot>();
            if (balanceSlot != null)
            {
                balanceSlot.removeItem(balanceSlot.side, data.weight);
            }
        }
    }

    public Transform GetOriginalParent()
    {
        return originalParent;
    }
    
    public void removeFromInventory()
    {
        Inventory.Instance.RemoveItem(data);
    }
    
    public void addToInventory()
    {
        Inventory.Instance.AddItem(data);
    }
    
    public void ReturnToInventory()
    {
        addToInventory();
    }
}