
public abstract class Buff
{
    public BuffMetadata Meta { get; }

    protected Buff(BuffMetadata meta)
    {
        Meta = meta;
    }
    public abstract void Apply(GlobalRuleData data);
    public abstract void Remove(GlobalRuleData data);
}