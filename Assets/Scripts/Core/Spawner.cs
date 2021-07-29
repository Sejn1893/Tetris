using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public ShapeScript[] Shapes;
    public Transform QueuedPos;
    private ShapeScript _queuedShape;

    public float QueScale = 0.5f;
    // Start is called before the first frame update


    private void Awake()
    {
        InitQueue();
    }
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    ShapeScript GetRandomShape()
    {
        int randomShape = Random.Range(0, Shapes.Length);
        if (Shapes[randomShape])
        {
            return Shapes[randomShape];
        }
        else
        {
            return null;
        }
    }
    public ShapeScript SpawnShape()
    {
        ShapeScript shape = null;
        shape = GetQueuedShape();
        shape.transform.position = transform.position;
        shape.transform.localScale = Vector3.one;



        //shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity);
        if (shape)
        {
            return shape;
        }
        else
        {
            return null;
        }
    }
    private void InitQueue()
    {
        _queuedShape = null;
        FillQueue();
    }
    private void FillQueue()
    {
        if(!_queuedShape)
        {
            _queuedShape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity);
            _queuedShape.transform.position = QueuedPos.position;
            _queuedShape.transform.localScale = new Vector3(QueScale, QueScale, QueScale);
        }
    }
    private ShapeScript GetQueuedShape()
    {
        ShapeScript nextShape = null;

        if (_queuedShape)
        {
            nextShape = _queuedShape;
        }
        _queuedShape = null;
        FillQueue();

        return nextShape;
    }

}
