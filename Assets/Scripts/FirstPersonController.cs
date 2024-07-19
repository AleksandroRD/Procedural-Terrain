using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private float _walkingSpeed = 7.5f;
    [SerializeField] private float _runningSpeed = 11.5f;
    [SerializeField] private float _gravity = 20.0f;
    [SerializeField] private float _cameraSensitivity = 2.0f;
    [SerializeField] private float _maxPitch = 85;
    [SerializeField] private float _minPitch = -90;

    private CharacterController _characterController;
    private float _rotationX = 0;

    private Camera _playerCamera;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float speed = Input.GetKey(KeyCode.LeftShift) ? _runningSpeed : _walkingSpeed;
        
        Vector3 movement = new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime;
        movement = transform.TransformDirection(movement);

        _characterController.Move(movement);

        // Simulate gravity
        _characterController.Move(Vector3.down * Time.deltaTime * _gravity);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * _cameraSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _cameraSensitivity;

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, _minPitch, _maxPitch);

        _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
        transform.rotation *= Quaternion.Euler(0f, mouseX, 0f);
    }

}
