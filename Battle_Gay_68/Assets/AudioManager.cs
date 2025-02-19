
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

    public void PlaySFX(string clip)
    {
        switch (clip)
        {
            case "death":
                SFXSource.PlayOneShot(death);
                break;
            case "checkpoint":
                SFXSource.PlayOneShot(checkpoint);
                break;
            case "wallTouch":
                SFXSource.PlayOneShot(wallTouch);
                break;
            case "portalIn":
                SFXSource.PlayOneShot(portalIn);
                break;
            case "portalOut":
                SFXSource.PlayOneShot(portalOut);
                break;
        }
    }

}
