public class StrengthCoatingBuff: Buff
{
    public StrengthCoatingBuff(BuffMetadata meta) : base(meta){}

    public override void Apply(GlobalRuleData data)
    {
        data.CanBreakLocks=true;
    }

    public override void Remove(GlobalRuleData data)
    {
        data.CanBreakLocks=false;
    }
}