using System;
using System.Collections.Generic;

public class SudokuGenerator
{
    private const int SIZE = 9;
    private int[,] board = new int[SIZE, SIZE];
    private int[,] solution = new int[SIZE, SIZE];
    private Random random = new Random();

    // Public 接口
    public int[,] Generate(int emptyCount)
    {
        // 1️⃣ 生成完整解
        FillBoard();
        Array.Copy(board, solution, solution.Length);
        // 2️⃣ 挖空，保证唯一解
        MakeHoles(emptyCount);

        return board;
    }
    
    public int[,] GetSolution()
    {
        return solution;
    }
    // ========================
    // 生成完整解
    // ========================
    private bool FillBoard()
    {
        return FillCell(0, 0);
    }

    private bool FillCell(int row, int col)
    {
        if (row == SIZE) return true; // 完成

        int nextRow = col == SIZE - 1 ? row + 1 : row;
        int nextCol = col == SIZE - 1 ? 0 : col + 1;

        List<int> numbers = new List<int>();
        for (int i = 1; i <= 9; i++) numbers.Add(i);
        Shuffle(numbers);

        foreach (int num in numbers)
        {
            if (IsSafe(row, col, num))
            {
                board[row, col] = num;
                if (FillCell(nextRow, nextCol)) return true;
                board[row, col] = 0;
            }
        }

        return false; // 回溯
    }

    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }


    private bool IsSafe(int row, int col, int num)
    {
        // 检查行列
        for (int i = 0; i < SIZE; i++)
        {
            if (board[row, i] == num || board[i, col] == num)
                return false;
        }

        // 检查 3x3
        int startRow = row / 3 * 3;
        int startCol = col / 3 * 3;
        for (int r = startRow; r < startRow + 3; r++)
            for (int c = startCol; c < startCol + 3; c++)
                if (board[r, c] == num) return false;

        return true;
    }

    // ========================
    // 挖空并保证唯一解
    // ========================
    private void MakeHoles(int emptyCount)
    {
        List<(int, int)> positions = new List<(int, int)>();
        for (int r = 0; r < SIZE; r++)
            for (int c = 0; c < SIZE; c++)
                positions.Add((r, c));

        Shuffle(positions);

        int removed = 0;
        foreach (var pos in positions)
        {
            int backup = board[pos.Item1, pos.Item2];
            board[pos.Item1, pos.Item2] = 0;

            if (HasUniqueSolution())
            {
                removed++;
                if (removed >= emptyCount) break;
            }
            else
            {
                board[pos.Item1, pos.Item2] = backup; // 恢复
            }
        }
    }

    // ========================
    // 唯一解验证
    // ========================
    private bool HasUniqueSolution()
    {
        int[,] copy = new int[SIZE, SIZE];
        Array.Copy(board, copy, board.Length);
        int solutions = 0;

        void Solve(int row, int col)
        {
            if (solutions > 1) return; // 已经不唯一

            if (row == SIZE)
            {
                solutions++;
                return;
            }

            int nextRow = col == SIZE - 1 ? row + 1 : row;
            int nextCol = col == SIZE - 1 ? 0 : col + 1;

            if (copy[row, col] != 0)
            {
                Solve(nextRow, nextCol);
            }
            else
            {
                for (int num = 1; num <= 9; num++)
                {
                    if (IsSafeForBoard(copy, row, col, num))
                    {
                        copy[row, col] = num;
                        Solve(nextRow, nextCol);
                        copy[row, col] = 0;
                    }
                }
            }
        }

        Solve(0, 0);
        return solutions == 1;
    }

    private bool IsSafeForBoard(int[,] b, int row, int col, int num)
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (b[row, i] == num || b[i, col] == num)
                return false;
        }

        int startRow = row / 3 * 3;
        int startCol = col / 3 * 3;
        for (int r = startRow; r < startRow + 3; r++)
            for (int c = startCol; c < startCol + 3; c++)
                if (b[r, c] == num) return false;

        return true;
    }
}
