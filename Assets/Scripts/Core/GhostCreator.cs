using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCreator : MonoBehaviour
{

    ShapeScript _ghostShape = null;
    private bool _bottomHit = false;
    public Color GhostColor = new Color(1f, 1f, 1f, 0.2f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MakeGhost(ShapeScript originalShape, Board gameBoard)
    {
        if(!_ghostShape)
        {
            _ghostShape = Instantiate(originalShape, originalShape.transform.position, originalShape.transform.rotation);
            _ghostShape.gameObject.name = "GhostShape";

            SpriteRenderer[] allRenderers = _ghostShape.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer r in allRenderers)
            {
                r.color = GhostColor;
            }
        }
        else
        {
            _ghostShape.transform.position = originalShape.transform.position;
            _ghostShape.transform.rotation = originalShape.transform.rotation;
        }

        _bottomHit = false;

        while (!_bottomHit)
        {
            _ghostShape.MoveDown();
            if (!gameBoard.IsValidPosition(_ghostShape))
            {
                _ghostShape.MoveUp();
                _bottomHit = true;  
            }
        }
    }
    public void ResetGhost()
    {
        Destroy(_ghostShape.gameObject);    
    }
}
