public class CompositeStripesBuff: Buff
{
    public CompositeStripesBuff(BuffMetadata meta) : base(meta) {}

    public override void Apply(GlobalRuleData data)
    {
        data.breakMaskLimits = true;
    }

    public override void Remove(GlobalRuleData data)
    {
        data.breakMaskLimits = false;
    }
}