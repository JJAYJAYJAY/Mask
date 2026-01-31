using System;
using System.Collections.Generic;
public enum RewardOptionType
{
    RandomMask,
    RandomBuff,
    DoNothing
}

public class RewardOption
{
    public RewardOptionType type;
    public string displayName;
    public ItemData item;
    public BuffMetadata buff;
    public Action OnSelect;
}

public class RewardSelector
{
    RewardRandomPool pool;

    public RewardSelector(RewardRandomPool pool)
    {
        this.pool = pool;
    }

    public List<RewardOption> Generate()
    {
        pool.PrintBuffPool();
        List<RewardOption> options = new();
        int count = 0;
        // 1️⃣ 随机面具 → 降级
        if (pool.HasMask())
            options.Add(CreateMaskOption());
        else if (pool.HasBuff(count))
            count++;
        else
            options.Add(CreateDoNothing());

        // 2️⃣ 随机能力 → 降级
        if (pool.HasBuff(count))
            count++;
        else
            options.Add(CreateDoNothing());
        if (count > 0)
        {
            var buffOptions = CreateBuffOption(count);
            options.AddRange(buffOptions);
        }
        // 3️⃣ 永远存在
        options.Add(CreateDoNothing());

        return options;
    }

    RewardOption CreateMaskOption()
    {
        var option = new RewardOption
        {
            type = RewardOptionType.RandomMask,
            displayName = "随机面具",
            item = pool.RandomMask(),
        };
        option.OnSelect = () =>
        {
            if (option.item == null) return;
            Inventory.Instance.AddItem(option.item);
            pool.RebuildPools();
        };
        return option;
    }


    List<RewardOption> CreateBuffOption(int count)
    {
        var buffs = pool.RandomBuffs(count);
        List<RewardOption> options = new();
        for (int i = 0; i < buffs.Count; i++)
        {
            var option = new RewardOption
            {
                type = RewardOptionType.RandomBuff,
                displayName = "随机能力",
                buff = buffs[i]
            };
            option.OnSelect = () =>
            {
                BuffManager.Instance.AddBuff(BuffFactory.Create(option.buff));
                pool.RebuildPools();
            };
            options.Add(option);
        }
       
        return options;
    }

    RewardOption CreateDoNothing()
    {
        return new RewardOption
        {
            type = RewardOptionType.DoNothing,
            displayName = "什么都不做",
            OnSelect = () => { }
        };
    }
}