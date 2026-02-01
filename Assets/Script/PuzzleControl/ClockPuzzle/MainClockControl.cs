using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainClockControl : MonoBehaviour
{
    public puzzleList type;
    //TODO 声音系统
    [Header("Audio")]
    public AudioSource rotateSound;
    
    [Header("UI")]
    public GameObject numberPrefab;
    public GameObject numberClock;
    private float radius;

    [Range(0, 360)]
    public float startAngleOffset = 90f;
    
    public GameObject colorMarkerPrefab;
    // public GameObject targetMarkPrefab;
    public int[] targetPositions;
    
    public Sprite[] ColorSprites;
    public Sprite GraySprite;
    public Sprite[] TargetSprites;
    private bool[,] previousMatch;
    // ===================== 枚举定义 =====================
    public enum SlotColor
    {
        None = 0,
        Red = 1,
        Yellow = 2,
        Blue = 3
    }

    // ===================== 轮盘数据 =====================
    [System.Serializable]
    public class RingData
    {
        public RectTransform ringTransform;
        
        private SlotColor[] slots = new SlotColor[12];
        public SlotColor[] Slots => slots;
    }

    public List<RingData> rings = new List<RingData>();
    private int[] numberPositions = new int[12];
    // ===================== Unity =====================
    void Start()
    {
        targetPositions = GetRandomPositions(3, 12);
        GenerateNumbers();
        GenerateSolvablePuzzle();
    }
    
    void Awake()
    {
        previousMatch = new bool[rings.Count, 12];
        // 默认全部 false
        for (int i = 0; i < rings.Count; i++)
        for (int j = 0; j < 12; j++)
            previousMatch[i, j] = false;
    }


    // ===================== 数字生成 =====================
    void GenerateNumbers()
    {
        radius = numberClock.GetComponent<RectTransform>().rect.width / 2f * 0.95f;
        for (int i = 1; i <= 12; i++)
        {
            float angle = startAngleOffset - (i * 30f);
            float radian = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(radian) * radius;
            float y = Mathf.Sin(radian) * radius;

            GameObject numObj = Instantiate(numberPrefab, numberClock.transform);
            RectTransform rect = numObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(x, y);

            numObj.transform.localRotation = Quaternion.Euler(0, 0, -(i * 30f));
            numberPositions[i-1] = Random.Range(0, 10);
            numObj.GetComponentInChildren<TMP_Text>().text = numberPositions[i-1].ToString();
            
            numObj.name = $"Num_{i}";
        }
    }

    // ===================== 解谜判定 =====================
    public void CheckMultiPointPuzzle()
    {
        // PrintCurrentState();
        bool blueOK   = IsColorMatchAtPosition(targetPositions[(int)SlotColor.Blue], SlotColor.Blue);
        bool yellowOK = IsColorMatchAtPosition(targetPositions[(int)SlotColor.Yellow], SlotColor.Yellow);
        bool redOK    = IsColorMatchAtPosition(targetPositions[(int)SlotColor.Red], SlotColor.Red);

        if (blueOK && yellowOK && redOK)
        {
            Debug.Log("<color=lime>解密成功！</color>");
            OnSolved();
        }
    }

    bool IsColorMatchAtPosition(int physPos, SlotColor targetColor)
    {
        foreach (var ring in rings)
        {
            // 1. 获取当前旋转了多少个“格” (顺时针方向)
            float z = ring.ringTransform.localEulerAngles.z;
            int rotationOffset = Mathf.RoundToInt((360f - z) % 360f / 30f);

            // 2. 计算当前物理位置对应的数组索引
            int dataIndex = (physPos - rotationOffset) % 12;
            if (dataIndex < 0) dataIndex += 12;

            if (ring.Slots[dataIndex] != targetColor)
                return false;
        }
        return true;
    }
    
    public int[] GetRandomPositions(int count, int max)
    {
        // 1. 准备一个包含 0-11 的候选列表
        List<int> candidates = new List<int>();
        for (int i = 0; i < max; i++) candidates.Add(i);

        int[] results = new int[count+1];

        // 2. 循环抽取
        for (int i = 1; i < count+1; i++)
        {
            int randomIndex = Random.Range(0, candidates.Count); // 随机选一个索引
            results[i] = candidates[randomIndex];              // 记录数值
            candidates.RemoveAt(randomIndex);                   // 【关键】从候选名单移除
        }

        results[0] = -1;
        return results;
    }
    
    void OnSolved()
    {
        // GameManager.Instance.OnClockSolved();
    }

    // ===================== 生成可解谜状态 =====================
    public void GenerateSolvablePuzzle()
    {
        string password = "";
        // 1. 清空
        foreach (var ring in rings)
            for (int i = 0; i < 12; i++)
                ring.Slots[i] = SlotColor.None;

        // 3. 随机初始旋转 + 写入答案
        foreach (var ring in rings)
        {
            int offset = Random.Range(0, 12);
            ring.ringTransform.localEulerAngles = new Vector3(0, 0, offset * -30f);
            ring.Slots[CalcIndex(targetPositions[(int)SlotColor.Blue], offset)]   = SlotColor.Blue;
            ring.Slots[CalcIndex(targetPositions[(int)SlotColor.Yellow], offset)] = SlotColor.Yellow;
            ring.Slots[CalcIndex(targetPositions[(int)SlotColor.Red], offset)]    = SlotColor.Red;
            int index = (offset - 1)%12;
            if (index < 0) index += 12;
            password += numberPositions[index].ToString(); 
        }

        foreach (var ring in rings)
        {
            int offset = Random.Range(0, 12);
            ring.ringTransform.localEulerAngles = new Vector3(0, 0, offset * -30f);
        }
        // 4. 干扰色
        FillNoiseColors();
        
        UpdateVisuals();
        Debug.Log($"谜题密码（仅供调试）: {password}");
        GameManager.Instance.puzzlePasswords[type] = password;
    }
    
    void UpdateVisuals()
    {
        for (int i = 0; i < rings.Count; i++)
        {
            float ringRadius;
            switch (i)
            {
                case 3:
                    ringRadius = rings[i].ringTransform.rect.width / 2f * 0.5f;
                    break;
                case 2:
                    ringRadius = rings[i].ringTransform.rect.width / 2f * 0.65f; 
                    break;
                case 1:
                    ringRadius = rings[i].ringTransform.rect.width / 2f * 0.7f;
                    break;
                case 0:
                    ringRadius = rings[i].ringTransform.rect.width / 2f * 0.75f;
                    break;
                default:
                    ringRadius = rings[i].ringTransform.rect.width / 2f * 0.8f;
                    break;
            }

            // 🔴 清理旧 Marker（非常重要）
            for (int c = rings[i].ringTransform.childCount - 1; c >= 0; c--)
            {
                Transform child = rings[i].ringTransform.GetChild(c);
                if (child.name.StartsWith("Marker_"))
                    Destroy(child.gameObject);
            }

            for (int dataIndex = 0; dataIndex < 12; dataIndex++)
            {
                SlotColor color = rings[i].Slots[dataIndex];
                if (color == SlotColor.None) continue;
                
                // 数据索引 0 就是正上方
                float angle = startAngleOffset - dataIndex * 30f;
                float radian = angle * Mathf.Deg2Rad;

                float x = Mathf.Cos(radian) * ringRadius;
                float y = Mathf.Sin(radian) * ringRadius;

                GameObject marker = Instantiate(colorMarkerPrefab, rings[i].ringTransform);
                marker.name = $"Marker_{dataIndex}";
                marker.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                marker.GetComponent<RectTransform>().localScale = new Vector3((4-i)*0.8f+1, (4-i)*0.8f+1, (4-i)*0.8f+1);
                marker.GetComponent<UnityEngine.UI.Image>().sprite = GetUISprite(color);
                if (type == puzzleList.ClockPuzzle2)
                {
                    // 50%概率变成灰色
                    if (Random.value < 0.5f)
                    {
                        marker.GetComponent<UnityEngine.UI.Image>().sprite = GraySprite;
                    }
                }
            }
        }
        
        //我想在最外环目标颜色位置生成颜色标志注意标志要旋转
        int outerRingIndex = 0;
        var outerRing = rings[outerRingIndex];

// 半径（比最外环 marker 稍微外一点）
        float targetRadius = outerRing.ringTransform.rect.width / 2f * 0.9f;

// 1️⃣ 清理旧目标标志
        for (int c = outerRing.ringTransform.childCount - 1; c >= 0; c--)
        {
            Transform child = outerRing.ringTransform.GetChild(c);
            if (child.name.StartsWith("TargetMarker_"))
                Destroy(child.gameObject);
        }

// 2️⃣ 为每种目标颜色生成标志
        foreach (SlotColor color in new[] { SlotColor.Red, SlotColor.Yellow, SlotColor.Blue })
        {
            int physPos = targetPositions[(int)color];

            // 物理角度（不加 ring 的旋转）
            float angle = startAngleOffset - physPos * 30f;
            float radian = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(radian) * targetRadius;
            float y = Mathf.Sin(radian) * targetRadius;

            GameObject marker = Instantiate(colorMarkerPrefab, outerRing.ringTransform);
            marker.gameObject.transform.SetParent(outerRing.ringTransform.parent);
            marker.transform.SetAsFirstSibling();
            marker.name = $"TargetMarker_{color}";

            RectTransform rt = marker.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(x, y);
            rt.localScale = Vector3.one * 5.5f;
            // 3️⃣ 朝向圆心（关键）
            Vector2 dirToCenter = -rt.anchoredPosition;
            float rotZ = Mathf.Atan2(dirToCenter.y, dirToCenter.x) * Mathf.Rad2Deg + 90f;
            rt.localRotation = Quaternion.Euler(0, 0, rotZ);
           
            // 4️⃣ 设置颜色
            var img = marker.GetComponent<UnityEngine.UI.Image>();
            img.sprite = GetTargetSprite(color);
        }
    }
    Sprite GetTargetSprite(SlotColor color)
    {
        switch (color)
        {
            case SlotColor.Red: return TargetSprites[0];
            case SlotColor.Yellow: return TargetSprites[1];
            case SlotColor.Blue: return TargetSprites[2];
            default: return null;
        }
    }

    Sprite GetUISprite(SlotColor color)
    {
        switch (color)
        {
            case SlotColor.Red: return ColorSprites[0];
            case SlotColor.Yellow: return ColorSprites[1];
            case SlotColor.Blue: return ColorSprites[2];
            default: return null;
        }
    }

    int CalcIndex(int physPos, int offset)
    {
        int idx = (physPos - offset) % 12;
        return idx < 0 ? idx + 12 : idx;
    }

    void FillNoiseColors()
    {
        foreach (var ring in rings)
        {
            int count = Random.Range(2, 6);
            for (int i = 0; i < count; i++)
            {
                int idx = Random.Range(0, 12);
                if (ring.Slots[idx] == SlotColor.None)
                {
                    ring.Slots[idx] = (SlotColor)Random.Range(1, 4);
                }
            }
        }
    }

    // ===================== 调试输出 =====================
    void PrintCurrentState()
    {
        Debug.Log("Target Positions: " +
                  $"Blue at {targetPositions[(int)SlotColor.Blue]}, " +
                  $"Yellow at {targetPositions[(int)SlotColor.Yellow]}, " +
                  $"Red at {targetPositions[(int)SlotColor.Red]}");
        Debug.Log("<color=cyan>=== 当前轮盘状态 ===</color>");

        for (int i = 0; i < rings.Count; i++)
        {
            int offset = Mathf.RoundToInt(rings[i].ringTransform.localEulerAngles.z / 30f);
            int topIdx = (0 - offset) % 12;
            if (topIdx < 0) topIdx += 12;

            string line = $"<b>Ring {i}</b> (偏移:{offset}): ";

            for (int j = 0; j < 12; j++)
            {
                string v = rings[i].Slots[j].ToString();
                if (j == topIdx)
                    line += $"<color=yellow>[{v}]</color> ";
                else
                    line += v + " ";
            }

            Debug.Log(line);
        }
    }
    
}
