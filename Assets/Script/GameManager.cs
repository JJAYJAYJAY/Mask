using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 单例实例
    public static GameManager Instance { get; private set; }
    // 不同的谜题不同的密码用map存 //随机生成
    public Dictionary<string, string> puzzlePasswords = new Dictionary<string, string>();
    // 全局buff规则
    public GlobalRuleData Data = new();
    // 随机选择器
    public RewardSelector rewardSelector;
    public RewardRandomPool rewardRandomPool;
    
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
        rewardSelector = new RewardSelector(rewardRandomPool);
    }

    // 提供一个统一的接口来控制
    public void SetBlocker(bool state)
    {
        if (globalBlocker == null) return;
        // alpha=1 显示，alpha=0 隐藏
        globalBlocker.alpha = state ? 0.5f : 0f;
        
        // blocksRaycasts 是核心：为 true 时拦截点击，为 false 时点击穿透
        globalBlocker.blocksRaycasts = state;
        
        // 可选：是否让遮罩内的按钮失效
        globalBlocker.interactable = state;
    }
    
    void GenerateAllPasswords()
    {

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
    
    public void showText(string content)
    {
        text.text = content;
    }
}