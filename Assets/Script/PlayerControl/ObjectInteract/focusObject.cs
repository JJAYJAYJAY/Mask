using UnityEngine;

public class focusObject:BaseInteractable
{
    public enum Type
    {
        Focus,
        Description,
        ShowOther
    }
    [Header("Type")]
    public Type type;
    [Header("Focus Parameter")]
    public DetailPanelController panel;
    public FocusPanelController focusPanel;
    public Sprite focusIcon;
    
    [Header("Show Description")]
    public string word;
    
    [Header("Show Other")]
    public GameObject otherObject;
    protected override void OnInteract()
    {
        switch (type)
        {
            case Type.Focus:
                focusPanel.image.sprite = focusIcon;
                panel.OpenFromWorldPos(transform.position);
                break;
            case Type.Description:
                GameManager.Instance.showText(word);
                break;
            case Type.ShowOther:
                this.otherObject.SetActive(true);
                this.gameObject.SetActive(false);
                break;
        }
        
    }
}
