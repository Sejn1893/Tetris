using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{

    public delegate void TouchEventHandler(Vector2 swipe);

    public static event TouchEventHandler DragEvent;
    public static event TouchEventHandler SwipeEvent;
    public static event TouchEventHandler TapEvent;

    Vector2 _touchMovement;

    float _tapMaxTime = 0;
    public float TimeToNextTap = 0.2f;

    [Range(50, 150)]
    public int MinDragDistance = 100;
    [Range(50, 250)]
    public int MinSwipeDistance = 200;

    private void OnTap()
    {
        if (TapEvent != null)
        {
            TapEvent(_touchMovement);
        }
    }
    private void OnDrag()
    {
        if (DragEvent != null)
        {
            DragEvent(_touchMovement);
        }
    }
    private void OnSwipe()
    {
        if (SwipeEvent != null)
        {
            SwipeEvent(_touchMovement);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                _tapMaxTime = Time.time + TimeToNextTap;
                _touchMovement = Vector2.zero;
            }
            else if ((touch.phase == TouchPhase.Moved) || (touch.phase == TouchPhase.Stationary))
            {

                if (Mathf.Sign(_touchMovement.x) != Mathf.Sign(touch.deltaPosition.x) && Mathf.Abs(touch.deltaPosition.x) > 2f)
                {
                    _touchMovement = new Vector2(-_touchMovement.x, _touchMovement.y);
                }

                _touchMovement += touch.deltaPosition;

                if (_touchMovement.magnitude > MinDragDistance)
                {
                    OnDrag();
                }

            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (_touchMovement.magnitude > MinSwipeDistance)
                {
                    OnSwipe();
                }
                else if (Time.time < _tapMaxTime)
                {
                    OnTap();
                }
            }
        }
    }
}
