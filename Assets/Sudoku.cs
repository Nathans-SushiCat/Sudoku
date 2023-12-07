using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

public class Sudoku : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;
    int[,] Map = new int[9, 9];


    public void CreateScrambledMap()
    {
        CreateNewMap();
        ScrambleMap();
    }



    public void ScrambleMap()
    {
        for(int i = 0; i <= 6; i+=3)
        {
            for (int j = 0; j <= 6; j += 3)
            {
                int deleteAmmount = Random.Range(3,9);
                for(int k = 0; k < deleteAmmount; k++)
                {
                    int x = Random.Range(0, 3);
                    int y = Random.Range(0, 3);

                    if (Map[i + x, j + y] != 10)
                        Map[i + x, j + y] = 10;
                    else
                        k--;
                }
            }
        }

        DrawMap();
    }

    public void DrawMap()
    {
        string map = "";
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                if (Map[i, j] == 10)
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
                    Map = new int[9,9];
                    return false;
                }

                Map[i, j] = positions[Random.Range(0, positions.Length)];
                
            }
        }
        DrawMap();
        return true;
    }
    public int[] GetPossiblePositions(int x, int y)
    {
        List<int> array = new List<int>();
        for(int i = 1; i <= 9; i++)
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

        int vonX = ((int)((x+1) / 3.1f))*3;
        int bisX = vonX + 2;
        int vonY = ((int)((y+1) / 3.1f)) * 3;
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
}
