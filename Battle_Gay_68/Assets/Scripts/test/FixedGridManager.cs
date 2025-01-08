using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedGridManager : MonoBehaviour
{
    public GameObject redCandyPrefab;
    public GameObject blueCandyPrefab;
    public GameObject greenCandyPrefab;
    public int width = 5;  // ความกว้างของตาราง
    public int height = 5; // ความสูงของตาราง

    public GameObject[,] gridArray;  // เก็บข้อมูลไอเท็มในตาราง

    // กำหนดสีของแต่ละตำแหน่งในตาราง (R = แดง, B = ฟ้า, G = เขียว)
    public string[,] predefinedGrid = new string[5, 5]
    {
        { "R", "B", "G", "R", "B" },
        { "G", "G", "B", "R", "G" },
        { "B", "R", "R", "B", "G" },
        { "G", "B", "B", "G", "R" },
        { "R", "G", "R", "B", "B" }
    };

    void Start()
    {
        gridArray = new GameObject[width, height];
        InitializeFixedGrid();
    }

    void InitializeFixedGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // เลือก Prefab ตามสีที่กำหนดไว้ใน predefinedGrid
                GameObject selectedPrefab = null;

                switch (predefinedGrid[x, y])
                {
                    case "R":
                        selectedPrefab = redCandyPrefab;
                        break;
                    case "B":
                        selectedPrefab = blueCandyPrefab;
                        break;
                    case "G":
                        selectedPrefab = greenCandyPrefab;
                        break;
                }

                if (selectedPrefab != null)
                {
                    // สร้างไอเท็มในตำแหน่งที่กำหนด
                    Vector2 position = new Vector2(x, y);
                    GameObject candy = Instantiate(selectedPrefab, position, Quaternion.identity);

                    // เก็บไว้ใน gridArray
                    gridArray[x, y] = candy;
                }
            }
        }
    }
}
