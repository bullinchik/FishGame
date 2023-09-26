using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UserResources;

public class FinishOrDieCollider : MonoBehaviour
{
    static GameObject DieScreenSctatic;
    static GameObject FinishScreenStatic;
    static Image EvolutionImageStatic;
    static GameObject NextLevelButtonStatic;

    public GameObject evolution;
    [SerializeField] private GameObject DieScreen;
    [SerializeField] private GameObject FinishScreen;
    [SerializeField] private TextMeshProUGUI DisplayMoney;
    [SerializeField] private Image EvolutionImage;
    [SerializeField] private GameObject NextLevelButton;
    private static TextMeshProUGUI _finishText;
    private static TextMeshProUGUI evolutionProgressText;

    private int startEvoStage;
    
    private Image evolutionFish;
    
    public UserResources ur;
    public static bool evolutioning;

    //Delete this after fix animations on fish
    private void Awake()
    {
        if (evoStage == 1)
            transform.eulerAngles = new Vector3(0f, 90f, 0f );
        else if (evoStage == 0)
            transform.eulerAngles = new Vector3(-90, 0, -90);
    }

    void Start()
    {
        evolutionFish = evolution.GetComponentsInChildren<Image>()[2];
        evolutioning = false;
        DieScreenSctatic = DieScreen;
        FinishScreenStatic = FinishScreen;
        NextLevelButtonStatic = NextLevelButton;
        EvolutionImageStatic = EvolutionImage;

        _finishText = NextLevelButton.GetComponentInChildren<TextMeshProUGUI>();
        evolutionProgressText = FinishScreen.GetComponentsInChildren<TextMeshProUGUI>()[3];
        evolution.SetActive(false);
        DieScreenSctatic.SetActive(false);
        FinishScreenStatic.SetActive(false);
        NextLevelButtonStatic.SetActive(false);

        EvolutionImageStatic.fillAmount = evoProgress / 100f;

        DisplayMoney.text = UserResources.money.ToString();
        startEvoStage = UserResources.evoStage;
    }

    public void NextLevel()
    {
        if (evolutioning == true)
        {
            Evolve();
            evolutioning = false;
            return;
        }

        //for testbuild
        if (evoStage == 1 && startEvoStage == 1)
        {
            UserResources.AddEvoStage(-1);
            SceneManager.LoadSceneAsync("MainScene");
        }
        else
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void Die()
    {
        DieScreenSctatic.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public static void FinishCollider(MonoBehaviour instance)
    {
        int playerStrength = (int)GameObject.FindGameObjectWithTag("Player").GetComponent<EatingFood>().CurrentStrength;
        int evolutionAdd;

        //for test build
        if (UserResources.evoStage == 0)
        {
            evolutionAdd = 100;
        }
        else 
        {
            evolutionAdd = 0;
        }
        
        // if (playerStrength < 200)
        // {
        //     evolutionAdd = 20;
        // }
        // else if (200 <= playerStrength && playerStrength < 300)
        // {
        //     evolutionAdd = 30;
        // }
        // else // if playerstrength > 10
        // {
        //     evolutionAdd = 50;
        // }

        FinishScreenStatic.SetActive(true);
        NextLevelButtonStatic.SetActive(true);
        AddEvoProgress(evolutionAdd);

        instance.StartCoroutine(FillImage(evolutionAdd));
        
        AddMoney(20);
    }

    private void Evolve()
    {
        ur.UpdateSprite();
        evolutionFish.sprite = UserResources.sprite;
        evolution.SetActive(true);
    }

    static IEnumerator FillImage(int evolutionAdd)
    {
        float percent = evolutionAdd / 100f;
        float fillPoint = EvolutionImageStatic.fillAmount + percent;

        if (fillPoint >= 0.99f)
        {
            FinishOrDieCollider.evolutioning = true;
            _finishText.text = "EVOLVE";

        }
        else
        {
            _finishText.text = "NEXT LEVEL";
        }

        while (EvolutionImageStatic.fillAmount < fillPoint)
        {
            EvolutionImageStatic.fillAmount += 0.005f;

            evolutionProgressText.text = ((int)(EvolutionImageStatic.fillAmount * 100f)).ToString() + "%";
            yield return null;
        }

        NextLevelButtonStatic.SetActive(true);
    }
}
