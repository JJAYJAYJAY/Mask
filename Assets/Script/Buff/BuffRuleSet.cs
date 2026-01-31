using System.Collections.Generic;

public class GlobalRuleData
{
    // —— 信息显示类 ——
    public bool ShowHintText = false;
    public bool HighlightKeyItems = false;
    public bool ShowCorrectDigits = false;

    // —— 难度调整类 ——
    public bool SimplifySudoku = false;
    public bool CanBreakLocks = false;
    public bool breakMaskLimits = false;
    // —— 结算类 ——
    public bool CanFlash = false;

    // 你也可以用 Dictionary
    public Dictionary<string, int> IntValues = new();
    
    public List<Mask> Masks = new();
}
