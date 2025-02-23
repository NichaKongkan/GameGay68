using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [System.Serializable]
    public class Response
    {
        public string responseText;  // ข้อความของตัวเลือก
        public int nextSceneIndex;   // ฉากถัดไปเมื่อเลือกตัวเลือกนี้
    }

    [System.Serializable]
    public class SceneData
    {
        public Sprite backgroundImage;  // ภาพพื้นหลัง
        public VideoClip videoClip;     // วิดีโอพื้นหลัง
        public string[] dialogues;      // บทพูดของฉาก
        public bool delayDialogueBox;
        public List<Response> responses;
    }

    public CanvasGroup dialogueCanvasGroup;
    public Image backgroundImage;
    public VideoPlayer videoPlayer;
    public TextMeshProUGUI dialogueText;
    public List<SceneData> scenes;
    public GameObject responsePanel;
    public GameObject responseButtonPrefab;

    private int currentSceneIndex = 0;
    private int currentDialogueIndex = 0;
    private TypewriterEffect typewriterEffect;

    private Coroutine typingCoroutine;

    void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>(); // หา TypewriterEffect ที่อยู่ใน GameObject เดียวกัน
        // ✅ ตรวจสอบว่า dialogueCanvasGroup ถูกกำหนดค่าแล้วหรือยัง
        if (dialogueCanvasGroup == null)
        {
            dialogueCanvasGroup = dialogueText.GetComponent<CanvasGroup>();
            if (dialogueCanvasGroup == null)
            {
                Debug.LogError("❌ ไม่พบ CanvasGroup! โปรดเพิ่มที่ Dialogue Box และกำหนดค่าใน Inspector");
            }
        }
        ShowScene(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextDialogue();
        }
    }

    void ShowScene(int sceneIndex)
    {
        if (sceneIndex >= scenes.Count)
        {
            Debug.Log("จบ Intro");
            return;
        }

        currentSceneIndex = sceneIndex;
        currentDialogueIndex = 0;
        SceneData scene = scenes[currentSceneIndex];

        if (scene.videoClip != null)
        {
            videoPlayer.clip = scene.videoClip;
            videoPlayer.gameObject.SetActive(true);
            backgroundImage.gameObject.SetActive(false);
            videoPlayer.Play();
        }
        else
        {
            backgroundImage.sprite = scene.backgroundImage;
            backgroundImage.gameObject.SetActive(true);
            videoPlayer.gameObject.SetActive(false);
        }

        dialogueCanvasGroup.alpha = 0; // เริ่มต้นซ่อนกรอบข้อความ

        // ✅ ทำให้ข้อความจางๆ แล้วค่อยโผล่
        if (scene.delayDialogueBox)
        {
            StartCoroutine(FadeInDialogue(1.5f)); // ค่อยๆ ปรากฏภายใน 1.5 วิ
        }
        else
        {
            dialogueCanvasGroup.alpha = 1; // แสดงทันที
        }

        PlayDialogue(scene.dialogues[0]);
    }

    IEnumerator FadeInDialogue(float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            dialogueCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }
    }


    void NextDialogue()
    {
        SceneData scene = scenes[currentSceneIndex];

        if (currentDialogueIndex < scene.dialogues.Length - 1)
        {
            currentDialogueIndex++;
            PlayDialogue(scene.dialogues[currentDialogueIndex]);
        }
        else
        {
            ShowResponses(scene.responses); // 🔥 เรียกใช้เมื่อลงท้ายบทสนทนา
        }
    }

    void PlayDialogue(string dialogue)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = typewriterEffect.Run(dialogue, dialogueText);
    }
    void ShowResponses(List<Response> responses)
    {
        responsePanel.SetActive(true);

        // ลบปุ่มเก่าก่อนสร้างใหม่
        foreach (Transform child in responsePanel.transform)
        {
            Destroy(child.gameObject);
        }

        // สร้างปุ่มตัวเลือก
        foreach (Response response in responses)
        {
            GameObject buttonObj = Instantiate(responseButtonPrefab, responsePanel.transform);
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = response.responseText;

            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => SelectResponse(response.nextSceneIndex));
        }
    }

    void SelectResponse(int nextSceneIndex)
    {
        responsePanel.SetActive(false); // ซ่อนตัวเลือก
        ShowScene(nextSceneIndex); // ไปยังฉากที่เลือก
    }


}
