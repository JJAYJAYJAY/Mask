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

    void BreakRandomLocks()
    {
        int index = Random.Range(0, lockmanagers.Count);
    }
}
