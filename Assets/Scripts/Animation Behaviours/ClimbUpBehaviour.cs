using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClimbUpBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player _player;

        _player = FindObjectOfType<Player>().GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is Null!");
        }

        _player.ClimbToIdle();        
    }
}
