using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class EatingFood : MonoBehaviour
{
    public TextMeshProUGUI strengthViewText;
    public int CurrentColorScheme;
    
    private float _baseStrength;
    private float _foodSumOnLevel;
    private float _eatedFood = 0;
    public float CurrentStrength { get; private set; }

    private float _evoMulti;
    private GameObject[] _sceneFood;
    
    public static bool finishing;
    public GameObject progressBar;

    public void Start()
    {
        
        finishing = false;
        _baseStrength = UserResources.basicAttack;
        _evoMulti = UserResources.evoMultiplicateur;
        CurrentStrength = UserResources.basicAttack * _evoMulti;
        strengthViewText.text = Math.Truncate(CurrentStrength).ToString(); 
        _sceneFood = GameObject.FindGameObjectsWithTag("Food");

        _foodSumOnLevel = GetSumFoodValueOnScene(_sceneFood);
        RecolorFood(_sceneFood);
    }

    private float GetSumFoodValueOnScene(GameObject[] foodOnLevel)
    {
        float summ = 0;
        for (int i = 0; i < foodOnLevel.Length; i++)
        {
            summ += foodOnLevel[i].GetComponentInChildren<Food>().Value;
        }

        GameObject[] gates = GameObject.FindGameObjectsWithTag("Gate");
        for (int i = 0; i < gates.Length; i++)
        {
            if (gates[i].GetComponentInChildren<Gate>().actionSymbol == '+')
            {
                summ += gates[i].GetComponentInChildren<Gate>().gateModifierValue;
            }
        }

        return summ;
    }

    private void RecolorFood(GameObject[] food)
    {
        Color redColor, greenColor; 
        
        ColorUtility.TryParseHtmlString("#F67C7C", out redColor);
        ColorUtility.TryParseHtmlString("#86FFAA", out greenColor);
        
        Dictionary<int, Color[]> colorDictionary = new Dictionary<int, Color[]>()
        {
            {1, new [] {Color.white, redColor} },
            {2, new [] {greenColor, redColor} }
        };

        for (int i = 0; i < food.Length; i++) {
            int foodValue = food[i].GetComponentInChildren<Food>().Value;
            TextMeshProUGUI foodText = food[i].GetComponentInChildren<Food>().displayValue;
            if (foodValue < CurrentStrength) {
                foodText.color = colorDictionary[CurrentColorScheme][0];
            }
            else {
                foodText.color = colorDictionary[CurrentColorScheme][1];
            }
        }
    }

    public void OnTriggerEnter(Collider objectCollider)
    {
        if (objectCollider.gameObject.CompareTag("Food")) { 
            FoodCollider(objectCollider);
        }
        else if (objectCollider.gameObject.CompareTag("Gate")) {
            GateCollider(objectCollider);
        }
        else if (objectCollider.gameObject.CompareTag("Hook")) {
            FinishOrDieCollider.Die();
        }
        else if (objectCollider.gameObject.CompareTag("Finish")) {
            StartCoroutine(Finishing(objectCollider, this));
        }
    }

    private IEnumerator Finishing(Collider objectCollider, MonoBehaviour obj)
    {
        
        finishing = true;
        progressBar.SetActive(false);
        float xMod;
        if (CurrentStrength < 200)
        {
            xMod = 10;
            objectCollider.GetComponentsInChildren<Animator>()[0].Play("HideWeed");//SetTrigger("Eat");
            objectCollider.GetComponentsInChildren<Animator>()[1].SetTrigger("Open");
        }
        else if (200 <= CurrentStrength && CurrentStrength < 300)
        {
            xMod = 30;

            Animator animatorWeed = objectCollider.GetComponentsInChildren<Animator>()[0];
            animatorWeed.Play("HideWeed");//SetTrigger("Eat");

            if (animatorWeed.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animatorWeed.IsInTransition(0))
            {
                objectCollider.GetComponentsInChildren<Animator>()[2].Play("HideWeed");//SetTrigger("Eat");
                objectCollider.GetComponentsInChildren<Animator>()[3].SetTrigger("Open");
            }
        }
        else // if playerstrength > 30
        {
            xMod = 50;
            Animator animatorWeed = objectCollider.GetComponentsInChildren<Animator>()[0];
            animatorWeed.Play("HideWeed"); //SetTrigger("Eat");

            if (animatorWeed.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animatorWeed.IsInTransition(0))
            {
                objectCollider.GetComponentsInChildren<Animator>()[2].Play("HideWeed"); //SetTrigger("Eat");
                objectCollider.GetComponentsInChildren<Animator>()[4].Play("HideWeed"); //SetTrigger("Eat");
                objectCollider.GetComponentsInChildren<Animator>()[5].SetTrigger("Open");
            }
        }

        float zPos = transform.position.z + xMod;

        while (transform.position.z < zPos)
        {
            yield return null;
        }

        finishing = false;
        FinishOrDieCollider.FinishCollider(obj);
    }

    private void FoodCollider(Collider foodCollider)
    {
        int foodValue = foodCollider.gameObject.GetComponentInChildren<Food>().Value;
        if (foodValue < CurrentStrength) {
            ModifierStrengthAndSize(foodValue);
            
            Destroy(foodCollider.gameObject);

            _sceneFood = GameObject.FindGameObjectsWithTag("Food");
            
            RecolorFood(_sceneFood);
        }
        else {
            FinishOrDieCollider.Die();
        }
    }

    private void GateCollider(Collider gateCollider)
    {
        int gateModifier = gateCollider.gameObject.GetComponentInChildren<Gate>().gateModifierValue;
        char actionSymbol = gateCollider.gameObject.GetComponentInChildren<Gate>().actionSymbol;

        ModifierStrengthAndSize(gateModifier, actionSymbol);
    }

    private void ModifierStrengthAndSize(int value, char modifier = '+')
    {
        switch (modifier) {
            case '+':
                _baseStrength += value;
                _eatedFood += value;
                break;
            case '-':
                _baseStrength -= value;
                _eatedFood -= value;
                
                if (_baseStrength <= 1) {
                    FinishOrDieCollider.Die();
                }
                break;
            case '/':
                _baseStrength /= value;
                _eatedFood -= value;
                break;
            case '*':
                _eatedFood *= value;
                _baseStrength *= value;
                break;
        }
        
        CurrentStrength = _baseStrength * _evoMulti;
        strengthViewText.text = Math.Truncate(CurrentStrength).ToString();

        float maxSize = UserResources.evoStage == 1 ? 0.5f : 1.5f;
        
        float currentSize = _eatedFood / _foodSumOnLevel;
        if (currentSize > 1)
        {
            currentSize = 1;
        }
        float sizeMultiplicator = currentSize * maxSize + 1;
        
        Vector3 resizeVector = new Vector3(sizeMultiplicator, sizeMultiplicator, sizeMultiplicator);
        gameObject.transform.localScale = resizeVector;
        
        _sceneFood = GameObject.FindGameObjectsWithTag("Food");
        RecolorFood(_sceneFood);
    }
}
