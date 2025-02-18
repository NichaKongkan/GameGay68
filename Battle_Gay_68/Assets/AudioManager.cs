
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------------ Audio Sources ------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("------------ Audio Clips ------------")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip checkpoint;
    public AudioClip wallTouch;
    public AudioClip portalIn;
    public AudioClip portalOut;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

}
