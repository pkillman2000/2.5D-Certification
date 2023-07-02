using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatingPlatform : MonoBehaviour
{
    [SerializeField]
    private Vector3[] _destinations; // 0 = left, 1 = right
    [SerializeField]
    private float _platformSpeed;
    private int _targetDestination = 1;


    private void Update()
    {
        MoveElevator();
    }

    private void MoveElevator()
    {
        Vector3 currentPosition = this.transform.position;
        if (currentPosition != _destinations[_targetDestination]) // If not at target destination
        {
            transform.position = Vector3.MoveTowards(transform.position, _destinations[_targetDestination], _platformSpeed * Time.deltaTime);
        }
        else // At target destination
        {
            if (_targetDestination == 0)
            {
                _targetDestination = 1;
            }
            else
            {
                _targetDestination = 0;
            }
        }
    }
}
