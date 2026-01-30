using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseInteractable : MonoBehaviour
{
    protected Material mat;
    protected Color originalColor;

    public Color hoverColor = Color.yellow;

    protected virtual void Start()
    {
        Renderer r = GetComponent<Renderer>();
        if (r != null)
        {
            mat = r.material;
            originalColor = mat.color;
        }
    }

    protected virtual void OnMouseEnter()
    {
        Debug.Log("OnMouseEnter");
        if (IsPointerBlocked()) return;
        if (mat != null)
            mat.color = hoverColor;
    }

    protected virtual void OnMouseExit()
    {
        if (mat != null)
            mat.color = originalColor;
    }

    protected virtual void OnMouseDown()
    {
        if (IsPointerBlocked()) return;
        if (GameState.IsInDetailView) return;

        OnInteract();
    }

    /// <summary>
    /// 子类必须实现的交互逻辑
    /// </summary>
    protected abstract void OnInteract();

    protected bool IsPointerBlocked()
    {
        return EventSystem.current != null &&
               EventSystem.current.IsPointerOverGameObject();
    }
}