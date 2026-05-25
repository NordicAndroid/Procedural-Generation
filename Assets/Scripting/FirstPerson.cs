using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class FirstPerson : MonoBehaviour
{
    
    public Canvas startMenu;
    public float sensX;
    public float sensY;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //Get Mouse input
        //float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
    }


    public void SpawnPlayer()
    {
        gameObject.SetActive(true);
        startMenu.enabled = false;
    }
}
