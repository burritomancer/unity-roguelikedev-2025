using UnityEngine;

[RequireComponent(typeof(Actor))]
sealed class Fighter : MonoBehaviour
{
    [SerializeField] private int maxHp, hp, defense, power;
    [SerializeField] private Actor target;

    public int Hp
    {
        get => hp;
        set
        {
            hp = Mathf.Max(0, Mathf.Min(value, maxHp));

            if (GetComponent<Player>())
            {
                UIManager.Instance.SetHealth(hp, maxHp);
            }
            
            if (hp == 0)
                Die();
        }
    }
    
    public int Defense => defense;
    public int Power => power;
    public Actor Target {  get => target; set => target = value; }

    private void Start()
    {
        if (GetComponent<Player>())
        {
            UIManager.Instance.SetHealthMax(maxHp);
            UIManager.Instance.SetHealth(hp, maxHp);
        }
    }

    private void Die()
    {
        if (GetComponent<Player>())
        {
            UIManager.Instance.AddMessage($"You died!", "#ff0000");
        }
        else
        {
            UIManager.Instance.AddMessage($"{name} is dead!", "#ffa500");
        }
        
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GameManager.Instance.DeadSprite;
        spriteRenderer.color = new Color(191, 0, 0, 1);
        spriteRenderer.sortingOrder = 0;

        name = $"Remains of {name}";
        GetComponent<Actor>().BlocksMovement = false;
        GetComponent<Actor>().IsAlive = false;
        if (!GetComponent<Player>())
        {
            GameManager.Instance.RemoveActor(this.GetComponent<Actor>());
        }
    }
}
