using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPerson : MonoBehaviour
{
    public float speed = 5;
    public float sens = 15;
    public Canvas startMenu;
    private GameObject camera;
    private CharacterController characterController;
    private InputAction moveAction;
    private Vector3 movementVector;
    private Vector2 lookVector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        characterController = gameObject.GetComponent<CharacterController>();
        camera = GameObject.Find("FirstPersonPlayer/Camera");
    }

    // Update is called once per frame
    void Update()
    {

        lookVector = InputSystem.actions.FindAction("Look").ReadValue<Vector2>() * sens;
        transform.Rotate(Vector3.up, lookVector.x * Time.deltaTime);
        camera.transform.Rotate(Vector3.left, lookVector.y * Time.deltaTime);

        movementVector = new Vector3(moveAction.ReadValue<Vector2>().x, movementVector.y, moveAction.ReadValue<Vector2>().y);
        movementVector = movementVector * speed;
        characterController.SimpleMove(movementVector);
    }
    
    public void SpawnPlayer()
    {
        gameObject.SetActive(true);
        startMenu.enabled = false;
    }
}
