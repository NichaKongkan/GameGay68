using UnityEngine;

public class CandyItem : MonoBehaviour
{
    public Vector2 initialPosition;
    public CandyGridManager gridManager;
    private FixedGridManager fixedGridManager;

    void Start()
    {
        gridManager = FindObjectOfType<CandyGridManager>();
        fixedGridManager = FindObjectOfType<FixedGridManager>();
    }

    public void OnMouseDown()
    {
        // เก็บตำแหน่งของไอเทมที่ถูกคลิก
        initialPosition = transform.position;

        // คำนวณตำแหน่งใน array (ให้เป็น integer)
        int x = Mathf.RoundToInt(initialPosition.x);
        int y = Mathf.RoundToInt(initialPosition.y);

        // ตรวจสอบว่า x, y อยู่ในขอบเขตของ gridArray หรือไม่
        if (x >= 0 && x < fixedGridManager.width && y >= 0 && y < fixedGridManager.height)
        {
            // ดึงสีของไอเทมจาก predefinedGrid
            string itemColor = fixedGridManager.predefinedGrid[x, y];

            // แสดงผลข้อมูลใน Debug.Log
            Debug.Log("Player selected: " + itemColor + " candy at position [" + x + "," + y + "]");
        }
        else
        {
            Debug.LogWarning("Clicked position is out of bounds!");
        }
    }

    
    public void OnMouseUp()
    {
        // หาไอเทมที่ปล่อยเมาส์เพื่อสลับตำแหน่ง
        Vector2 releasePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //gridManager.SwapItems(this.gameObject, releasePosition);
    }
    
}
