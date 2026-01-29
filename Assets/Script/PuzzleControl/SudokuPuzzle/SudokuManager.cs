using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SudokuManager: MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject vitalCellPrefab;
    public GameObject emptySlotPrefab;
    public GameObject detectPrefab;
    public GameObject detectBoard;
    public GameObject vitalBoard;
    public GameObject dataBoard;
    public GameObject pieceArea;
    
    public Sprite[] numberSprites;
    public Sprite[] vitalNumberSprites;
    public Sprite[] patternSprites;
    public static SudokuManager Instance { get; private set; }
    private List<DetectCell> cells = new List<DetectCell>();
    // 0 表示空
    int[,] puzzle;
    int[,] solution;

    private int[,] currentState; 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SudokuGenerator sudokuGenerator = new SudokuGenerator();
        puzzle = sudokuGenerator.Generate(10); // 生成一个有40个空格的数独谜题
        solution = sudokuGenerator.GetSolution(); // 获取对应的解
        printBoard();
        currentState = puzzle;
    }

    void Start()
    {
        GenerateBoard();
    }
    public List<int> GetRandomPositions(int count, List<int> candidates)
    {
        List<int> results = new List<int>(new int[count]);
        
        // 循环抽取
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, candidates.Count); // 随机选一个索引
            results[i] = candidates[randomIndex];              // 记录数值
            candidates.RemoveAt(randomIndex);                   // 【关键】从候选名单移除
        }
        
        return results;
    }

    void GenerateBoard()
    {
        // 统计棋盘的空位置
        List<int> candidates = new List<int>();
        for (int i = 0; i < 81; i++)
        {
            int row = i / 9;
            int col = i % 9;
            if (puzzle[row, col] == 0)
            {   
                candidates.Add(i);
            }
        }
        //在vitalBoard上随机4个点坐标生成 Vitalcell
        List<int> positions = GetRandomPositions(4, candidates);
        // 创建81个空物体
        for (int i = 0; i < 81; i++)
        {
            GameObject go;

            if (positions.Contains(i))
            {
                // VitalCell
                go = Instantiate(vitalCellPrefab, vitalBoard.transform);
                Image img = go.transform.Find("Number").GetComponent<Image>();
                for (int j = 0; j < positions.Count; j++)
                {
                    if(positions[j] == i) img.sprite = vitalNumberSprites[j];   
                }
            }
            else
            {
                // 空 Slot
                go = Instantiate(emptySlotPrefab, vitalBoard.transform);
            }
        }
        for (int i = 0; i < 81; i++)
        {
            int row = i / 9;
            int col = i % 9;

            if (puzzle[row, col] != 0)
            {
                // 普通 Cell
                GameObject go = Instantiate(cellPrefab, dataBoard.transform);
                Image img = go.transform.Find("Number").GetComponent<Image>();
                img.sprite = numberSprites[puzzle[row, col]];
                Image patternImg = go.transform.Find("Pattern").GetComponent<Image>();
                patternImg.sprite = patternSprites[i%2];
                AddCell(false,i);
            }
            else
            {
                GameObject emptySlot = new GameObject("EmptySlot");
                emptySlot.AddComponent<RectTransform>();
                emptySlot.transform.SetParent(dataBoard.transform);
                AddCell(true,i);
            }
        }
        
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (puzzle[row, col] == 0)
                {
                    int number = solution[row, col]; // 取正确答案作为碎片
                    GameObject pieceGO = Instantiate(cellPrefab, pieceArea.transform);

                    // 设置图片
                    Image img = pieceGO.transform.Find("Number").GetComponent<Image>();
                    if (img != null)
                    {
                        img.sprite = numberSprites[number];
                        img.raycastTarget = true;
                    }
                    Image patternImg = pieceGO.transform.Find("Pattern").GetComponent<Image>();
                    patternImg.sprite = patternSprites[(row * 9 + col) % 2];
                    

                    // 可随机摆放在碎片区
                    RectTransform rt = pieceGO.GetComponent<RectTransform>();
                    rt.localScale = Vector3.one;

                    float xOffset = Random.Range(-pieceArea.GetComponent<RectTransform>().rect.width / 2 + 30,
                        pieceArea.GetComponent<RectTransform>().rect.width / 2 - 30);
                    float yOffset = Random.Range(-pieceArea.GetComponent<RectTransform>().rect.height / 2 + 30,
                        pieceArea.GetComponent<RectTransform>().rect.height / 2 - 30);
                    rt.anchoredPosition = new Vector2(xOffset, yOffset);

                    // 确保挂载 Piece 组件
                    if (pieceGO.GetComponent<DragCell>() == null)
                        pieceGO.AddComponent<DragCell>();
                
                    pieceGO.GetComponent<DragCell>().number = number;
                    // 确保挂载 CanvasGroup
                    if (pieceGO.GetComponent<CanvasGroup>() == null)
                    {
                        CanvasGroup cg = pieceGO.AddComponent<CanvasGroup>();
                        cg.blocksRaycasts = true;
                    }
                }
            }
        }
    }
    
    public void AddCell(bool isActive,int index)
    {
        GameObject detectCell=Instantiate(detectPrefab,detectBoard.transform);
        detectCell.AddComponent<DetectCell>();
        DetectCell cell = detectCell.GetComponent<DetectCell>();
        cell.canPlace = isActive;
        cell.index = index;
        cells.Add(cell);
    }
    
    public void modifyCurrentState(int index, int number)
    {
        int row = index / 9;
        int col = index % 9;
        currentState[row, col] = number;
    }
    
    public void printMatrix()
    {
        string output = "";
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                output += currentState[r, c] + " ";
            }
            output += "\n";
        }
        Debug.Log(output);
    }

    public void printBoard()
    {
        //打印谜底
        string output = "";
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                output += solution[r, c] + " ";
            }
            output += "\n";
        }
        Debug.Log(output);
    }
}