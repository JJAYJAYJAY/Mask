using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;
    private List<Buff> buffs = new();

    void Awake()
    {
        Instance = this;
    }

    public void AddBuff(Buff buff)
    {
        buff.Apply(GameManager.Instance.Data);
        buffs.Add(buff);
    }

    public void RemoveBuff(Buff buff)
    {
        buff.Remove(GameManager.Instance.Data);
        buffs.Remove(buff);
    }

    // void Debug()
    // {
    //     //加入所有buff
    //     AddBuff(new CompositeStripesBuff());
    //     AddBuff(new DeepScratchesBuff());
    //     AddBuff(new FluorescentPaintBuff());
    //     AddBuff(new GreedScarBuff());
    //     AddBuff(new ObviousBuff());
    //     AddBuff(new SimpleStyleBuff());
    //     AddBuff(new SolidColorPendantBuff());
    //     AddBuff(new StrengthCoatingBuff());
    // }
}

public static class BuffFactory
{
    public static Buff Create(BuffMetadata meta)
    {
        switch (meta.name)
        {
            case "ReduceCost":
                return new CompositeStripesBuff(meta);

            // 以后继续加
        }

        Debug.LogError($"Unknown buff meta: {meta.name}");
        return null;
    }
}
