public class ObviousBuff: Buff
{
    public ObviousBuff(BuffMetadata meta) : base(meta){}

    public override void Apply(GlobalRuleData data)
    {
        //TODO: 随机破坏一个
    }

    public override void Remove(GlobalRuleData data)
    {
    }
}