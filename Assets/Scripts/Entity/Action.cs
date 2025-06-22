using UnityEngine;

public static class Action
{
    public static void EscapeAction()
    {
        Debug.Log("Quit");
    }

    public static bool BumpAction(Actor actor, Vector2 direction)
    {
        Actor target = GameManager.Instance.GetBlockingActorAtLocation(actor.transform.position + (Vector3)direction);

        if (target)
        {
            MeleeAction(actor, target);
            return false;
        }
        else
        {
            MovementAction(actor, direction);
            return true;
        }
    }

    public static void MeleeAction(Actor actor, Actor target)
    {
        int damage = actor.GetComponent<Fighter>().Power - target.GetComponent<Fighter>().Defense;

        string attackDesc = $"{actor.name} attacks {target.name}";

        string colorHex = "";

        if (actor.GetComponent<Player>())
        {
            colorHex = "#ffffff";
        }
        else
        {
            colorHex = "#d1a3a4";
        }

        if (damage > 0)
        {
            UIManager.Instance.AddMessage($"{attackDesc} for {damage} hit points.", colorHex);
            target.GetComponent<Fighter>().Hp -= damage;
        }
        else
        {
            UIManager.Instance.AddMessage($"{attackDesc} but does no damage.", colorHex);
        }
        GameManager.Instance.EndTurn();
    }
    
    public static void MovementAction(Actor actor, Vector2 direction)
    {
        actor.Move(direction);
        actor.UpdateFieldOfView();
        GameManager.Instance.EndTurn();
    }
    
    public static void SkipAction()
    {
        GameManager.Instance.EndTurn();
    }
}
