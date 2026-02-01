using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalClockManager:MonoBehaviour
{
    public List<LockManager> unlockmanagers;
    public List<LockManager> lockmanagers;
    
    public GameObject SpeakEnd;
    public GameObject ChaosEnd;
    public GameObject SoulEnd;
    public GameObject CompleteEnd;
    public GameObject NormalEnd;
    
    public AudioSource audioSource;
    public AudioClip SpeakEndBgm;
    public AudioClip ChaosEndBgm;
    public AudioClip SoulEndBgm;
    public AudioClip CompleteEndBgm;
    public AudioClip NormalEndBgm;
    public static GlobalClockManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        unlockmanagers = new List<LockManager>();
        lockmanagers = new List<LockManager>();
    }

    public void Unlock(LockManager lockmanager)
    {
        unlockmanagers.Add(lockmanager);
        lockmanagers.Remove(lockmanager);
        CheckEnd();
    }

    public void BreakRandomLock()
    {
        int index = Random.Range(0, lockmanagers.Count);
        lockmanagers[index].Onsuccess();
        //移除加入
        unlockmanagers.Add(lockmanagers[index]);
        lockmanagers.RemoveAt(index);
    }
    public void playEndStroy(int index){
        StartCoroutine(EndStroySequence(index));
    }
    private IEnumerator EndStroySequence(int index)
    {
        // 1️⃣ 渐黑（等待播放完）
        yield return ScreenFader.Instance.FadeOut(0.5f);

        // 2️⃣ 后续逻辑
        GlitchTextWriter.Instance.playEndStory(index);
        yield return ScreenFader.Instance.FadeIn(0.1f);
    }

    public void CheckEnd()
    {
        //normal 一个面具
        // soul 没有面具
        // Chaos 多个面具
        // complete所有面具
        // speaker 穿孔+全部面具
        if (lockmanagers.Count == 0)
        {
            List<ItemData> itemDatas = new List<ItemData>();
            foreach (var item in Inventory.Instance.items)
            {
                if (item.type == ItemType.Mask)
                {
                    itemDatas.Add(item);
                }
            }

            if (itemDatas.Count == 0)
            {
                audioSource.PlayOneShot(SoulEndBgm);
                playEndStroy(2);
                SoulEnd.SetActive(true);
                return;
            }
            if (itemDatas.Count == 1)
            {
                audioSource.PlayOneShot(SoulEndBgm);
                playEndStroy(1);
                NormalEnd.SetActive(true);
                return;
            }
            if (itemDatas.Count < 5)
            {
                audioSource.PlayOneShot(ChaosEndBgm);
                playEndStroy(3);
                ChaosEnd.SetActive(true);
                return;
            }

            if (itemDatas.Count == 5)
            {
                if (GameManager.Instance.globalRuleData.SpecialEnd)
                {
                    audioSource.PlayOneShot(SpeakEndBgm);
                    playEndStroy(5);
                    SpeakEnd.SetActive(true);
                    return;
                }
                audioSource.PlayOneShot(CompleteEndBgm);
                playEndStroy(4);
                CompleteEnd.SetActive(true);
            }
        }
    }
}
