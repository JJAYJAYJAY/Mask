
using UnityEngine;
using UnityEngine.UI;
public class FocusPanelController: MonoBehaviour
{
    public ItemData itemData;
    public Image image;
    public Button getButton;
    public InteractObject interactObject;
    void Start()
    {
        getButton.onClick.AddListener(Onclick);
    }
    public void Onclick()
    {
        Inventory.Instance.AddItem(itemData);
        interactObject.gameObject.SetActive(false);
        interactObject.panel.CloseToLast();
    }
}
