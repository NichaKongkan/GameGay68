using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CandyGridManager : MonoBehaviour
{
    public FixedGridManager gridManager;
    public CandyItem candyItem;
    public GameObject[,] grid;
    

    void Start()
    {
        gridManager = GetComponent<FixedGridManager>();
        Array test = gridManager.gridArray;


        if (candyItem != null)
        {
            candyItem.OnMouseDown();
        }
        else
        {
            Debug.Log("Did't use code in CandyItem");
        }

        
    }

    public void SwapItems(int x, int y)
    {
        // ตรวจสอบตำแหน่งรอบๆ [x, y] ว่ามีอยู่ในขอบเขตของ array หรือไม่
        if (x > 0) Swap(x, y, x - 1, y); // ขึ้น
        if (x < 4) Swap(x, y, x + 1, y); // ลง
        if (y > 0) Swap(x, y, x, y - 1); // ซ้าย
        if (y < 4) Swap(x, y, x, y + 1); // ขวา
    }

    // ฟังก์ชันสำหรับการสลับ
    void Swap(int x1, int y1, int x2, int y2)
    {
        string temp = gridManager.predefinedGrid[x1, y1];
        gridManager.predefinedGrid[x1, y1] = gridManager.predefinedGrid[x2, y2];
        gridManager.predefinedGrid[x2, y2] = temp;

        // พิมพ์ผลลัพธ์หลังการสลับ
        Debug.Log("Swapped: [" + x1 + "," + y1 + "] with [" + x2 + "," + y2 + "]");
    }


    /**

    


    

    public void SwapItems(GameObject selectedItem, Vector2 targetPosition)
    {
        GameObject targetItem = FindItemAtPosition(targetPosition);

        if (targetItem == null)
        {
            Debug.LogError("Target item not found at position: " + targetPosition);
            return;
        }

        if (IsAdjacent(selectedItem, targetItem))
        {
            // สลับตำแหน่ง
            Vector2 selectedPos = selectedItem.transform.position;
            selectedItem.transform.position = targetItem.transform.position;
            targetItem.transform.position = selectedPos;

            // ตรวจจับการจับคู่
            if (CheckMatch(selectedItem) || CheckMatch(targetItem))
            {
                DestroyMatchedItems();
                DropItems();
            }
            else
            {
                // สลับกลับถ้าไม่มีการจับคู่
                targetItem.transform.position = selectedPos;
                selectedItem.transform.position = targetItem.transform.position;
            }
        }
        else
        {
            Debug.Log("Items are not adjacent.");
        }
    }

    private GameObject FindItemAtPosition(Vector2 position)
    {
        int rows = gridManager.width;
        int columns = gridManager.height;

        if (grid == null)
        {
            Debug.LogError("Grid is null! Ensure it's initialized in Start.");
            return null;
        }

        int row = Mathf.RoundToInt(position.y);
        int col = Mathf.RoundToInt(position.x);
    
        if (row >= 0 && row < rows && col >= 0 && col < columns)
        {
            return grid[row, col];
        }
    
        Debug.LogWarning($"Position {position} is out of bounds!");
        return null;
    }

    private bool IsAdjacent(GameObject item1, GameObject item2)
    {
        Vector2 pos1 = item1.transform.position;
        Vector2 pos2 = item2.transform.position;

        return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y) == 1; // เช็คเฉพาะทิศทาง 4 ด้าน
    }

    private bool CheckMatch(GameObject item)
    {
        // ใส่ตรรกะตรวจจับการจับคู่ (3 ชิ้นติดกัน)
        return false; // ตัวอย่างเบื้องต้น
    }

    private void DestroyMatchedItems()
    {
        // ใส่ตรรกะทำลายไอเทมที่จับคู่ได้
    }

    private void DropItems()
    {
        // ใส่ตรรกะให้ไอเทมตกลงมา
    }
    */
}
