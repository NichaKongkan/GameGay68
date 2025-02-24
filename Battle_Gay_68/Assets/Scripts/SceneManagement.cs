using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName { get; private set; }

    public void SetTransitionName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetPlayerPosition();
    }

    private void SetPlayerPosition()
    {
        GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");

        if (spawnPoint != null && PlayerController.Instance != null)
        {
            PlayerController.Instance.transform.position = spawnPoint.transform.position;
            Debug.Log("Player moved to SpawnPoint: " + spawnPoint.transform.position);
            
            PlayerController.Instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        }
        else
        {
            Debug.LogWarning("SpawnPoint or Player not found!");
        }
    }
}
