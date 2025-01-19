using UnityEngine;
using UnityEngine.UI;

public class BgImage : MonoBehaviour
{
    private Color randomColor;

    void Start()
    {
        randomColor = new Color (Random.Range(0.1f, 1), Random.Range(0.1f, 1), Random.Range(0.1f, 1));
        GetComponent<SpriteRenderer>().color = randomColor;
    }
}
