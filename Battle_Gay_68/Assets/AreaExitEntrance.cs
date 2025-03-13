using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExitEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void Start() {
        Debug.Log($"TransitionName: {transitionName} | SceneTransitionName: {SceneManagement.Instance.SceneTransitionName}");

        if (transitionName == SceneManagement.Instance.SceneTransitionName) {
            if (PlayerController.Instance != null) {
                PlayerController.Instance.transform.position = this.transform.position;
                Debug.Log("PlayerController moved successfully.");
            }
            else {
                Debug.LogWarning("PlayerController.Instance is null! Check if PlayerController is present in the scene.");
            }

            StartCoroutine(FadeInRoutine());  // 🔹 เพิ่มการดีเลย์ให้ FadeToClear() ทำงานได้แน่นอน
        }
    }

    private IEnumerator FadeInRoutine()
    {
        yield return new WaitForSeconds(0.5f);  // 🔹 ดีเลย์ครึ่งวินาที
        UIFade.Instance.FadeToClear();
        Debug.Log("UIFade.Instance.FadeToClear() called.");
    }
}
