using UnityEngine;

public class SidebarPanel : MonoBehaviour
{
    public GameObject sidebar;
    public GameObject itemPrefab;
    public DetailPanelController detailPanelController;
    void Start()
    {
        detailPanelController.OnOpened += Onopen;
        detailPanelController.OnClosed += Onclose;
    }

    void Onopen()
    {
        var items = Inventory.Instance.items;

        for (int i = 0; i < items.Count; i++)
        {
            GameObject go = Instantiate(itemPrefab, sidebar.transform);
            go.GetComponentInChildren<SidebarItem>().Init(items[i]);
        }
    }
    
    void Onclose()
    {
        foreach (Transform child in sidebar.transform)
        {
            Destroy(child.gameObject);
        }
    }
}