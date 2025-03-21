using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 10f; // ตั้งเวลาเป็น 60 วินาที (สามารถเปลี่ยนได้)
    public GameObject gameOverPanel; // ตัวแปรเก็บ UI Game Over
    public Button restartButton; // ปุ่มกดกลับไปหน้าแรก

    private bool isGameOver = false;

    void Start()
    {
        // ซ่อน UI Game Over ตอนเริ่มเกม
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // เชื่อม Event ปุ่มกับฟังก์ชัน RestartGame
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        if (!isGameOver)
        {
            timeRemaining -= Time.deltaTime; // นับเวลาถอยหลัง
            if (timeRemaining <= 0)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        isGameOver = true;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true); // เปิด UI Game Over
    }

    void RestartGame()
    {
        SceneManager.LoadScene("Main Menu"); // เปลี่ยนเป็นชื่อ Scene ที่ต้องการให้กลับไป
    }
}
