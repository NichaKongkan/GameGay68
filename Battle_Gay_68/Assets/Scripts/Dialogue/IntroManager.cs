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
        public bool delayBackground; // ✅ เพิ่ม checkbox สำหรับ Background
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
        return;
    }

    currentSceneIndex = sceneIndex;
    currentDialogueIndex = 0;
    SceneData scene = scenes[currentSceneIndex];

    // ซ่อนกล่องข้อความและฉากหลังเริ่มต้น
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
            StartCoroutine(ShowBackgroundThenDialogue(scene.delayDialogueBox)); // รอให้ฉากหลัง fade ก่อนแสดง Dialogue
        }
        else if (scene.delayDialogueBox)
        {
            StartCoroutine(WaitForKeyPress(KeyCode.E)); // ถ้าติ๊กแค่ delayDialogueBox รอให้กด E
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
    yield return StartCoroutine(FadeInBackground(1f)); // รอให้ Background ค่อยๆ จางเข้ามา

    if (delayDialogueBox)
    {
        yield return StartCoroutine(WaitForKeyPress(KeyCode.E)); // รอให้กด E ก่อนแสดงกล่องข้อความ
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



    IEnumerator ShowBackgroundThenDialogue()
    {
        // รอจนกว่าผู้เล่นจะกด E
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        // แสดง Dialogue
        dialogueCanvasGroup.alpha = 1;
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

        // Ensure the dialogue index is within bounds
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
        responsePanel.SetActive(true);

        foreach (Transform child in responsePanel.transform)
        {
            Destroy(child.gameObject);
        }

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
        responsePanel.SetActive(false);
        ShowScene(nextSceneIndex);
    }

    void ShowDialogueBox()
    {
        if (currentDialogueIndex < scenes[currentSceneIndex].dialogues.Length)
        {
            string dialogue = scenes[currentSceneIndex].dialogues[currentDialogueIndex];
            PlayDialogue(dialogue); // ทำการเล่นข้อความแบบ Typewriter
        }
        dialogueCanvasGroup.alpha = 1; // ทำให้กล่องข้อความปรากฏ
    }
}
