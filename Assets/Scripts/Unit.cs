using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
    
    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
