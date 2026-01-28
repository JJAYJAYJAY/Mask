using System.Collections.Generic;
using UnityEngine;
public class SudokuData
{
    public int[,] solution = new int[9, 9];   // 完整解
    public int[,] puzzle   = new int[9, 9];   // 玩家看到的
    public int[,] pattern  = new int[9, 9];   // 0 / 1 花纹
    int[,] demoSolution =
    {
        {5,3,4,6,7,8,9,1,2},
        {6,7,2,1,9,5,3,4,8},
        {1,9,8,3,4,2,5,6,7},
        {8,5,9,7,6,1,4,2,3},
        {4,2,6,8,5,3,7,9,1},
        {7,1,3,9,2,4,8,5,6},
        {9,6,1,5,3,7,2,8,4},
        {2,8,7,4,1,9,6,3,5},
        {3,4,5,2,8,6,1,7,9}
    };

    void GeneratePuzzle()
    {
        solution = demoSolution;
        // 先复制解
        for (int r = 0; r < 9; r++)
        for (int c = 0; c < 9; c++)
            puzzle[r, c] = solution[r, c];

        // 随机挖 40 个洞
        int holes = 40;
        while (holes > 0)
        {
            int r = Random.Range(0, 9);
            int c = Random.Range(0, 9);

            if (puzzle[r, c] != 0)
            {
                puzzle[r, c] = 0;
                holes--;
            }
        }
        List<SudokuPiece> pieces = new List<SudokuPiece>();

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (puzzle[r, c] == 0)
                {
                    pieces.Add(new SudokuPiece
                    {
                        number = solution[r, c],
                        pattern = pattern[r, c]
                    });
                }
            }
        }
    }
}

class SudokuPiece
{
    public int number;      // 1-9
    public int pattern;     // 0 or 1
}
