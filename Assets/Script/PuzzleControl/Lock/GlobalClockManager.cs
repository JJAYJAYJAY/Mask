using System.Collections.Generic;
using UnityEngine;

public class GlobalClockManager:MonoBehaviour
{
    public List<LockManager> unlockmanagers =  new List<LockManager>();
    public List<LockManager> lockmanagers =  new List<LockManager>();
    
    public GameObject SpeakEnd;
    public GameObject ChaosEnd;
    public GameObject SoulEnd;
    public GameObject CompleteEnd;
    public GameObject NormalEnd;
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
            List<ItemData> itemDatas = new List<ItemData>();
            foreach (var item in Inventory.Instance.items)
            {
                if (item.type == ItemType.Mask)
                {
                    itemDatas.Add(item);
                }
            }

            if (itemDatas.Count == 0)
            {
                SoulEnd.SetActive(true);
                return;
            }
            if (itemDatas.Count == 1)
            {
                NormalEnd.SetActive(true);
                return;
            }
            if (itemDatas.Count < 5)
            {
                ChaosEnd.SetActive(true);
                return;
            }

            if (itemDatas.Count == 5)
            {
                if (GameManager.Instance.globalRuleData.SpecialEnd)
                {
                    SpeakEnd.SetActive(true);
                    return;
                }
                CompleteEnd.SetActive(true);
            }
            
            //normal 一个面具
            // soul 没有面具
            // Chaos 多个面具
            // complete所有面具
            // speaker 穿孔+全部面具
        }
    }
}
