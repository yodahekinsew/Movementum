using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Animator anim;

    [Header("Audio Sources")]
    public AudioSource gameLoop;
    public AudioSource startLoop;
    public AudioSource endLoop;
    public AudioSource sfx;

    [Header("Volumes")]
    [Range(0f, 1f)]
    public float gameVolume;
    [Range(0f, 1f)]
    public float gameVolumeMultiplier;
    [Range(0f, 1f)]
    public float startVolume;
    [Range(0f, 1f)]
    public float startVolumeMultiplier;
    [Range(0f, 1f)]
    public float endVolume;
    [Range(0f, 1f)]
    public float endVolumeMultiplier;
    [Range(0f, 1f)]
    public float sfxVolume;
    [Range(0f, 1f)]
    public float sfxVolumeMultiplier;

    [Header("Controls")]
    public Slider audioSlider;

    private float mainVolume;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", .5f);
            mainVolume = .5f;
            audioSlider.value = .5f;
        }
        else
        {
            mainVolume = PlayerPrefs.GetFloat("Volume");
            audioSlider.value = mainVolume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameLoop.volume = mainVolume * gameVolume * gameVolumeMultiplier;
        startLoop.volume = mainVolume * startVolume * startVolumeMultiplier;
        endLoop.volume = mainVolume * endVolume * endVolumeMultiplier;
        sfx.volume = mainVolume * sfxVolume * sfxVolumeMultiplier;
    }

    public void ChangeAudio()
    {
        mainVolume = audioSlider.value;
        PlayerPrefs.SetFloat("Volume", mainVolume);
    }

    public void StartPlaying()
    {
        anim.SetTrigger("StartGame");
        gameLoop.Stop();
        gameLoop.Play();
    }

    public void EndGame()
    {
        // Play explosion
        sfx.Play();

        anim.SetTrigger("EndGame");
        endLoop.Stop();
        endLoop.Play();
    }

    public void StopPlaying()
    {
        anim.SetTrigger("BackStart");
        startLoop.Stop();
        startLoop.Play();
    }

    public void PlayAgain()
    {
        anim.SetTrigger("PlayAgain");
        gameLoop.Stop();
        gameLoop.Play();
    }

    public void PlaySFX(AudioClip clip, float volumeScale = 1.0f)
    {
        sfx.PlayOneShot(clip, volumeScale);
    }
}
