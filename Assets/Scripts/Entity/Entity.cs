using UnityEngine;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    [SerializeField] private bool isSentient, blocksMovement;

    [SerializeField] private int fieldOfViewRange = 8;
    [SerializeField] private List<Vector3Int> fieldOfView;
    private AdamMilVisibility algorithm;
    
    public bool IsSentient => isSentient;
    public bool BlocksMovement => blocksMovement;

    void Start()
    {
        if (isSentient)
        {
            if (GetComponent<Player>())
            {
                GameManager.Instance.InsertEntity(this, 0);
            }
            else
            {
                GameManager.Instance.AddEntity(this);
            }
        }
        
        fieldOfView = new List<Vector3Int>();
        algorithm = new AdamMilVisibility();
        UpdateFieldOfView();
    }
    
    public void Move(Vector2 direction)
    {
        transform.position += (Vector3)direction;
    }

    public void UpdateFieldOfView()
    {
        Vector3Int gridPosition = MapManager.Instance.FloorMap.WorldToCell(transform.position);
        
        fieldOfView.Clear();
        algorithm.Compute(gridPosition, fieldOfViewRange, fieldOfView);

        if (GetComponent<Player>())
        {
            MapManager.Instance.UpdateFogMap(fieldOfView);
            MapManager.Instance.SetEntitiesVisibilities();
        }
    }
}