using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;

    private DialogueUI dialogueUI;
    private List<GameObject> tempResponseButtons = new List<GameObject>();

    private void Start(){
        dialogueUI = GetComponent<DialogueUI>();
    }

    public void ShowResponse(Response[] responses) {
        float responseBoxHeight = 0;

        foreach (Response response in responses) {
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response));
        
            tempResponseButtons.Add(responseButton);

            responseBoxHeight += responseButtonTemplate.sizeDelta.y;
        }

        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        responseBox.gameObject.SetActive(true);
    }

    private void OnPickedResponse(Response response) {
        float value = -1f; // ค่าปริยาย ถ้าหาไม่เจอ

        switch (response.ResponseText)
        {
            case "Yes":
                value = 1f;
                break;
            case "No":
                value = 0f;
                break;
            case "Ok":
                value = 0.5f;
                break;
        }

        //save the data
        PlayerPrefs.SetFloat("playerAgree", value);
        PlayerPrefs.Save();

        Debug.Log("Number of agree of player: " + PlayerPrefs.GetFloat("playerAgree"));


        responseBox.gameObject.SetActive(false);
        foreach (GameObject button in tempResponseButtons) {
            Destroy(button);
        }
        tempResponseButtons.Clear();
        dialogueUI.ShowDialogue(response.DialogueObject);
    }
}
