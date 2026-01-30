using System;
using UnityEngine;

[Serializable]
public class TimeEvent
{
    public int targetHour;
    public int targetMinute;
    public GameObject targetObject; // 要操作的物体
    public bool activeState = true; // 到时间显示或隐藏
}

public class TimeReactiveObject : MonoBehaviour
{
    public TimeEvent[] events;

    private void OnEnable()
    {
        TimePuzzleManager.Instance.OnTimeChanged += HandleTimeChanged;
        HandleTimeChanged(TimePuzzleManager.Instance.hour, TimePuzzleManager.Instance.minute); // 初始化
    }

    private void OnDisable()
    {
        if (TimePuzzleManager.Instance != null)
            TimePuzzleManager.Instance.OnTimeChanged -= HandleTimeChanged;
    }

    private void HandleTimeChanged(int hour, int minute)
    {
        foreach (var te in events)
        {
            if (hour == te.targetHour && minute == te.targetMinute)
            {
                te.targetObject.SetActive(te.activeState);
            }
            else
            {
                te.targetObject.SetActive(!te.activeState); // 可选：没到时间反向隐藏
            }
        }
    }
}