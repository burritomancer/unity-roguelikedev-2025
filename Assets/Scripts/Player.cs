using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputAction _moveAction;
    private InputAction _exitAction;
    [SerializeField] private bool moveKeyHeld;

    private void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _exitAction = InputSystem.actions.FindAction("Exit");
    }

    private void OnEnable()
    {
        _moveAction.Enable();

        _moveAction.started += OnMovement;
        _moveAction.canceled += OnMovement;
        
        _exitAction.performed += OnExit;
    }
    
    private void OnDisable()
    {
        _moveAction.Disable();
        
        _moveAction.started -= OnMovement;
        _moveAction.canceled -= OnMovement;
        
        _exitAction.performed -= OnExit;
    }
    
    private void OnMovement(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            moveKeyHeld = true;
        else if (ctx.canceled)
            moveKeyHeld = false;
    }
    
    private void OnExit(InputAction.CallbackContext ctx)
    {
        Debug.Log("Exit");
    }
    
    private void FixedUpdate()
    {
        if(GameManager.Instance.IsPlayerTurn && moveKeyHeld)
            MovePlayer();
    }

    private void MovePlayer()
    {
        transform.position += (Vector3)_moveAction.ReadValue<Vector2>();
        GameManager.Instance.EndTurn();
    }
}