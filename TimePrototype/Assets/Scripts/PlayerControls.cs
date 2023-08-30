using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    [SerializeField] private CharacterController _controller;



    public InputActions _playerInputActions;

    [SerializeField] private float _moveSpeed = 15.0f;

    [SerializeField] private float sensitivity = 0.1f;

    private float _lookRotation;
    [SerializeField] private GameObject _camHolder;

    [SerializeField] private float _dashDistance = 12.0f;

    private TimeRewind _timeRewind;

    private void Awake()
    {



        Cursor.lockState = CursorLockMode.Locked;
        
        _playerInputActions = new InputActions();

        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Atack.performed += Atack;
        _playerInputActions.Player.Dash.performed += Dash;
        _playerInputActions.Player.Rewind.performed += Rewind;

        ////Limb
        //_playerInputActions.Limb.Enable();
        // //_playerInputActions.Limb.Jump.performed += Jump;
        //_playerInputActions.Limb.BecomeStatue.performed += BecomeStatue;

        ////Statue
        //_playerInputActions.Statue.BecomeLeftArm.performed += BecomeLeftArm;
        //_playerInputActions.Statue.BecomeRightArm.performed += BecomeRightArm;
        //_playerInputActions.Statue.BecomeLeftLeg.performed += BecomeLeftLeg;
        //_playerInputActions.Statue.BecomeRightLeg.performed += BecomeRightLeg;
    }

    private void Start()
    {
        _timeRewind = GetComponent<TimeRewind>();
    }

    private void FixedUpdate()
    {
        if (_playerInputActions.Player.enabled)
        {

            Vector2 moveInputVec = _playerInputActions.Player.Move.ReadValue<Vector2>();

            Vector3 move = transform.right * moveInputVec.x + transform.forward * moveInputVec.y;



            _controller.Move(move * _moveSpeed * Time.fixedDeltaTime);

        }
    }

    

    private void LateUpdate()
    {
        if (_playerInputActions.Player.enabled)
        {
            Vector2 lookInputVec = _playerInputActions.Player.Look.ReadValue<Vector2>();

            //Horizontal
            transform.Rotate(Vector3.up * lookInputVec.x * sensitivity);

            //Vertical
            _lookRotation += (-lookInputVec.y * sensitivity);
            _lookRotation = Mathf.Clamp(_lookRotation, -90, 90);
            _camHolder.transform.eulerAngles = new Vector3(_lookRotation, _camHolder.transform.eulerAngles.y, _camHolder.transform.eulerAngles.z);

        }
     
    }

    public void Atack(InputAction.CallbackContext context)
    {
        //if (context.performed)
        //{
            Debug.Log("Atack");
        //    _controller.Move(new Vector3(0, 2, 0));
        //}
    }

    public void Dash(InputAction.CallbackContext context)
    {
        //if (context.performed)
        //{
        Debug.Log("Dash");

        Vector3 dashDirection = _camHolder.transform.forward;
        dashDirection.y = 0;
        dashDirection.Normalize();
        dashDirection *= _dashDistance;

        _controller.Move(dashDirection);
        //}
    }

    public void Rewind(InputAction.CallbackContext context)
    {
  
        Debug.Log("Rewind");
        _timeRewind.Rewind();
     
    
    }

}
