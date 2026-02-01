using UnityEngine;

public class InteractObject:BaseInteractable
{
    public enum Type
    {
        Focus,
        Description,
        ShowOther
    }
    [Header("Type")]
    public Type type;

    public ItemData data;
    [Header("Focus Parameter")]
    public DetailPanelController panel;
    public FocusPanelController focusPanel;
    public Sprite focusIcon;
    public bool canGet;
    
    [Header("Show Other")]
    public GameObject otherObject;
    protected override void OnInteract()
    {
        switch (type)
        {
            case Type.Focus:
                focusPanel.image.sprite = focusIcon;
                focusPanel.itemData = data;
                focusPanel.interactObject = this;
                focusPanel.image.preserveAspect = true;
                focusPanel.getButton.gameObject.SetActive(canGet);
                RectTransform rt = focusPanel.image.rectTransform;
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                panel.OpenFromWorldPos(transform.position);
                break;
            case Type.Description:
                if(data) GameManager.Instance.showText(data.description);
                break;
            case Type.ShowOther:
                this.otherObject.SetActive(true);
                this.gameObject.SetActive(false);
                break;
        }
        
    }
}
