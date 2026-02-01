using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 奖励池管理（面具 + Buff）
/// </summary>
public class RewardRandomPool
{
    // ===== 全局数据 =====
    private List<ItemData> allObjects;
    private List<BuffMetadata> allBuffs;

    // ===== 当前可选池 =====
    private List<ItemData> maskPool;
    private List<BuffMetadata> buffPool;

    public RewardRandomPool(List<ItemData> objects, List<BuffMetadata> buffs)
    {
        allObjects = objects;
        allBuffs = buffs;
        maskPool = new List<ItemData>();
        buffPool = new List<BuffMetadata>();
        RebuildPools();
        
    }

    // ===== 对外接口：重建池 =====
    public void RebuildPools()
    {
        RebuildMaskPool();
        RebuildBuffPool();
    }

    // ===== 面具池 =====
    private void RebuildMaskPool()
    {
        maskPool.Clear();
        foreach (var obj in allObjects)
        {
            if (obj.type == ItemType.Mask && !Inventory.Instance.HasItem(obj))
            {
                maskPool.Add(obj);
            }
        }
    }

    public bool HasMask() => maskPool.Count > 0;

    /// <summary>
    /// 随机返回一个面具（不修改池）
    /// </summary>
    public ItemData RandomMask()
    {
        if (maskPool.Count == 0) return null;
        return maskPool[Random.Range(0, maskPool.Count)];
    }

    // ===== Buff 池 =====
    private void RebuildBuffPool()
    {
        buffPool.Clear();
        foreach (var buff in allBuffs)
        {
            // belong = 面具物品 id
            var maskMeta = allObjects.Find(o => o.MaskType == buff.belong);
            if ((GameManager.Instance.globalRuleData.BreakMaskLimits)||(maskMeta != null && Inventory.Instance.HasItem(maskMeta) && !BuffManager.Instance.HasBuff(buff)))
            {
                buffPool.Add(buff);
            }
        }
    }

    public bool HasBuff(int count) => buffPool.Count > count;

    /// <summary>
    /// 单次随机 Buff（可能重复）
    /// </summary>
    public BuffMetadata RandomBuff()
    {
        if (buffPool.Count == 0) return null;
        return buffPool[Random.Range(0, buffPool.Count)];
    }

    /// <summary>
    /// 一次性获取 N 个 Buff，保证不重复
    /// </summary>
    public List<BuffMetadata> RandomBuffs(int count)
    {
        var tempPool = new List<BuffMetadata>(buffPool); // 临时池
        var result = new List<BuffMetadata>();

        count = Mathf.Min(count, tempPool.Count); // 避免数量过多
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, tempPool.Count);
            result.Add(tempPool[index]);
            tempPool.RemoveAt(index); // 无放回
        }

        return result;
    }
    
    //打印buffpool
    public void PrintBuffPool()
    {
        Debug.Log("Current Buff Pool:");
        foreach (var buff in buffPool)
        {
            Debug.Log(buff.buffName);
        }
    }
    public void OnMakeLimitsChanged(bool flag)
    {
        RebuildBuffPool();
    }
}
