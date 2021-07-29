using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public Transform EmptyCell;
    public int Width = 10;
    public int Height = 30;
    public int Header = 8;

    private Transform[,] _grid;

    public ParticlePlayer[] RowGlowFX = new ParticlePlayer[4];

    public int CompletedRows = 0;
    private void Awake()
    {
        _grid = new Transform[Width, Height];
    }
    // Start is called before the first frame update
    void Start()
    {
        DrawEmptyCells();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DrawEmptyCells()
    {
        if (EmptyCell != null)
        {
            for (int i = 0; i < Height - Header; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Transform cloneCell;
                    cloneCell = Instantiate(EmptyCell, new Vector3(j, i, 0), Quaternion.identity);
                    cloneCell.transform.parent = transform;

                }
            }
        }
        else
        {
            Debug.Log("EmptyCell not assigned");
        }
    }
    bool IsWhithinBoard(int x,int y)
    {
        return (x >= 0 && x < Width && y >= 0);
    }
    bool IsOccupied(int x, int y, ShapeScript shape)
    {
        return _grid[x, y] != null && _grid[x, y].parent != shape.transform;
    }
    public bool IsValidPosition(ShapeScript shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);

            if (!IsWhithinBoard((int) pos.x, (int) pos.y))
            {
                return false;
            }
            if (IsOccupied((int)pos.x, (int)pos.y,shape))
            {
                return false;    
            }
        }
        return true;
        
    }
    
    public void StoreShapeInGrid(ShapeScript shape)
    {
        if (shape == null) return;
        
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            _grid[(int)pos.x, (int)pos.y] = child; 
        }    
    }
    private bool IsRowFull(int y)
    {
        for (int i = 0; i < Width; i++)
        {
            if (_grid[i,y] == null)
            {
                return false;
            }
            
        }
        return true;
    }
    private void ClearRow(int y)
    {
        for (int i = 0; i < Width; i++)
        {
            if (_grid[i,y] != null)
            {
                Destroy(_grid[i, y].gameObject);
            }
            _grid[i, y] = null;
        }
    }
    private void MoveOneRowDown(int y)
    {
        for (int i = 0; i < Width; i++)
        {
            if (_grid[i,y] != null)
            {
                _grid[i, y - 1] = _grid[i, y];
                _grid[i, y] = null;
                _grid[i, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }
    private void MoveAllRowsDown(int Starty)
    {
        for (int i = Starty; i < Height; i++)
        {
            MoveOneRowDown(i);
        }

    }
    public void ClearAllRows()
    {

        CompletedRows = 0;
        for (int i = 0; i < Height; i++)
        {
            if (IsRowFull(i))
            {
                ClearRowFX(CompletedRows, i);
                CompletedRows++;
            }
        }
        for (int i = 0; i < Height; i++)
        {
            if (IsRowFull(i))
            {
       
                ClearRow(i);
                MoveAllRowsDown(i + 1);
                i--;
            }
        }
    }
    public bool IsOverLimits(ShapeScript shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= Height - Header -1)
            {
                return true;
            }
        }
        return false;
    }
    private void ClearRowFX(int idx, int y)
    {
        if (RowGlowFX[idx])
        {
            RowGlowFX[idx].transform.position = new Vector3(0, y, -2);
            RowGlowFX[idx].PlayFX();
        }
    }
}
