using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [System.Serializable]
    public class Response
    {
        public string responseText;
        public int nextSceneIndex;
    }

    [System.Serializable]
    public class SceneData
    {
        public Sprite backgroundImage;
        public VideoClip videoClip;
        public string[] dialogues;
        public bool delayDialogueBox;
        public bool delayBackground;
        public List<Response> responses;
        public bool endSceneImmediately; // ✅ เพิ่มตัวเลือกแยกตามแต่ละ Scene
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
        typewriterEffect = GetComponent<TypewriterEffect>();
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
            SceneManager.LoadScene("Sample1");
            return;
        }

        currentSceneIndex = sceneIndex;
        currentDialogueIndex = 0;
        SceneData scene = scenes[currentSceneIndex];

        // ✅ เช็คว่า Scene ปัจจุบันมีการตั้งค่าให้จบเลยไหม
        if (scene.endSceneImmediately)
        {
            Debug.Log($"จบ Scene {sceneIndex} ทันที");
            SceneManager.LoadScene("Sample1");
            return;
        }

        dialogueCanvasGroup.alpha = 0;
        backgroundImage.color = new Color(1, 1, 1, scene.delayBackground ? 0 : 1);

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

            if (scene.delayBackground)
            {
                StartCoroutine(ShowBackgroundThenDialogue(scene.delayDialogueBox));
            }
            else if (scene.delayDialogueBox)
            {
                StartCoroutine(WaitForKeyPress(KeyCode.E));
            }
            else
            {
                dialogueCanvasGroup.alpha = 1;
                ShowDialogueBox();
            }
        }
    }


    IEnumerator ShowBackgroundThenDialogue(bool delayDialogueBox)
    {
        yield return StartCoroutine(FadeInBackground(1f));

        if (delayDialogueBox)
        {
            yield return StartCoroutine(WaitForKeyPress(KeyCode.E));
        }

        dialogueCanvasGroup.alpha = 1;
        ShowDialogueBox();
    }

    private IEnumerator WaitForKeyPress(KeyCode key)
    {
        while (!Input.GetKeyDown(key))
        {
            yield return null;
        }

        dialogueCanvasGroup.alpha = 1;
        ShowDialogueBox();
    }

    IEnumerator FadeInBackground(float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            backgroundImage.color = new Color(1, 1, 1, alpha);
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
            if (scene.responses == null || scene.responses.Count == 0)
            {
                ShowScene(currentSceneIndex + 1);
            }
            else
            {
                ShowResponses(scene.responses);
            }
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
        foreach (Transform child in responsePanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Response response in responses)
        {
            GameObject buttonObj = Instantiate(responseButtonPrefab, responsePanel.transform);
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = response.responseText;  // ✅ เซ็ตค่าก่อนเปิด Panel  
            buttonText.fontSize = 120;

            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => SelectResponse(response.nextSceneIndex));
        }

        responsePanel.SetActive(true);  // ✅ เปิดหลังจากเซ็ตค่าเสร็จ
        LayoutRebuilder.ForceRebuildLayoutImmediate(responsePanel.GetComponent<RectTransform>()); // ✅ บังคับ UI รีเฟรช
    }


    void SelectResponse(int nextSceneIndex)
    {
        responsePanel.SetActive(false);
        ShowScene(nextSceneIndex);
    }

    void ShowDialogueBox()
    {
        if (currentDialogueIndex < scenes[currentSceneIndex].dialogues.Length)
        {
            string dialogue = scenes[currentSceneIndex].dialogues[currentDialogueIndex];
            PlayDialogue(dialogue);
        }
        dialogueCanvasGroup.alpha = 1;
    }
}
