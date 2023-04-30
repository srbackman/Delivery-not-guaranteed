using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private bool connected = false;

    IEnumerator CheckForControllers()
    {
        while (true)
        {
            var controllers = Input.GetJoystickNames();

            if (!connected && ((controllers.Length > 0) && controllers[0] != ""))
            {
                connected = true;
                Debug.Log("Connected");
            }
            else if (connected && ((controllers.Length == 0) || controllers[0] == ""))
            {
                connected = false;
                Debug.Log("Disconnected");
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void Awake()
    {
        StartCoroutine(CheckForControllers());
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        
    }
}
