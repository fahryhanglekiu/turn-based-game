using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }
    
    [SerializeField] private Transform gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(), 
            LevelGrid.Instance.GetHeight()
        ];
        
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                
                GridSystemVisualSingle gridSystemVisualSingle = gridSystemVisualTransform.GetComponent<GridSystemVisualSingle>();

                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingle;
            }
        }
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        for (int i = 0; i < gridPositionList.Count; i++)
        {
            int x = gridPositionList[i].x;
            int z = gridPositionList[i].z;

            GridSystemVisualSingle gridSystemVisualSingle = gridSystemVisualSingleArray[x, z];
            
            gridSystemVisualSingle.Show();
        }
    }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < gridSystemVisualSingleArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridSystemVisualSingleArray.GetLength(1); z++)
            {
                GridSystemVisualSingle gridSystemVisualSingle = gridSystemVisualSingleArray[x, z];
                gridSystemVisualSingle.Hide();
            }
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPosition();
        
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
        ShowGridPositionList(unit.GetMoveAction().GetValidActionGridPositionList());
    }
}
