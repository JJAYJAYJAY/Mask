using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockManager : MonoBehaviour
{
    public LockWheel[] wheels;
    public int[] correctPassword;

    public TextMeshProUGUI correctNumText;
    public TextMeshProUGUI DescribeText;
    public TextMeshProUGUI TipsText;
    public string description;
    public string tips;
    public Button breakButton;
    public Button tryButton;

    public List<GameObject> relateGameobject;
    public DetailPanelController choosePanel;
    public DetailPanelController selfPanel;
    
    public puzzleList type;
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("MainCamera not found!");
        }
    }

    void Start()
    {
        GameManager.Instance.globalRuleData.OnShowCorrectDigitsChanged += OnshowCorrectDigitsChanged;
        GameManager.Instance.globalRuleData.OnCanBreakLocksChanged += OncanBreakLocksChanged;
        GameManager.Instance.globalRuleData.OnShowHintTextChanged += OnShowHintTextChanged;
        //添加listener
        
        tryButton.onClick.AddListener(CheckPassword);
        breakButton.onClick.AddListener(BreakLock);
        DescribeText.text = description;
        TipsText.text = tips;
    }
    public void CheckPassword()
    {
        if (!GameManager.Instance.puzzlePasswords.TryGetValue(type, out var value))
            return;
        SetPassword(GameManager.Instance.puzzlePasswords[type]);
        Debug.Log(GameManager.Instance.puzzlePasswords[type]);
        // for (int i = 0; i < correctPassword.Length; i++)
        // {
        //     Debug.Log(correctPassword[i]);
        // }
        int count = 0;
        bool flag = true;
        if (wheels.Length != correctPassword.Length)
            return ;

        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i].CurrentValue == correctPassword[i]) count++;
            if (wheels[i].CurrentValue != correctPassword[i])
                flag=false;
        }
        if (correctNumText != null)
            correctNumText.text = "正确密码位数："+count + "/" + wheels.Length;
        if (flag)
        {
            Onsuccess();
        }
    }

    public void Onsuccess()
    {
        StartCoroutine(OnSuccessCoroutine());
    }

    private IEnumerator OnSuccessCoroutine()
    {
        // 1️⃣ 关闭当前面板
        selfPanel.CloseToLast();

        // 2️⃣ 等待 0.5 秒（等待关闭动画）
        yield return new WaitForSeconds(0.5f);

        // 3️⃣ 解锁逻辑
        GlobalClockManager.Instance.Unlock(this);

        // 4️⃣ 隐藏关联物体
        foreach (var item in relateGameobject)
        {
            item.SetActive(false);
        }

        // 5️⃣ 屏幕中心打开另一个面板
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 10f);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenCenter);
        choosePanel.OpenFromWorldPos(worldPos);
    }
    
    public void SetPassword(string password)
    {
        if (password.Length != wheels.Length)
        {
            Debug.LogError("Password length does not match wheels count!");
            return;
        }

        correctPassword = new int[password.Length];

        for (int i = 0; i < password.Length; i++)
        {
            char c = password[i];

            if (c < '0' || c > '9')
            {
                Debug.LogError("Invalid password character: " + c);
                return;
            }

            correctPassword[i] = c - '0';
            Debug.Log(c.ToString());
        }
    }

    void BreakLock()
    {
        selfPanel.CloseToLast();
        GlobalClockManager.Instance.Unlock(this);
        foreach (var item in relateGameobject)
        {
            item.SetActive(false);
        }
        
        GameManager.Instance.globalRuleData.CanBreakLocks=false;
    }

    void OnshowCorrectDigitsChanged(bool flag)
    {
        correctNumText.gameObject.SetActive(flag);
    }
    
    void OncanBreakLocksChanged(bool flag)
    {
        breakButton.gameObject.SetActive(flag);
    }

    void OnShowHintTextChanged(bool flag)
    {
        TipsText.gameObject.SetActive(flag);
    }
}