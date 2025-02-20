using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoChanger : MonoBehaviour
{
    public VideoPlayer videoPlayer; // ตัว Video Player
    public VideoClip[] videoClips; // วิดีโอหลายตัวที่ต้องการใช้
    private int currentIndex = 0; // วิดีโอตัวปัจจุบัน

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // เมื่อกดปุ่ม E
        {
            ChangeVideo();
        }
    }

    void ChangeVideo()
    {
        if (videoClips.Length == 0) return; // ถ้าไม่มีวิดีโอ ไม่ต้องทำอะไร

        currentIndex = (currentIndex + 1) % videoClips.Length; // วนลูปเปลี่ยนวิดีโอ
        videoPlayer.clip = videoClips[currentIndex]; // เปลี่ยนคลิปวิดีโอ
        videoPlayer.Play(); // เล่นวิดีโอใหม่
    }
}
