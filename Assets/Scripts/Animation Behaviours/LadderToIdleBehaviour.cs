using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderToIdleBehaviour : StateMachineBehaviour
{

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player _player;

        _player = FindObjectOfType<Player>().GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is Null!");
        }

        _player.ClimbLadderToIdle();

    }

}
