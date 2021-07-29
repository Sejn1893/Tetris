using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    Board _gameBoard;
    Spawner _spawner;
    ShapeScript _activeShape;
    SceneControl _sceneControl;
    SoundManager _soundManager;
    ScoreManager _scoreManager;
    GhostCreator _ghost;

    private float _dropInterval = 1f;
    private float _dropIntervalModded;
    private float TimeToDrop;

    private float _timeToNextKey;

    public float DropDownMultiplier;
    public float KeyRepeatRate = 1f;

    private bool _gameOver = false;

    enum Direction {none, left, right, up, down}

    Direction _dragDirection = Direction.none;
    Direction _swipeDirection = Direction.none;

    float _timeToNextDrag;
    float _timeToNextSwipe;

    bool _didTap = false;

    [Range(0.05f, 1f)]
    public float MinTimeToDrag = 0.2f;
    [Range(0.05f, 1f)]
    public float MinTimeToSwipe = 0.4f;



    void OnEnable()
    {
        TouchController.DragEvent += DragHandler;
        TouchController.SwipeEvent += SwipeHandler;
        TouchController.TapEvent += TapHandler;
    }
    void OnDisable()
    {
        TouchController.DragEvent -= DragHandler;
        TouchController.SwipeEvent -= SwipeHandler;
        TouchController.TapEvent -= TapHandler;
    }




    // Start is called before the first frame update
    void Start()
    {
        _ghost = FindObjectOfType<GhostCreator>();
        _scoreManager = FindObjectOfType<ScoreManager>();
        _soundManager = FindObjectOfType<SoundManager>();
        _sceneControl = FindObjectOfType<SceneControl>();
        _gameBoard = FindObjectOfType<Board>();
        _spawner = FindObjectOfType<Spawner>();

        if (_spawner)
        {
            if (_activeShape == null)
            {
                _activeShape = _spawner.SpawnShape();
            }
            _spawner.transform.position = Vectorf.Round(_spawner.transform.position);
        }
        _dropIntervalModded = _dropInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_spawner || !_gameBoard || !_activeShape || _gameOver) return;

        PlayerInput();
    }
    void LateUpdate()
    {
        if (_ghost)
        {
            _ghost.MakeGhost(_activeShape, _gameBoard);
        }
    }
    private void PlayerInput()
    {

        //KeyboardInput
        if (Input.GetKey(KeyCode.RightArrow) && Time.time > _timeToNextKey)
        {
            MoveRight();

        }
        else if (Input.GetKey(KeyCode.LeftArrow) && Time.time > _timeToNextKey)
        {
            MoveLeft();

        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            RotateShape();

        }
        else if ((Input.GetKey(KeyCode.DownArrow) && (Time.time > _timeToNextKey)) || (Time.time > TimeToDrop))
        {
            MoveShapeDown();

        }

        // TouchInput ------------------------------------------------------------------------------------------------

        else if ((_dragDirection == Direction.right && Time.time > _timeToNextDrag) || (_swipeDirection == Direction.right && Time.time > _timeToNextSwipe))
        { 
            MoveRight();
            _timeToNextDrag = Time.time + MinTimeToDrag;
            _timeToNextSwipe = Time.time + MinTimeToSwipe;
           
        }

        else if ((_dragDirection == Direction.left && Time.time > _timeToNextDrag) || _swipeDirection == Direction.left && Time.time > _timeToNextSwipe)

        {
            MoveLeft();
            _timeToNextDrag = Time.time + MinTimeToDrag;
            _timeToNextSwipe = Time.time + MinTimeToSwipe;

        }
        else if (_didTap && Time.time > _timeToNextSwipe)
        {
            RotateShape();
            
            _didTap = false;
          
        }
        else if (_dragDirection == Direction.down && Time.time > _timeToNextDrag)
        {
            MoveShapeDown();
            _timeToNextDrag = Time.time + MinTimeToDrag;
           

        }
        
        _dragDirection = Direction.none;
        _swipeDirection = Direction.none;
        _didTap = false;
        




    }

    private void MoveShapeDown()
    {
        TimeToDrop = Time.time + _dropIntervalModded;
        _timeToNextKey = Time.time + KeyRepeatRate / DropDownMultiplier;
        _activeShape.MoveDown();

        if (!_gameBoard.IsValidPosition(_activeShape))
        {
            if (_gameBoard.IsOverLimits(_activeShape))
            {
                _activeShape.MoveUp();
                _gameOver = true;
                _sceneControl.GameOver.SetActive(true);
                PlaySound(_soundManager.GameOverSound, 1f);
            }
            else
            {
                ShapeLanded();
            }

        }
    }

    private void RotateShape()
    {
        _activeShape.RotateRight();
        _timeToNextKey = Time.time + KeyRepeatRate;
        if (!_gameBoard.IsValidPosition(_activeShape))
        {
            _activeShape.RotateLeft();
        }
    }

    private void MoveLeft()
    {
        _activeShape.MoveLeft();
        _timeToNextKey = Time.time + KeyRepeatRate;
        if (!_gameBoard.IsValidPosition(_activeShape))
        {
            _activeShape.MoveRight();
        }
        else
        {
            PlaySound(_soundManager.MoveSound, 0.5f);
        }
    }

    private void MoveRight()
    {
        _activeShape.MoveRight();
        _timeToNextKey = Time.time + KeyRepeatRate;
        if (!_gameBoard.IsValidPosition(_activeShape))
        {
            _activeShape.MoveLeft();
        }
        else
        {
            PlaySound(_soundManager.MoveSound, 0.5f);
        }
    }

    private void ShapeLanded()
    {
        _activeShape.MoveUp();
        _gameBoard.StoreShapeInGrid(_activeShape);

        _activeShape.LandShapeFX();
        if (_ghost)
        {
            _ghost.ResetGhost();
        }


        _activeShape = _spawner.SpawnShape();

        _timeToNextKey = Time.time;

        _gameBoard.ClearAllRows();
        PlaySound(_soundManager.DropSound, 4f);

        if (_scoreManager.DidLevelUp)
        {
            _dropIntervalModded = Mathf.Clamp(_dropInterval - (((float)_scoreManager.Level - 1) * 0.05f), 0.05f, 1f);
        }

        if (_gameBoard.CompletedRows > 0)
        {
        
            _scoreManager.ScoreCounter(_gameBoard.CompletedRows);
            _scoreManager.LevelUp();
            PlaySound(_soundManager.ClearRowSound, 0.5f);
        }
    }

    private void PlaySound(AudioClip clip, float volMultiplier)
    {
        if (_soundManager.FxEnabled && clip)
        {
            AudioSource.PlayClipAtPoint(clip, _activeShape.transform.position, Mathf.Clamp(_soundManager.FxVolume * volMultiplier, 0.5f, 1f));
        }
    }
    private void DragHandler(Vector2 dragMovement)
    {
        _dragDirection = GetDirection(dragMovement);
    }
    private void SwipeHandler(Vector2 swipeMovement)
    {
        _swipeDirection = GetDirection(swipeMovement);
    }
    private void TapHandler(Vector2 tapMovement)
    {
        _didTap = true;
    }
    private Direction GetDirection(Vector2 swipeMovement)
    {
        Direction swipeDir = Direction.none;
        // horizontal
        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDir = (swipeMovement.x >= 0) ? Direction.right : Direction.left;
        }
        //vertical
        else
        {
            swipeDir = (swipeMovement.y >= 0) ? Direction.up : Direction.down;
        }
        return swipeDir;
    }
}
