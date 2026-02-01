using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePuzzleManager : MonoBehaviour
{
    [Range(0, 23)] public int hour;
    [Range(0, 59)] public int minute;

    // 时间变化事件
    public event Action<int, int> OnTimeChanged;

    private void Awake()
    {
       
    }

    // 设置时间（UI/闹钟调用）
    public void SetTime(int newHour, int newMinute)
    {
        hour = Mathf.Clamp(newHour, 0, 23);
        minute = Mathf.Clamp(newMinute, 0, 59);

        // 广播事件
        OnTimeChanged?.Invoke(hour, minute);
    }

    // 快捷增加时间（可选，动画用）
    public void AddMinutes(int delta)
    {
        int totalMinutes = hour * 60 + minute + delta;
        totalMinutes = (totalMinutes + 24 * 60) % (24 * 60);
        hour = totalMinutes / 60;
        minute = totalMinutes % 60;
        OnTimeChanged?.Invoke(hour, minute);
    }
}
