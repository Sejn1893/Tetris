using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    private int _score = 0;
    
    public int Level = 1;

    private int _scoreForLevelUp;
    private int _scoreReset = 500;

    public Text ScoreText;
    public Text LinesText;
    public Text LevelText;

    public bool DidLevelUp = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _scoreForLevelUp = _scoreReset;
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = GetScore().ToString();
        LevelText.text = GetLevel().ToString();
    }
    public void ScoreCounter(int n)
    {
        DidLevelUp = false;
        switch (n)
        {
            case 1:
                _score += 40;
                break;
            case 2:
                _score += 80;
                break;
            case 3:
                _score += 160;
                break;
            case 4:
                _score += 320;
                break;


        }
        AddToScore(n);
        

    }

    public void LevelUp()
    {
        
        if (_score >= _scoreForLevelUp)
        {
            DidLevelUp = true;
            Level++;
            _scoreForLevelUp = _scoreReset;
            _scoreForLevelUp *= Level;
            Debug.Log(Level);
            Debug.Log(_scoreForLevelUp);
        }
        
    }

    public int GetScore()
    {
        return _score;
    }
    public void AddToScore(int scoreValue)
    {
        _score += scoreValue;
        
    }
    public int GetLevel()
    {
        return Level;
    }
    
    
}