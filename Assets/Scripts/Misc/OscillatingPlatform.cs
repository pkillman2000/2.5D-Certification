using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatingPlatform : MonoBehaviour
{
    [SerializeField]
    private Vector3[] _floors; // 0 = top, 1 = bottom
    [SerializeField]
    private float _elevatorSpeed;
    [SerializeField]
    private float _pauseAtFloor;
    private int _targetFloor = 1;
    private bool _elevatorMoving = true;

   
    private void FixedUpdate()
    {
        if(_elevatorMoving) 
        {
            MoveElevator();
        }
    }

    private void MoveElevator()
    {
        Vector3 currentPosition = this.transform.position;
        if (currentPosition != _floors[_targetFloor]) // If not at target floor
        {
            transform.position = Vector3.MoveTowards(currentPosition, _floors[_targetFloor], _elevatorSpeed * Time.deltaTime);
        }
        else // At target floor
        {
            _elevatorMoving = false;
            StartCoroutine(PauseAtFloorRoutine());
        }        
    }

    private IEnumerator PauseAtFloorRoutine()
    {
        yield return new WaitForSeconds(_pauseAtFloor);

        if(_targetFloor == 0)
        {
            _targetFloor = 1;
        }
        else
        {
            _targetFloor = 0;
        }
        _elevatorMoving = true;
    }

    // Makes Player child of the platform so it moves with the platform
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
