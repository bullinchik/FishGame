using System.Collections.Generic;
using UnityEngine;
using Tabtale.TTPlugins;

public class UserResources : MonoBehaviour
{
    [System.Serializable]
    public struct StageMaterials
    {
        [SerializeField] public Material[] materials;
    }

    public StageMaterials[] GlobalMaterials;

    [SerializeField] private Mesh[] meshesOnStages;
    [SerializeField] private GameObject[] modelsOnStages;
    [SerializeField] private Sprite[] stagesImages;

    public static int level { get; private set; }
    public static int money { get; private set; }
    public static int basicAttack { get; private set; }
    public static float evoMultiplicateur { get; private set; }
    public static int evoStage { get; private set; }
    public static int evoProgress { get; private set; }


    public static Mesh mesh { get; private set; }
    public static GameObject model { get; private set; }
    public static Sprite sprite { get; private set; }
    public static Material[] stageMaterials{ get; private set; }
    


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    void Awake()
    {
        TTPCore.Setup();
        if (PlayerPrefs.GetInt("basicAttack") < 2)
        {
            PlayerPrefs.SetInt("basicAttack", 2);
        }
        
        if (PlayerPrefs.GetInt("evoMulti") < 1)
        {
            PlayerPrefs.SetInt("evoMulti", 1);
        }

        level = PlayerPrefs.GetInt("Level");
        money = PlayerPrefs.GetInt("Money");
        basicAttack = PlayerPrefs.GetInt("basicAttack");
        evoMultiplicateur = PlayerPrefs.GetInt("evoMulti");
        evoStage = PlayerPrefs.GetInt("evoStage");
        evoProgress = PlayerPrefs.GetInt("evoProgress");


        if (evoStage >= modelsOnStages.Length)
        {
            evoStage = 0;
        }
        model = modelsOnStages[evoStage];

        /*if (evoStage >= meshesOnStages.Length)
        {
            evoStage = 0;
        }
        mesh = meshesOnStages[evoStage];
        stageMaterials = GlobalMaterials[evoStage].materials;*/
    }

    public static void AddLevel(int addLevel)
    {
        level += level;
        PlayerPrefs.SetInt("Level", level);
    }
    public static void AddMoney(int addMoney)
    {
        money += addMoney;
        PlayerPrefs.SetInt("Money", money);
    }
    public static void UpdateBasicAttack(int addAttack)
    {
        basicAttack += addAttack;
        PlayerPrefs.SetInt("basicAttack", basicAttack);
    }
    public static void addEvoMultiply(int addMulti)
    {
        evoMultiplicateur += addMulti;
        PlayerPrefs.SetFloat("evoMulti", evoMultiplicateur);
    }
    public static void AddEvoStage(int addStage)
    {
        evoStage += addStage;
        PlayerPrefs.SetInt("evoStage", evoStage);
    }
    public static void AddEvoProgress(int progress)
    {
        evoProgress += progress;

        if (evoProgress < 100)
        {
            PlayerPrefs.SetInt("evoProgress", evoProgress);
            return;
        }

        evoProgress %= 100;
        PlayerPrefs.SetInt("evoProgress", evoProgress);
        AddEvoStage(1);
    }

    public void UpdateMesh()
    {

        if (evoStage >= modelsOnStages.Length)
        {
            evoStage = 0;
        }
        model = modelsOnStages[evoStage];


        /* if (evoStage >= meshesOnStages.Length)
         {
             evoStage = 0;
         }
         mesh = meshesOnStages[evoStage];
         stageMaterials = GlobalMaterials[evoStage].materials;*/


    }

    public void UpdateSprite()
    {
        if (evoStage >= stagesImages.Length)
        {
            evoStage = 0;
        }
        sprite = stagesImages[evoStage];
    }
}
