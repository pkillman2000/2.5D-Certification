using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabChecker : MonoBehaviour
{
    private Player _player;
    private Vector3 _playerSnapPosition; // This is the character position hanging from the ledge
    private Vector3 _playerIdlePosition; // This is the character position after the climb and in Idle animation

    private void Start()
    {
        _player = GetComponentInParent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is Null!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ledge_Checker")
        {
            _playerSnapPosition = other.GetComponent<LedgeChecker>().GetPlayerSnapPosition();
            _playerIdlePosition = other.GetComponent<LedgeChecker>().GetPlayerIdlePosition();
            _player.LedgeGrab(_playerSnapPosition, _playerIdlePosition);
        }

        if(other.tag == "Ladder")
        {
            _player.ClimbLadder();
        }

        if(other.tag == "LadderChecker")
        {
            _playerIdlePosition = other.GetComponent<LadderChecker>().GetPlayerIdlePosition();

            _player.ClimbToTopOfLadder(_playerIdlePosition);
        }
    }
}
