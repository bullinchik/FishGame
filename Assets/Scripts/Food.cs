using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public class Food : MonoBehaviour
{

    public int Value;
    [FormerlySerializedAs("displayText")] public TextMeshProUGUI displayValue;
    
    private float _playerStrength;
    private void Start()
    {
        float scaleStart = 200f;
        float scaler = 1f /  (100 - Value) * scaleStart;
        displayValue.text = Value.ToString();

        transform.localScale = new Vector3(scaler, scaler, scaler);
    }

}
