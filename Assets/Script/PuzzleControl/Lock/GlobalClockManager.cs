using System.Collections.Generic;
using UnityEngine;

public class GlobalClockManager:MonoBehaviour
{
    List<LockManager> unlockmanagers =  new List<LockManager>();
    List<LockManager> lockmanagers =  new List<LockManager>();
    
    public static GlobalClockManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Unlock(LockManager lockmanager)
    {
        unlockmanagers.Add(lockmanager);
        lockmanagers.Remove(lockmanager);
        CheckEnd();
    }

    public void BreakRandomLock()
    {
        int index = Random.Range(0, lockmanagers.Count);
        lockmanagers[index].Onsuccess();
        //移除加入
        unlockmanagers.Add(lockmanagers[index]);
        lockmanagers.RemoveAt(index);
    }

    public void CheckEnd()
    {
        if (lockmanagers.Count == 0)
        {
            
        }
    }
}
