using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private string mouseXInputName, mouseYInputName;
    [SerializeField] private float mouseSensitivity;

    [SerializeField] private Transform playerBody;

    private float xAxisClamp;
    public GameObject interact;
    public GameObject flashlight;
    public GameObject door;

    private void Awake()
    {
        LockCursor();
        xAxisClamp = 0.0f;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (PauseMenu.gameIsPause)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            LockCursor();
        }
        CameraRotation();
        InteractGuide();
    }


    void InteractGuide()
    {
        RaycastHit whatIHit;
        if (Physics.Raycast(transform.position, transform.forward, out whatIHit, Mathf.Infinity,9))
        {
            if (whatIHit.collider.tag != "Untagged" && whatIHit.collider.tag != "Player" && Vector3.Distance(transform.position, whatIHit.collider.transform.position) <= 3f)
            {
                interact.SetActive(true);
                if (whatIHit.collider.tag == "Flashlight" && Input.GetKey(KeyCode.E))
                {
                    flashlight.SetActive(true);
                    Destroy(GameObject.FindWithTag("Flashlight"));
                }
                if(whatIHit.collider.tag == "Door" && Input.GetKey(KeyCode.E))
                {
                    door.GetComponent<Animator>().SetBool("open", true);
                }
            }
            else
            {
                interact.SetActive(false);
            }
        }
    }


    private void CameraRotation()
    {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        if (xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }

    /*private void CheckForShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit whatIHit;
            if(Physics.Raycast(transform.position, transform.forward, out whatIHit, Mathf.Infinity))
            {
                Debug.Log(whatIHit.collider.name);
            }
        }
    }*/
}
