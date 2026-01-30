using System.Collections;
using UnityEngine;

public class DetailPanelController : MonoBehaviour
{
    public float animSpeed = 5f;

    private CanvasGroup group;
    private Vector3 originScale;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        originScale = transform.localScale;
        transform.localScale = Vector3.zero;
        group.alpha = 0f;
        gameObject.SetActive(false);
    }

    public void OpenFromWorldPos(Vector3 worldPos)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(Open(worldPos));
    }

    public void CloseToWorldPos(Vector3 worldPos)
    {
        if (!gameObject.activeSelf) return;

        StopAllCoroutines();
        StartCoroutine(Close(worldPos));
    }

    IEnumerator Open(Vector3 worldPos)
    {
        GameState.IsInDetailView = true;
        GameManager.Instance.SetBlocker(true);

        Vector3 start = Camera.main.WorldToScreenPoint(worldPos);
        Vector3 center = new(Screen.width / 2f, Screen.height / 2f, 0f);

        gameObject.SetActive(true);
        transform.position = start;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * animSpeed;
            float p = Mathf.Clamp01(t);

            transform.position = Vector3.Lerp(start, center, p);
            transform.localScale = Vector3.Lerp(Vector3.zero, originScale, p);
            group.alpha = p;

            yield return null;
        }
    }

    IEnumerator Close(Vector3 worldPos)
    {
        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;
        float startAlpha = group.alpha;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * animSpeed;
            float p = Mathf.SmoothStep(0, 1, t);

            Vector3 target = Camera.main.WorldToScreenPoint(worldPos);
            transform.position = Vector3.Lerp(startPos, target, p);
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, p);
            group.alpha = Mathf.Lerp(startAlpha, 0f, p);

            yield return null;
        }

        gameObject.SetActive(false);
        GameState.IsInDetailView = false;
        GameManager.Instance.SetBlocker(false);
    }
}
