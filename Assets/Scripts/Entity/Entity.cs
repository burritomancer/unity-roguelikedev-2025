using UnityEngine;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    [SerializeField] private bool blocksMovement;

    public bool BlocksMovement { get => blocksMovement; set => blocksMovement = value; }

    public void AddToGameManager()
    {
        GameManager.Instance.Entities.Add(this);
    }
    
    public void Move(Vector2 direction)
    {
        transform.position += (Vector3)direction;
    }
}