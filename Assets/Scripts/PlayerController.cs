using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Player Setting")]
    [SerializeField] private float _interactableDistance = 2.0f;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _mouseSensativity = 100.0f;
    [SerializeField] private float _minCameraView = -70.0f;
    [SerializeField] private float _maxCameraView = 80.0f;

    [Header("Sound Effect")]
    public AudioSource[] _clip;

    [Header("Reference")]
    [SerializeField] public Camera _camera;
    [SerializeField] private CharacterController _characterController = null;

    protected float vertRot;
    [SerializeField] private float _rotSpeed;
    [SerializeField] private float _rotationLimit;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded = false;
    public Vector3 _playerVelocity;
    private float gravity = -9.18f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        } 
    }
       
    // Update is called once per frame
    void Update()
    {        
        //Character Rotation
        if (Input.GetMouseButton(0))
        {
            CharacterRotation();
        }

        //Character Movement
        CharacterMove();

    
    }

    public void CharacterMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && _playerVelocity.y < 0)
        {
            //Debug.Log("isGrounded true");
            _playerVelocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //move player based on W,A,S,D
        Vector3 move = transform.forward * z + transform.right * x;
        _characterController.Move(move * _speed * Time.deltaTime);

        _playerVelocity.y += gravity * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);

        if (x != 0.0f || z != 0.0f)
        {
            if (!_clip[0].isPlaying)
            {
                _clip[0].volume = Random.Range(0.8f, 1.0f);
                _clip[0].pitch = Random.Range(0.8f, 1.1f);
                _clip[0].Play();
            }
        }
    }
    public virtual void CharacterRotation()
    {
        vertRot -= GetVerticalValue();
        vertRot = vertRot <= -_rotationLimit ? -_rotationLimit :
                  vertRot >= _rotationLimit ? _rotationLimit :
                  vertRot;

        RotateVertical();
        RotateHorizontal();
    }
    protected float GetVerticalValue() => Input.GetAxis("Mouse Y") * _rotSpeed * Time.deltaTime;
    protected float GetHorizontalValue() => Input.GetAxis("Mouse X") * _rotSpeed * Time.deltaTime;
    protected virtual void RotateVertical() => _camera.transform.localRotation = Quaternion.Euler(vertRot, 0f, 0f);
    protected virtual void RotateHorizontal() => transform.Rotate(Vector3.up * GetHorizontalValue());
}
