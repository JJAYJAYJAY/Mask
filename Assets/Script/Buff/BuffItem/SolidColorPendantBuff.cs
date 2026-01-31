
public class SolidColorPendantBuff: Buff
{
    public SolidColorPendantBuff(BuffMetadata meta) : base(meta){}

    public override void Apply(GlobalRuleData data)
    {
        data.ShowHintText = true;
    }

    public override void Remove(GlobalRuleData data)
    {
        data.ShowHintText = false;
    }
}
