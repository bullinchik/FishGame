using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI levelNum;
    [SerializeField] private TextMeshProUGUI finishLevelNum;
    [SerializeField] private Image uiFillImage;
    [SerializeField] private Image fishImage;

    [Header("Player & Endline")]
    [SerializeField] private Transform playerTransform;

    private Vector3 _endLinePos;
    private float fullDistance;
    //private Vector3 fishPos;
    public float GetDistance()
    {
        return Vector3.Distance(playerTransform.position, _endLinePos);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject finish = GameObject.FindWithTag("Finish");
        _endLinePos = finish ? finish.transform.position : playerTransform.position + new Vector3(0f, 0f, playerTransform.position.z + 100f);
        fullDistance = GetDistance();
        
        //fix after test build
        levelNum.text = "LEVEL "; // + (SceneManager.GetActiveScene().buildIndex + 1).ToString();
        finishLevelNum.text = "LEVEL ";// + (SceneManager.GetActiveScene().buildIndex + 1).ToString();
        //fishPos = fishImage.transform.position;
    }

    private void updateProgressFill(float value)
    {
        uiFillImage.fillAmount = value;
        float width = uiFillImage.sprite.rect.width;

        fishImage.transform.localPosition = new Vector3(width * value - width / 2, 0f, 0f);
    }

    private void Update()
    {
        float newDistance = GetDistance();
        float progressiveValue = Mathf.InverseLerp(fullDistance, 0f, newDistance);

        updateProgressFill(progressiveValue);
    }
}
