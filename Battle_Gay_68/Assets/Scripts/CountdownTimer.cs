using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public float countdownTime = 10f; // เวลาก่อนที่ UI จะปรากฏ
    public GameObject timeUpPanel; // UI ที่จะเปิดเมื่อหมดเวลา
    public Text timerText; // UI Text แสดงเวลา
    public Button nextSceneButton; // ปุ่มสำหรับเปลี่ยน Scene
    public string nextSceneName = "Sample1"; // ชื่อ Scene ที่ต้องการเปลี่ยนไป
    public PlayerController playerController; // ตัวควบคุมการเคลื่อนที่ของ Player

    private float currentTime;

    void Start()
    {
        currentTime = countdownTime;
        timeUpPanel.SetActive(false); // ซ่อน UI ตอนเริ่มเกม

        if (nextSceneButton != null)
        {
            nextSceneButton.onClick.AddListener(GoToNextScene); // ผูกปุ่มให้เปลี่ยน Scene
        }
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (timerText != null)
                timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();
        }
        else
        {
            ShowTimeUpUI();
        }
    }

    void ShowTimeUpUI()
    {
        timeUpPanel.SetActive(true); // เปิด UI เมื่อหมดเวลา

        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    void GoToNextScene()
    {
        SceneManager.LoadScene(nextSceneName); // เปลี่ยนไปยัง Scene ที่กำหนด
    }
}
