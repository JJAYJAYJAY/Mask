using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;
    private List<BuffMetadata> buffs = new();
    public List<BuffMetadata> allBuffs;
    public Dictionary<string, BuffMetadata> buffDict = new();

    [Header("Debug UI")]
    public Canvas debugCanvas; // 场景里挂一个 Canvas
    public Button buttonTemplate; // 场景里挂一个 Button，作为模板
    public float buttonSpacing = 30f;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (BuffMetadata buff in allBuffs)
        {
            buffDict[buff.buffName] = buff;
        }

        SetupDebugPanel();
    }

    void SetupDebugPanel()
    {
        if (debugCanvas == null || buttonTemplate == null) return;

        buttonTemplate.gameObject.SetActive(false); // 模板隐藏

        for (int i = 0; i < allBuffs.Count; i++)
        {
            BuffMetadata meta = allBuffs[i];
            Button btn = Instantiate(buttonTemplate, debugCanvas.transform);
            btn.gameObject.SetActive(true);
            btn.transform.localPosition = new Vector3(0, -i * buttonSpacing, 0);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = meta.buffName;

            btn.onClick.AddListener(() =>
            {
                Buff newBuff = BuffFactory.Create(meta);
                AddBuff(newBuff);
                Debug.Log("Added Buff: " + meta.buffName);
            });
        }
    }

    public void AddBuff(Buff buff)
    {
        buff.Apply(GameManager.Instance.globalRuleData);
        buffs.Add(buff.Meta);
    }

    public void RemoveBuff(Buff buff)
    {
        buff.Remove(GameManager.Instance.globalRuleData);
        buffs.Remove(buff.Meta);
    }
    
    public bool HasBuff(BuffMetadata meta)
    {
        return buffs.Contains(meta);
    }
}