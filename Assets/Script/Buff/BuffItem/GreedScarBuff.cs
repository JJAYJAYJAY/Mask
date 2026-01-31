public class GreedScarBuff: Buff
{
    public GreedScarBuff(BuffMetadata meta) : base(meta) {}

    public override void Apply(GlobalRuleData data)
    {
        data.CanFlash = true;
    }

    public override void Remove(GlobalRuleData data)
    {
        data.CanFlash = false;
    }
}