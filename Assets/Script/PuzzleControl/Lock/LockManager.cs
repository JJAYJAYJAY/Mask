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
    void Start()
    {
        GameManager.Instance.Data.OnShowCorrectDigitsChanged += OnshowCorrectDigitsChanged;
        GameManager.Instance.Data.OnCanBreakLocksChanged += OncanBreakLocksChanged;
    }
    public void CheckPassword()
    {
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

    void OnshowCorrectDigitsChanged(bool flag)
    {
        correctNumText.gameObject.SetActive(flag);
    }
    
    void OncanBreakLocksChanged(bool flag)
    {
        breakButton.gameObject.SetActive(flag);
    }
}