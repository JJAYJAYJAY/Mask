using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BalanceSlot : MonoBehaviour, IDropHandler
{
    public enum Side { Left, Right }
    public Side side;

    public BalanceControl balance;
    public DetailPanelController detailPanel;

    private void Start()
    {
        detailPanel.OnClosed+=OnDetailPanelClosed;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        var item = eventData.pointerDrag?.GetComponent<SidebarItem>();
        if (item == null) return;
        item.transform.position = eventData.position;
        // 放到天平槽位
        if (item.GetOriginalParent() != transform)
        {
            item.transform.SetParent(transform);
            item.removeFromInventory();
            // 加权重
            if (side == Side.Left)
                balance.AddLeftWeight(item.data.weight);
            else
                balance.AddRightWeight(item.data.weight);
        }
    }
    
    public void removeItem(Side fromSide, float weight)
    {
        if (fromSide == Side.Left)
            balance.AddLeftWeight(-weight);
        else
            balance.AddRightWeight(-weight);
    }
    
    //关闭界面时候返回所有物品
    private void OnDetailPanelClosed()
    {
        foreach (Transform child in transform)
        {
            var item = child.GetComponent<SidebarItem>();
            if (item != null)
            {
                // 移除权重
                removeItem(side, item.data.weight);
                // 返回原位置
                item.ReturnToInventory();
                Destroy(child.gameObject);
            }
        }
    }
}