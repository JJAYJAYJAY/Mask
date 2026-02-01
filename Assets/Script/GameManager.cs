using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum puzzleList
{
    ClockPuzzle1,
    ClockPuzzle2,
    SudokuPuzzle,
    PatternPuzzle1,
    PatternPuzzle2,
    TimePuzzle1,
    TimePuzzle2,
    BalancePuzzle1,
    BalancePuzzle2
}
public class GameManager : MonoBehaviour
{
    // 单例实例
    public static GameManager Instance { get; private set; }
    // 不同的谜题不同的密码用map存
    public Dictionary<puzzleList, string> puzzlePasswords = new Dictionary<puzzleList, string>();
    public DetailPanelController buffInit;
    // 全局buff规则
    public GlobalRuleData globalRuleData = new();
    // 随机选择器
    public RewardSelector rewardSelector;
    public RewardRandomPool rewardRandomPool;
    [Header("Audio")]
    public AudioSource audioSource;
    public  AudioClip buttonClick;
    public  AudioClip maskSelect;
    [Header("UI References")]
    [SerializeField] private CanvasGroup globalBlocker; // 拖入带 CanvasGroup 的全屏遮罩
    
    public TextMeshProUGUI text;
    public ItemDatabase database;
    private void Awake()
    {
        // 确保场景中只有一个 GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        // 初始状态：关闭遮罩
        SetBlocker(false);
        
        // 生成所有谜题的随机密码
        GenerateAllPasswords();
        
    }

    private void Start()
    {
        rewardRandomPool = new RewardRandomPool(database.allItems,BuffManager.Instance.allBuffs);
        globalRuleData.OnBreakMaskLimitsChanged += rewardRandomPool.OnMakeLimitsChanged;
        rewardSelector = new RewardSelector(rewardRandomPool);
        buffInit.OpenToDefault();
    }

    // 提供一个统一的接口来控制
    public void SetBlocker(bool state)
    {
        if (globalBlocker == null) return;
        // alpha=1 显示，alpha=0 隐藏
        globalBlocker.alpha = state ? 0.6f : 0f;
        
        // blocksRaycasts 是核心：为 true 时拦截点击，为 false 时点击穿透
        globalBlocker.blocksRaycasts = state;
        
        // 可选：是否让遮罩内的按钮失效
        globalBlocker.interactable = state;
    }
    
    void GenerateAllPasswords()
    {
        puzzlePasswords[puzzleList.PatternPuzzle1] = "4315";
        puzzlePasswords[puzzleList.PatternPuzzle2] = "541327";
        puzzlePasswords[puzzleList.TimePuzzle1] = "1502";
        puzzlePasswords[puzzleList.TimePuzzle1] = "0251";
        // 打印出来方便调试（记得正式发布时删掉）
        foreach (var puzzle in puzzlePasswords)
        {
            Debug.Log($"谜题: {puzzle.Key} 的随机答案是: {puzzle.Value}");
        }
    }
    
    
    public ItemData GetItem(string id)
    {
        return database.GetItem(id);
    }
    Coroutine clearCoroutine;
    public void showText(string content)
    {
        text.text = content;

        // 如果之前已经在倒计时，先停掉
        if (clearCoroutine != null)
            StopCoroutine(clearCoroutine);

        clearCoroutine = StartCoroutine(ClearAfterDelay(3f));
    }

    IEnumerator ClearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        text.text = "";
        clearCoroutine = null;
    }

    public void ButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClick);
    }
    
    public void selectMask(RewardOption option){
        audioSource.PlayOneShot(maskSelect);
        StartCoroutine(SelectItemSequence(option.item));
    }
    private IEnumerator SelectItemSequence(ItemData item)
    {
        // 1️⃣ 渐黑（等待播放完）
        yield return ScreenFader.Instance.FadeOut(0.5f);

        // 2️⃣ 后续逻辑
        Inventory.Instance.AddItem(item);
        GlitchTextWriter.Instance.PlayMaskStory(item.MaskType);
        rewardRandomPool.RebuildPools();
        yield return ScreenFader.Instance.FadeIn(0.1f);
    }

}