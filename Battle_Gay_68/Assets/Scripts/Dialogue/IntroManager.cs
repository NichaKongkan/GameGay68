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
        public int relationshipChange;  //Respone Value
        public BossID targetBoss;
    }

    public enum BossID
    {
        None,
        Boss_1,
        Boss_2,
        Boss_3
    }

    public int relationshipWithBoss;
    public TextMeshProUGUI relationshipText;

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

    public class GameStatus
    {
        public static int relationshipWithBoss = 0; //Add ID Boss
    }

    public string sceneToGo;
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
        GameStatus.relationshipWithBoss = PlayerPrefs.GetInt("RelationshipWithBoss", 0);
        UpdateRelationshipText();  // ✅ เพิ่มบรรทัดนี้
        Debug.Log($"Relationship Start: {GameStatus.relationshipWithBoss}");

        typewriterEffect = GetComponent<TypewriterEffect>();
        ShowScene(0);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextDialogue();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(sceneToGo);
        }
    }

    void ShowScene(int sceneIndex)
    {
        if (sceneIndex >= scenes.Count)
        {
            Debug.Log("จบ Intro");
            SceneManager.LoadScene(sceneToGo);
            return;
        }

        currentSceneIndex = sceneIndex;
        currentDialogueIndex = 0;
        SceneData scene = scenes[currentSceneIndex];

        // ✅ เช็คว่า Scene ปัจจุบันมีการตั้งค่าให้จบเลยไหม
        if (scene.endSceneImmediately)
        {
            Debug.Log($"จบ Scene {sceneIndex} ทันที");
            SceneManager.LoadScene(sceneToGo);
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
            button.onClick.AddListener(() => SelectResponse(response));

        }

        responsePanel.SetActive(true);  // ✅ เปิดหลังจากเซ็ตค่าเสร็จ
        LayoutRebuilder.ForceRebuildLayoutImmediate(responsePanel.GetComponent<RectTransform>()); // ✅ บังคับ UI รีเฟรช
    }


    public void SelectResponse(Response response)
    {
        if (response.targetBoss != BossID.None)
        {
            string bossKey = "RelationshipWith" + response.targetBoss.ToString();
            int currentRelationship = PlayerPrefs.GetInt(bossKey, 0);
            int newRelationship = currentRelationship + response.relationshipChange;

            PlayerPrefs.SetInt(bossKey, newRelationship);
            PlayerPrefs.Save();

            UpdateRelationshipText();  // ✅ เพิ่มบรรทัดนี้เพื่ออัปเดตค่าใน UI
            Debug.Log($"{response.targetBoss} relationship increased by {response.relationshipChange}. New value: {newRelationship}");
        }

        responsePanel.SetActive(false);
        ShowScene(response.nextSceneIndex);
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

    void UpdateRelationshipText()
    {
        string text = $"Boss 1: {PlayerPrefs.GetInt("RelationshipWithBoss_Boss_1", 0)}\n" +
                      $"Boss 2: {PlayerPrefs.GetInt("RelationshipWithBoss_Boss_2", 0)}\n" +
                      $"Boss 3: {PlayerPrefs.GetInt("RelationshipWithBoss_Boss_3", 0)}";

        if (relationshipText != null)
        {
            relationshipText.text = text;
        }
    }

}
