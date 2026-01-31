using UnityEngine;

public class DetailObject : BaseInteractable
{
    public DetailPanelController panel;

    protected override void OnInteract()
    {
        panel.OpenFromWorldPos(transform.position);
    }

    public void OnBackButton()
    {
        panel.CloseToWorldPos(transform.position);
    }
}