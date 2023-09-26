using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gate : MonoBehaviour
{
    public TextMeshProUGUI gateModifier;
    public char actionSymbol;
    public int gateModifierValue;
    
    // Start is called before the first frame update
    private void Start()
    {
        gateModifier.text = actionSymbol + gateModifierValue.ToString();
        
        Renderer renderer = GetComponent<Renderer>();

        if (actionSymbol == '+' ||
            actionSymbol == '*')
        {
            renderer.material.color = Color.green;
        }
        else if(actionSymbol == '-' ||
                actionSymbol == '/')
        {
            renderer.material.color = Color.red;
        }
    }
}
