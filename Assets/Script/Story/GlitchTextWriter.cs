using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GlitchTextWriter : MonoBehaviour
{
    public static GlitchTextWriter Instance;
    [Header("UI")]
    public TMP_Text textUI;

    [Header("Speed")]
    public int charsPerFrame = 2;     // 每帧推进几个字符（调大=更快）
    public int glitchTailLength = 1;  // 尾巴乱码长度（1~2最舒服）

    [Header("Data")]
    public StoryData[] storyDatas;
    public Dictionary<Mask,StoryData> storyDict;
    private const string glitchChars = "@#$%&*+=-?/\\!";
    private Coroutine playCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        foreach (var storyData in storyDatas)
        {
            storyDict.Add(storyData.mask, storyData);
        }
        gameObject.SetActive(false);
    }

    public void PlayMaskStory(Mask mask)
    {
        gameObject.SetActive(true);
        if (playCoroutine != null)
            StopCoroutine(playCoroutine);
        string fullText = storyDict[mask].content;
        playCoroutine = StartCoroutine(PlayCoroutine(fullText));
    }

    public void StopMaskStory()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator PlayCoroutine(string fullText)
    {
        int targetLength = fullText.Length;
        int currentLength = 0;

        StringBuilder builder = new StringBuilder(targetLength);

        textUI.text = "";

        while (currentLength < targetLength)
        {
            currentLength = Mathf.Min(
                currentLength + charsPerFrame,
                targetLength
            );

            builder.Clear();

            int stableCount = Mathf.Max(0, currentLength - glitchTailLength);

            for (int i = 0; i < stableCount; i++)
            {
                builder.Append(fullText[i]);
            }

            for (int i = stableCount; i < currentLength; i++)
            {
                // 换行不乱码
                if (fullText[i] == '\n')
                    builder.Append('\n');
                else
                    builder.Append(GetRandomChar());
            }

            textUI.text = builder.ToString();
            yield return null; // 每帧更新，不卡
        }

        // 最终修正为完整文本
        textUI.text = fullText;
    }

    private char GetRandomChar()
    {
        int index = Random.Range(0, glitchChars.Length);
        return glitchChars[index];
    }
}
