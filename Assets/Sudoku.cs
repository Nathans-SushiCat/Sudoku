using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http.Headers;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sudoku : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;
    int[,] Map = new int[9, 9];
    int solveTry = 0;
    TMP_Text[,] Text_MAP = new TMP_Text[9, 9];


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearMap();
            SetLockState();
        }
        if (Input.GetKeyDown(KeyCode.Space) && ChangeText.CurrentlyClickedText == null)
        {
            CheckWin();
        }
    }

    public void CreateScrambledMap()
    {
        CreateNewMap();
        ScrambleMap();
    }

    private void Start()
    {
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                Text_MAP[i, j] = GameObject.Find(i + "|" + j).GetComponent<TMP_Text>();
            }
        }
    }

    public void ScrambleMap()
    {
        for (int i = 0; i <= 6; i += 3)
        {
            for (int j = 0; j <= 6; j += 3)
            {
                int deleteAmmount = Random.Range(3, 9);
                for (int k = 0; k < deleteAmmount; k++)
                {
                    int x = Random.Range(0, 3);
                    int y = Random.Range(0, 3);

                    if (Map[i + x, j + y] != 0)
                        Map[i + x, j + y] = 0;
                    else
                        k--;
                }
            }
        }
        
        SetLockState();
        DrawMap();
    }

    public void ResetLockState()
    {
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                Text_MAP[i, j].gameObject.GetComponent<ChangeText>().Locked = false;
            }
        }
    }

    public void SetLockState()
    {
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                Text_MAP[i, j].gameObject.GetComponent<ChangeText>().Locked = Map[i, j] != 0;
            }
        }
    }

    public void DrawMapNEW()
    {
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                if (Map[i, j] != 0)
                    Text_MAP[i, j].text = "" + Map[i, j];
                else
                    Text_MAP[i, j].text = "";
            }
        }
    }

    public void DrawMap()
    {
        DrawMapNEW();
        string map = "";
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                if (Map[i, j] == 0)
                    map += "-";
                else
                    map += Map[i, j];

                //SPACING
                if ((j + 1f) % 3f == 0)
                    map += " ";
            }
            //SPACING
            map += "\n";
            if ((i + 1f) % 3f == 0)
                map += "\n";
        }
        textBox.text = map;
    }

    public void CreateNewMap()
    {
        bool created = false;
        while (!created)
        {
            created = TryCreateMap();
        }
    }

    public bool TryCreateMap()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                int[] positions = GetPossiblePositions(i, j);
                if (positions.Length == 0)
                {
                    Map = new int[9, 9];
                    return false;
                }

                Map[i, j] = positions[Random.Range(0, positions.Length)];

            }
        }
        DrawMap();
        return true;
    }

    public void SolveSudoku()
    {
        bool created = false;
        ReadMap();
        int[,] TEMPMAP = Map.Clone() as int[,];

        while (!created)
        {
            if (solveTry <= 5000000)
            {
                solveTry++;
            }
            else
            {
                solveTry = 0;
                Debug.LogError("Could not find a solution for the Sudoku!");
                return;
            }
            Map = TEMPMAP.Clone() as int[,];
            created = TrySolveSudoku();
        }
    }

    public bool TrySolveSudoku()
    {
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                if (Map[i, j] != 0)
                    continue;

                int[] positions = GetPossiblePositions(i, j);
                if (positions.Length == 0)
                {
                    Map = new int[9, 9];
                    return false;
                }
                Map[i, j] = positions[Random.Range(0, positions.Length)];
            }
        }
        solveTry = 0;
        DrawMap();
        return true;
    }

    public void ClearMap()
    {
        Map = new int[9, 9];
        DrawMap();
    }

    public int[] GetPossiblePositions(int x, int y)
    {
        List<int> array = new List<int>();
        for (int i = 1; i <= 9; i++)
        {
            array.Add(i);
        }
        //DELETE HORIZONTAL OR VERTICAL CONTAINED NUMBERS
        for (int _X = 0; _X < Map.GetLength(0); _X++)
        {
            if (array.Contains(Map[_X, y]))
            {
                array.Remove(Map[_X, y]);
            }
        }

        for (int _Y = 0; _Y < Map.GetLength(0); _Y++)
        {
            if (array.Contains(Map[x, _Y]))
            {
                array.Remove(Map[x, _Y]);
            }
        }

        int vonX = ((int)((x + 1) / 3.1f)) * 3;
        int bisX = vonX + 2;
        int vonY = ((int)((y + 1) / 3.1f)) * 3;
        int bisY = vonY + 2;

        while (vonX <= bisX)
        {
            while (vonY <= bisY)
            {
                array.Remove(Map[vonX, vonY]);
                vonY++;
            }
            vonY = ((int)((y + 1) / 3f)) * 3;
            vonX++;
        }


        return array.ToArray();
    }

    public void CheckWin()
    {
        ReadMap();
        if (Solved())
        {
            Debug.Log("YOU WIN!");
        }else
        {
            Debug.Log("YOU DONT WIN!");
        }
    }

    public void ReadMap()
    {
        for(int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                if (int.TryParse(Text_MAP[i, j].text, out int val))
                    Map[i, j] = val;
                else
                    Map[i, j] = 0;
            }
        }
    }

    public bool Solved()
    {
        
        List<int> listHorizontal = new List<int>();
        List<int> listVertical = new List<int>();
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                if (!listHorizontal.Contains(Map[i, j]))
                    listHorizontal.Add(Map[i, j]);
                else
                    return false;

                if (!listVertical.Contains(Map[j, i]))
                    listVertical.Add(Map[j, i]);
                else
                    return false;
            }
            listHorizontal = new List<int>();
            listVertical = new List<int>();
        }
        return true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}