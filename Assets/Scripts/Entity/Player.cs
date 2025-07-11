using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputAction _moveAction;
    private InputAction _exitAction;
    private InputAction _viewAction;
    [SerializeField] private bool moveKeyHeld;

    private void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _exitAction = InputSystem.actions.FindAction("Exit");
        _viewAction = InputSystem.actions.FindAction("View");
    }

    private void OnEnable()
    {
        _moveAction.Enable();

        _moveAction.started += OnMovement;
        _moveAction.canceled += OnMovement;
        
        _exitAction.performed += OnExit;
        
        _viewAction.performed += OnView;
        
    }
    
    private void OnDisable()
    {
        _moveAction.Disable();
        
        _moveAction.started -= OnMovement;
        _moveAction.canceled -= OnMovement;
        
        _exitAction.performed -= OnExit;
        
        _viewAction.performed -= OnView;
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
        Action.EscapeAction();
    }

    public void OnView(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            UIManager.Instance.ToggleMessageHistory();
    }
    
    private void FixedUpdate()
    {
        if (!UIManager.Instance.IsMessageHistoryOpen)
        {
            if (GameManager.Instance.IsPlayerTurn && moveKeyHeld && GetComponent<Actor>().IsAlive)
                MovePlayer();
        }
    }

    private void MovePlayer()
    {
        Vector2 direction = _moveAction.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        Vector3 futurePosition = transform.position + (Vector3)roundedDirection;
        
        if (IsValidPosition(futurePosition))
            moveKeyHeld = Action.BumpAction(GetComponent<Actor>(), roundedDirection);
    }
    
    private bool IsValidPosition(Vector3 futurePosition)
    {
        Vector3Int gridPosition = MapManager.Instance.FloorMap.WorldToCell(futurePosition);
        if(!MapManager.Instance.InBounds(gridPosition.x, gridPosition.y)
           || MapManager.Instance.ObstacleMap.HasTile(gridPosition)
           || futurePosition == transform.position)
            return false;
        return true;
    }
}