using UnityEngine;

public class LockManager : MonoBehaviour
{
    public LockWheel[] wheels;
    public int[] correctPassword;

    public bool CheckPassword()
    {
        if (wheels.Length != correctPassword.Length)
            return false;

        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i].CurrentValue != correctPassword[i])
                return false;
        }
        return true;
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

}