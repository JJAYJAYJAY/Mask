using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRuleData
{
    // ===== 事件 =====
    public event Action<bool> OnShowHintTextChanged;
    public event Action<bool> OnHighlightKeyItemsChanged;
    public event Action<bool> OnShowCorrectDigitsChanged;

    public event Action<bool> OnSimplifySudokuChanged;
    public event Action<bool> OnCanBreakLocksChanged;
    public event Action<bool> OnBreakMaskLimitsChanged;

    // ===== backing fields =====
    bool showHintText;
    bool highlightKeyItems;
    bool showCorrectDigits;

    bool simplifySudoku;
    bool canBreakLocks;
    bool breakMaskLimits;

    // —— 信息显示类 ——
    public bool ShowHintText
    {
        get => showHintText;
        set
        {
            if (showHintText == value) return;
            showHintText = value;
            OnShowHintTextChanged?.Invoke(value);
        }
    }

    public bool HighlightKeyItems
    {
        get => highlightKeyItems;
        set
        {
            if (highlightKeyItems == value) return;
            highlightKeyItems = value;
            OnHighlightKeyItemsChanged?.Invoke(value);
        }
    }

    public bool ShowCorrectDigits
    {
        get => showCorrectDigits;
        set
        {
            if (showCorrectDigits == value) return;
            OnShowCorrectDigitsChanged?.Invoke(value);
        }
    }

    // —— 难度调整类 ——
    public bool SimplifySudoku
    {
        get => simplifySudoku;
        set
        {
            if (simplifySudoku == value) return;
            simplifySudoku = value;
            OnSimplifySudokuChanged?.Invoke(value);
        }
    }

    public bool CanBreakLocks
    {
        get => canBreakLocks;
        set
        {
            if (canBreakLocks == value) return;
            canBreakLocks = value;
            OnCanBreakLocksChanged?.Invoke(value);
        }
    }

    public bool BreakMaskLimits
    {
        get => breakMaskLimits;
        set
        {
            if (breakMaskLimits == value) return;
            breakMaskLimits = value;
            OnBreakMaskLimitsChanged?.Invoke(value);
        }
    }
    // —— 结算类 ——
    public bool CanFlash = false;
    public bool SpecialEnd = false;

    // 你也可以用 Dictionary
    public Dictionary<string, int> IntValues = new();
    
    public List<Mask> Masks = new();
}

public static class BuffFactory
{
    public static Buff Create(BuffMetadata meta)
    {
        switch (meta.name)
        {
            case "复合条纹":
                return new CompositeStripesBuff(meta);
            case "巨力涂料":
                return new StrengthCoatingBuff(meta);
            case "贪婪疤痕":
                return new GreedScarBuff(meta);
            case "深层划痕":
                return new DeepScratchesBuff(meta);
            case "荧光涂料":
                return new FluorescentPaintBuff(meta);
            case "纯色挂坠":
                return new SolidColorPendantBuff(meta);
            case "显性纹路":
                return new ObviousBuff(meta);
            case "简约风格":
                return new SimpleStyleBuff(meta);
            case "穿孔":
                return new EndBuff(meta);
            // 以后继续加
        }

        Debug.LogError($"Unknown buff meta: {meta.name}");
        return null;
    }
}
