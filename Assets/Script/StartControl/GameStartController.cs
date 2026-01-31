using UnityEngine;
using System.Collections;

public class GameStartController : MonoBehaviour
{
    public CameraMover cameraMover;
    public ScreenFader fader;

    public CanvasGroup gameUI;
    public CanvasGroup startUI;

    public AudioClip bgmMusic;
    public AudioSource audioSource;
    public void OnStartButtonClicked()
    {
        StartCoroutine(StartGameSequence());
    }

    IEnumerator StartGameSequence()
    {
        // 禁用按钮防止重复点击
        Time.timeScale = 1f;

        // 1️⃣ 摄像机慢慢下移 + 同时开始变暗
        // StartCoroutine(cameraMover.MoveDown(15,2f));
        yield return fader.FadeOut(1f);
        HideStartUI();
        cameraMover.TeleportToMainView();
        // 2️⃣ 黑屏中（闭眼）——这里可以切状态
        yield return new WaitForSeconds(0.2f);

        // 3️⃣ 睁眼
        yield return fader.FadeIn(1.5f);

        // 4️⃣ 显示游戏 UI
        ShowGameUI();
        GameState.IsGameBegin = true;
        audioSource.PlayOneShot(bgmMusic);
        audioSource.loop = true;
    
    }

    void ShowGameUI()
    {
        gameUI.alpha = 1f;
        gameUI.interactable = true;
        gameUI.blocksRaycasts = true;
    }
    
    void HideStartUI()
    {
        startUI.alpha = 0f;
        startUI.interactable = false;
        startUI.blocksRaycasts = false;
    }
}