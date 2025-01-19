using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rigidBody;
    private Vector3 lastMousePos;
    public float sensitivity = 0.16f, clampDelta = 42f, bound = 5f;

    public bool canMove, gameOver, finish;
    public GameObject breakablePlayer;

    GameObject Restart;
    GameObject restartCanvas;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    public static void LoadMain()
    {
        int level = PlayerPrefs.GetInt("Level", 1);
        SceneManager.LoadScene("Level" + level);
    }


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        Restart = GameObject.FindGameObjectWithTag("Restart");
        restartCanvas = Restart.transform.GetChild(0).gameObject;
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }

        if (canMove)
        {
            if (Input.GetMouseButton(0))
            {
                UnityEngine.Vector3 newPosition = lastMousePos - Input.mousePosition;
                lastMousePos = Input.mousePosition;
                newPosition = new UnityEngine.Vector3(newPosition.x, 0, newPosition.y);
                Vector3 moveForce = Vector3.ClampMagnitude(newPosition, clampDelta);
                rigidBody.AddForce((-moveForce * sensitivity - rigidBody.linearVelocity/ 5f), ForceMode.VelocityChange);
            }
        }

        

        rigidBody.linearVelocity.Normalize();
    }

    [System.Obsolete]
    private void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -bound, bound), transform.position.y, transform.position.z);

        if (canMove)
        {
            transform.position += FindFirstObjectByType<CameraMovement>().cameraVelocity;
        }

        if (!canMove && gameOver)
        {
            if (Input.GetMouseButton(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                restartCanvas.SetActive(false);
            }
        }
        
        else if (!canMove && !finish)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FindObjectOfType<GameManager>().RemoveUi();
                canMove = true;
            }
        }
    }

    private void GameOver()
    {
        restartCanvas.SetActive(true);
        GameObject shatterSphere = Instantiate(breakablePlayer, transform.position, Quaternion.identity);

        foreach (Transform objects in shatterSphere.transform)
        {
            objects.GetComponent<Rigidbody>().AddForce(Vector3.forward * 5, ForceMode.Impulse);
        }

        canMove = false;
        gameOver = true;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    IEnumerator NextLevel()
    {
        finish = true;
        canMove = false;
        int currentLevel = PlayerPrefs.GetInt("Level", 1);
        int nextLevel = currentLevel + 1;
        PlayerPrefs.SetInt("Level", nextLevel);
        yield return new WaitForSeconds(1f);

        if (Application.CanStreamedLevelBeLoaded("Level" + nextLevel))
        {
            SceneManager.LoadScene("Level" + nextLevel);
        }
        else
        {
            nextLevel = 1;
            PlayerPrefs.SetInt("Level" , nextLevel);
            SceneManager.LoadScene("Level" + nextLevel);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!gameOver)
            {
                GameOver();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Finish")
        {
            StartCoroutine(NextLevel());
        }
    }

}
