using System.Collections.Generic;
using UnityEngine;

public class SudokuGenerator
{
    private const int SIZE = 9;
    private int[,] grid = new int[SIZE, SIZE];
    public int[,] patternMap = new int[SIZE, SIZE];
    private int currentPatternId = 0;
    
    public int[,] Generate()
    {
        FillGrid(0, 0);
        return grid;
    }
    
    private bool FillGrid(int row, int col)
    {
        if (row == SIZE)
            return true;

        int nextRow = (col == SIZE - 1) ? row + 1 : row;
        int nextCol = (col == SIZE - 1) ? 0 : col + 1;

        List<int> numbers = GetShuffledNumbers();

        foreach (int num in numbers)
        {
            if (IsValid(row, col, num))
            {
                grid[row, col] = num;
                if (FillGrid(nextRow, nextCol))
                    return true;
                grid[row, col] = 0;
            }
        }

        return false;
    }
    
    private bool IsValid(int row, int col, int num)
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (grid[row, i] == num) return false;
            if (grid[i, col] == num) return false;
        }

        int boxRow = row / 3 * 3;
        int boxCol = col / 3 * 3;

        for (int r = 0; r < 3; r++)
        for (int c = 0; c < 3; c++)
            if (grid[boxRow + r, boxCol + c] == num)
                return false;

        return true;
    }

    private List<int> GetShuffledNumbers()
    {
        List<int> nums = new List<int>();
        for (int i = 1; i <= 9; i++)
            nums.Add(i);

        for (int i = 0; i < nums.Count; i++)
        {
            int j = Random.Range(i, nums.Count);
            (nums[i], nums[j]) = (nums[j], nums[i]);
        }

        return nums;
    }

    void GenerateDiagonalPattern()
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                patternMap[r, c] = (r + c) % 2;
            }
        }
    }

}