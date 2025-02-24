using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetPlayerCameraFollow();
    }

    public void SetPlayerCameraFollow()
    {
        Debug.Log("Camera setup for new scene.");
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (cinemachineVirtualCamera != null && PlayerController.Instance != null)
        {
            cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
            Debug.Log("Camera is now following the player.");
        }
        else
        {
            Debug.LogWarning("Camera or Player not found!");
        }
    }
}
