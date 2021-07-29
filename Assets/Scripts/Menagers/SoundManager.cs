using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public bool MusicEnabled = true;
    public bool FxEnabled = true;

    [Range(0,1)]
    public float MusicVolume = 1f;

    [Range(0,1)]
    public float FxVolume = 1f;

    public AudioClip ClearRowSound;

    public AudioClip MoveSound;

    public AudioClip GameOverSound;

    public AudioClip DropSound;

    public AudioClip BackgroundMusic;

    public AudioSource MusicSource;

    SceneControl _sceneControl;
    // Start is called before the first frame update
    void Start()
    {
        _sceneControl = FindObjectOfType<SceneControl>();
        PlayBackgroundMusic(BackgroundMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (!MusicEnabled || !musicClip || !MusicSource) return;

        MusicSource.Stop();

        MusicSource.clip = musicClip;

        MusicSource.volume = MusicVolume;

        MusicSource.loop = true;

        MusicSource.Play();
        
    }

    private void UpdateMusic()
    {
        if (MusicSource.isPlaying != MusicEnabled)
        {
            if (MusicEnabled)
            {
                PlayBackgroundMusic(BackgroundMusic);
            }
            else
            {
                MusicSource.Stop();
                
            }
        }
    }
    public void ToggleMusic()
    {
        MusicEnabled = !MusicEnabled;
        IsMusicEnabled();
        UpdateMusic();
    }
    public void ToggleFX()
    {
        FxEnabled = !FxEnabled;
        IsFXEnabled();
    }
    private void IsMusicEnabled()
    {
        if (!MusicEnabled)
        {
            _sceneControl.MusicOff.SetActive(true);
        }
        else
        {
            _sceneControl.MusicOff.SetActive(false);
        }
    }
    private void IsFXEnabled()
    {
        if(!FxEnabled)
        {
            _sceneControl.FXOff.SetActive(true);
        }
        else
        {
            _sceneControl.FXOff.SetActive(false);
        }
    }
}
