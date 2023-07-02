using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text _collectableText;
    
    public void UpdateCollectableValue(int value)
    {
        _collectableText.text = "Collectables: " + value.ToString();
    }
}
