public class EndBuff: Buff
{
    public EndBuff(BuffMetadata meta) : base(meta){}
 
    public override void Apply(GlobalRuleData data)
    {
        data.SpecialEnd=true;
    }
 
    public override void Remove(GlobalRuleData data)
    {
        data.SpecialEnd=false;
    }
}