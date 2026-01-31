
public class DeepScratchesBuff:Buff
{
    public DeepScratchesBuff(BuffMetadata meta) : base(meta) {}
    public override void Apply(GlobalRuleData data)
    {
        data.ShowCorrectDigits = true;
    }

    public override void Remove(GlobalRuleData data)
    {
        data.ShowCorrectDigits = false;
    }
}
