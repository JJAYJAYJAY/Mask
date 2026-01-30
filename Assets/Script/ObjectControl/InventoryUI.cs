using UnityEngine;
public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public RectTransform itemsRoot;
    public InventoryItemUI itemPrefab;
    bool isOpen = false;
    DetailPanelController detailPanel;
    void Awake()
    {
        Instance = this;
        detailPanel = GetComponent<DetailPanelController>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        Debug.Log(isOpen);
        if (!isOpen)
        {
            detailPanel.OpenFromWorldPos(Vector3.zero);
            isOpen = true;
        }
        else
        {
            detailPanel.CloseToWorldPos(Vector3.zero);
            isOpen = false;
        }
    }


    public void AddItem(ItemData item)
    {
        var ui = Instantiate(itemPrefab, itemsRoot);
        ui.Init(item);

        ui.GetComponent<RectTransform>().anchoredPosition = GetRandomPos();
    }

    public void RemoveItem(ItemData item)
    {
        
        foreach (Transform child in itemsRoot)
        {
            var ui = child.GetComponent<InventoryItemUI>();
            if (ui != null && ui.item == item)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    Vector2 GetRandomPos()
    {
        Rect r = itemsRoot.rect;

        float x = Random.Range(r.xMin + 80, r.xMax - 80);
        float y = Random.Range(r.yMin + 80, r.yMax - 80);

        return new Vector2(x, y);
    }
}