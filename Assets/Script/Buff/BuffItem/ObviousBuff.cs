public class ObviousBuff: Buff
{
    public ObviousBuff(BuffMetadata meta) : base(meta){}

    public override void Apply(GlobalRuleData data)
    {
        GlobalClockManager.Instance.BreakRandomLock();
    }

    public override void Remove(GlobalRuleData data)
    {
    }
}