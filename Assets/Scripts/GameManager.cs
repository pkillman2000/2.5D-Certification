using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _collectables = 0;
    private UIManager _uiManager;

    private void Start()
    {
        _uiManager = FindObjectOfType<Canvas>().GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is Null!");
        }
    }
        
    public void ReceiveCollectable(int value)
    {
        _collectables += value;
        _uiManager.UpdateCollectableValue(_collectables);
    }
}
