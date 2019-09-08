using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private RectTransform crosshair;
    public PlayerMove movement;
    public float restingSize;
    public float maxSize;
    private float currentSize;

    void Start()
    {
        crosshair = GetComponent<RectTransform>();
        restingSize = 75;
        maxSize = 200;
    }

    void Update()
    {
        DynamicCrosshair();
    }

    bool isMoving
    {
        get
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                return true;
            }
            else
                return false;
        }
    }

    private void DynamicCrosshair()
    {
        if (isMoving)
        {
            currentSize = Mathf.Lerp(currentSize, maxSize - 50, Time.deltaTime * movement.GetMovementSpeed());
        }
        else if (movement.IsJump())
        {
            currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * movement.GetMovementSpeed());
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * movement.GetMovementSpeed());
        }

        crosshair.sizeDelta = new Vector2(currentSize, currentSize);
    }
}
