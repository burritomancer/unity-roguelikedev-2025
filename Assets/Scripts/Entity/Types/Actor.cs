using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : Entity
{
    [SerializeField] private bool isAlive = true;

    [SerializeField] private int fieldOfViewRange = 8;
    [SerializeField] private List<Vector3Int> fieldOfView = new List<Vector3Int>();
    [SerializeField] private AI aI;
    private AdamMilVisibility algorithm;
    
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public List<Vector3Int> FieldOfView => fieldOfView;

    private void OnValidate()
    {
        if (GetComponent<AI>())
        {
            aI = GetComponent<AI>();
        }
    }

    void Start()
    {
        AddToGameManager();
            if (GetComponent<Player>())
            {
                GameManager.Instance.InsertActor(this, 0);
            }
            else
            {
                GameManager.Instance.AddActor(this);
            }
        
        algorithm = new AdamMilVisibility();
        UpdateFieldOfView();
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
