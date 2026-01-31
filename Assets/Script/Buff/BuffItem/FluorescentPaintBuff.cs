
public class FluorescentPaintBuff: Buff
{
    public FluorescentPaintBuff(BuffMetadata meta) : base(meta) {}

    public override void Apply(GlobalRuleData data)
    {
        data.HighlightKeyItems = true;
    }

    public override void Remove(GlobalRuleData data)
    {
        data.HighlightKeyItems = false;
    }
}
