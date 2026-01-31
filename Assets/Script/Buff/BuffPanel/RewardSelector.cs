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
        List<RewardOption> options = new();

        // 1️⃣ 随机面具 → 降级
        if (pool.HasMask())
            options.Add(CreateMaskOption());
        else if (pool.HasBuff())
            options.Add(CreateBuffOption());
        else
            options.Add(CreateDoNothing());

        // 2️⃣ 随机能力 → 降级
        if (pool.HasBuff())
            options.Add(CreateBuffOption());
        else
            options.Add(CreateDoNothing());

        // 3️⃣ 永远存在
        options.Add(CreateDoNothing());

        return options;
    }

    RewardOption CreateMaskOption()
    {
        return new RewardOption
        {
            type = RewardOptionType.RandomMask,
            displayName = "随机面具",
            OnSelect = () =>
            {
                var mask = pool.RandomMask();
                if (mask == null) return;

                Inventory.Instance.AddItem(mask);
                pool.RebuildPools();
            }
        };
    }


    RewardOption CreateBuffOption()
    {
        return new RewardOption
        {
            type = RewardOptionType.RandomBuff,
            displayName = "随机能力",
            OnSelect = () =>
            {
                var buff = pool.RandomBuff();
                BuffManager.Instance.AddBuff(BuffFactory.Create(buff));
            }
        };
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