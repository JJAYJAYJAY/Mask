using System.Collections.Generic;

public class RewardRandomPool
{
    List<ItemData> allObjects;
    List<BuffMetadata> allBuffs;

    List<ItemData> maskPool = new();
    List<BuffMetadata> buffPool = new();

    public RewardRandomPool(List<ItemData> objects, List<BuffMetadata> buffs)
    {
        allObjects = objects;
        allBuffs = buffs;
        RebuildPools();
    }

    // ===== 对外接口（你第 3 条）=====
    public void RebuildPools()
    {
        RebuildMaskPool();
        RebuildBuffPool();
    }

    // ===== 面具池 =====
    void RebuildMaskPool()
    {
        maskPool.Clear();
        foreach (var obj in allObjects)
        {
            if (obj.type == ItemType.Mask &&
                !Inventory.Instance.HasItem(obj))
            {
                maskPool.Add(obj);
            }
        }
    }

    public bool HasMask() => maskPool.Count > 0;

    public ItemData RandomMask()
    {
        if (maskPool.Count == 0) return null;
        return maskPool[UnityEngine.Random.Range(0, maskPool.Count)];
    }

    // 当获得面具时由外部调用
    public void OnMaskAcquired(ItemData mask)
    {
        Inventory.Instance.AddItem(mask);
        RebuildPools();
    }

    // ===== Buff 池 =====
    void RebuildBuffPool()
    {
        buffPool.Clear();
        foreach (var buff in allBuffs)
        {
            // belong = 面具物品 id
            var maskMeta = allObjects.Find(o => o.MaskType == buff.belong);
            if (maskMeta != null && Inventory.Instance.HasItem(maskMeta))
            {
                buffPool.Add(buff);
            }
        }
    }

    public bool HasBuff() => buffPool.Count > 0;

    public BuffMetadata RandomBuff()
    {
        if (buffPool.Count == 0) return null;
        return buffPool[UnityEngine.Random.Range(0, buffPool.Count)];
    }
}