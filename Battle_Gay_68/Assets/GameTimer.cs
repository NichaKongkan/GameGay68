using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 60f; // ตั้งเวลาเริ่มต้น (วินาที)
    public GameObject gameOverPanel; // UI แจ้งว่าตาย
    public Button restartButton; // ปุ่มกลับไป Scene อื่น
    public Image screenFade; // UI Panel สีดำที่ค่อยๆ มืดขึ้น

    private bool isGameOver = false;
    private float maxFadeAlpha = 0.8f; // ความเข้มสูงสุดของขอบดำ

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); // ปิด UI ตายก่อนเริ่มเกม
        
        if (screenFade != null)
            screenFade.color = new Color(0, 0, 0, 0); // ตั้งให้โปร่งใสตอนเริ่ม

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

            // ทำให้ขอบจอมืดขึ้นเรื่อย ๆ
            UpdateScreenFade();
        }
    }

    void UpdateScreenFade()
    {
        if (screenFade != null)
        {
            float fadeAmount = Mathf.Clamp01(1 - (timeRemaining / 45f)); // คำนวณความเข้มของขอบดำ
            screenFade.color = new Color(255, 0, 0, fadeAmount * maxFadeAlpha);
        }
    }

    void GameOver()
    {
        isGameOver = true;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true); // แสดง UI "คุณตายแล้ว"
    }

    void RestartGame()
    {
        SceneManager.LoadScene("Main Menu"); // เปลี่ยนเป็นชื่อ Scene ที่ต้องการให้กลับไป
    }
}