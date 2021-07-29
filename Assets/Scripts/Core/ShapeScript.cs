using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeScript : MonoBehaviour
{

    public bool CanRotate = true;

    private GameObject[] _shapeGlowFX;
    public string ShapeGlowFXTag = "LandShapeFX";
    // Start is called before the first frame update
    void Start()
    {
        _shapeGlowFX = GameObject.FindGameObjectsWithTag(ShapeGlowFXTag);
       // InvokeRepeating("MoveDown", 0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LandShapeFX()
    {
        int i = 0;

        foreach (Transform item in gameObject.transform)
        {
            if (_shapeGlowFX[i])
            {
                _shapeGlowFX[i].transform.position = new Vector3(item.transform.position.x, item.transform.position.y, -2);
                ParticlePlayer particlePlayer = _shapeGlowFX[i].GetComponent<ParticlePlayer>();
                particlePlayer.PlayFX();
            }
            i++;
        }
    }
    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }
    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));
    }
    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }
    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }
    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }
    public void RotateRight()
    {
        if (CanRotate)
        {

            transform.Rotate(0, 0, 90);
        }
    }
    public void RotateLeft()
    {
        if (CanRotate)
        {
            transform.Rotate(0, 0, -90);
        }
    }
      
}
