using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public GameObject GameOver;
    public GameObject MusicOff;
    public GameObject FXOff;
    public GameObject Resume;
    public GameObject ResumeButton;

    private bool _isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        ResumeButton.SetActive(false);
        Resume.SetActive(false);
        FXOff.SetActive(false);
        MusicOff.SetActive(false);
        GameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void Paused()
    {
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            Time.timeScale = 0;
            Resume.SetActive(true);
            ResumeButton.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            Resume.SetActive(false);
            ResumeButton.SetActive(false);
        }
    }
    
}
