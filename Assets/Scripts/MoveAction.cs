using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;

    private int maxDistanceMoveAction = 1;
    
    private Vector3 targetPosition;
    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveUnit();
    }
    
    public void Move(GridPosition gridPosition)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    private void MoveUnit()
    {
        float stoppingDistance = 0.1f;
        
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float speed = 3f;
            float rotateSpeed = 20f;
            var moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
            //transform.forward = moveDirection;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            unitAnimator.SetBool("IsWalking", true);
        }
        else
            unitAnimator.SetBool("IsWalking", false);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxDistanceMoveAction; x <= maxDistanceMoveAction; x++)
        {
            for (int z = -maxDistanceMoveAction; z <= maxDistanceMoveAction; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;
                
                if(unitGridPosition == testGridPosition)
                    continue;
                
                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    continue;

                validGridPositions.Add(testGridPosition);
            }
        }

        return validGridPositions;
    }
}
