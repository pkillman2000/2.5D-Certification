using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderChecker : MonoBehaviour
{
    [SerializeField]
    private Vector3 _playerIdlePosition;


    public Vector3 GetPlayerIdlePosition()
    { 
        return _playerIdlePosition; 
    }
}
