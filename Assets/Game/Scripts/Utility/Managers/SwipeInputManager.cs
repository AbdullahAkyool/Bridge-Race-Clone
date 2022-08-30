
using UnityEngine;

public class SwipeInputManager : MonoSingleton<SwipeInputManager>
{
    public bool SwipingLeft { get; set; }
    public bool SwipingRight { get; set; }
    public bool SwipingUp { get; set; }
    public bool SwipingDown { get; set; }

    [SerializeField] private float swipingDetectSensitivity = 100;

    private bool swiping;

    private Vector2 startInputTouch = Vector2.zero;
    private Vector2 swipingDelta = Vector2.zero;


    private void Update()
    {
        SwipingLeft = false;
        SwipingRight = false;
        SwipingDown = false;
        SwipingUp = false;

        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                swiping = true;
                startInputTouch = Input.touches[0].position;
            }
            if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                Reset();
            }
        }

        swipingDelta = Vector2.zero;

        if (startInputTouch != Vector2.zero)
        {
            if (swiping)
            {
                swipingDelta = Input.touches[0].position - startInputTouch;
            }
        }

        if (swipingDelta.magnitude > swipingDetectSensitivity)
        {
            float x = swipingDelta.x;
            float y = swipingDelta.y;

            startInputTouch = Input.touches[0].position;

            if (x < 0)
            {
                SwipingLeft = true;
                //Debug.Log("Swiping left");
            }
            else if (x > 0)
            {
                SwipingRight = true;
                // Debug.Log("Swiping right");
            }
            if (y < 0)
            {
                //aşağıya swipe
                SwipingDown = true;
            }
            else if (y > 0)
            {
                //yukarı swipe
                SwipingUp = true;
            }

            //Reset();
            //bu satır eğer tek seferde bir swipe işlemi gerçekleşşin isteniyorsa açılmalı
            //aksi halde sürekli swipe algılanır

        }
    }

    private void Reset()
    {
        startInputTouch = Vector2.zero;
        swipingDelta = Vector2.zero;
        swiping = false;
    }
}
