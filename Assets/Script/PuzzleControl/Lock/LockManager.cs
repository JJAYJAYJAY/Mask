using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockManager : MonoBehaviour
{
    public LockWheel[] wheels;
    public int[] correctPassword;

    public TextMeshProUGUI correctNumText;
    
    public Button breakButton;
    public Button tryButton;

    public List<GameObject> relateGameobject;
    public DetailPanelController choosePanel;
    public DetailPanelController selfPanel;
    
    public puzzleList type;
    void Start()
    {
        GameManager.Instance.globalRuleData.OnShowCorrectDigitsChanged += OnshowCorrectDigitsChanged;
        GameManager.Instance.globalRuleData.OnCanBreakLocksChanged += OncanBreakLocksChanged;
        //添加listener
        
        tryButton.onClick.AddListener(CheckPassword);
        breakButton.onClick.AddListener(BreakLock);
    }
    public void CheckPassword()
    {
        SetPassword(GameManager.Instance.puzzlePasswords[type]);
        int count = 0;
        bool flag = true;
        if (wheels.Length != correctPassword.Length)
            return ;

        for (int i = 0; i < wheels.Length; i++)
        {
            Debug.Log(wheels[i].gameObject.name+":"+ wheels[i].CurrentValue);
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
        selfPanel.CloseToLast();
        GlobalClockManager.Instance.Unlock(this);
        foreach (var item in relateGameobject)
        {
            item.SetActive(false);
        }
        //屏幕中心打开
        choosePanel.OpenFromWorldPos(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 10f)));
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

            if (c < '1' || c > '9')
            {
                Debug.LogError("Invalid password character: " + c);
                return;
            }

            correctPassword[i] = c - '0';
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
}