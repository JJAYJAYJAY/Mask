public class SimpleStyleBuff:Buff
{
    public SimpleStyleBuff(BuffMetadata meta) : base(meta){}

    public override void Apply(GlobalRuleData data)
    {
        data.SimplifySudoku = true;
    }

    public override void Remove(GlobalRuleData data)
    {
        data.SimplifySudoku = false;
    }
    
}
