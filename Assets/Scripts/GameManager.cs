using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    private TMP_Text currentLevelText, nextLevelText;
    private Image fill;
    private int level;
    private float startDistance, distance;
    private GameObject player, finish, hand;
    private TextMeshPro levelNo;

    private void Awake()
    {
        currentLevelText = GameObject.Find("CurrentLevelText").GetComponent<TextMeshProUGUI>();
        nextLevelText = GameObject.Find("NextLevelText").GetComponent<TextMeshProUGUI>();
        fill = GameObject.Find("Fill").GetComponent<Image>();
        player = GameObject.Find("Player");
        finish = GameObject.Find("Finish");
        hand = GameObject.Find("Hand");
        levelNo = GameObject.Find("LevelNo").GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        level = PlayerPrefs.GetInt("Level", 1);
        levelNo.text = "Level " + level;
        nextLevelText.text = level + 1 + "";
        currentLevelText.text = level.ToString();

        startDistance = Vector3.Distance(player.transform.position, finish.transform.position);
    }

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, finish.transform.position);

        if (player.transform.position.z < finish.transform.position.z)
        {
            fill.fillAmount = 1 - (distance/startDistance);
        }
    }

    public void RemoveUi()
    {
        hand.SetActive(false);
    }
}
