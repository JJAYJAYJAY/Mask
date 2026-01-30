using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{
    public Transform startView;
    public Transform endView;

    public IEnumerator MoveDown(float distance, float duration)
    {
        float t = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.down * distance;

        while (t < duration)
        {
            t += Time.deltaTime;
            float k = t / duration;

            transform.position = Vector3.Lerp(startPos, targetPos, k);
            yield return null;
        }

        transform.position = targetPos;
    }


    public void TeleportToMainView()
    {
        transform.position = endView.position;
        transform.rotation = endView.rotation;
    }
}