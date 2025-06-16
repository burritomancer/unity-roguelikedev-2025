using UnityEngine;

public static class Action
{
    public static void EscapeAction()
    {
        Debug.Log("Quit");
    }
    
    public static void MovementAction(Entity entity, Vector2 direction)
    {
        entity.Move(direction);
        entity.UpdateFieldOfView();
        GameManager.Instance.EndTurn();
    }
    
    public static void SkipAction(Entity entity)
    {
        GameManager.Instance.EndTurn();
    }
}
