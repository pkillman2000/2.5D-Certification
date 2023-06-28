using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeChecker : MonoBehaviour
{
    [SerializeField]
    private Vector3 _playerSnapPosition; // This is the character position hanging from the ledge
    [SerializeField]
    private Vector3 _playerIdlePosition; // This is the character position after the climb and in Idle animation

    public Vector3 GetPlayerSnapPosition()
    { 
        return _playerSnapPosition; 
    }

    public Vector3 GetPlayerIdlePosition()
    {
        return _playerIdlePosition;
    }
}
