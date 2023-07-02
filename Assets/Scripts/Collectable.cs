using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    private int _collectableValue;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        if(_gameManager == null )
        {
            Debug.LogError("Game Manager is Null!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _gameManager.ReceiveCollectable(_collectableValue);
            Destroy(this.gameObject);
        }        
    }
}
