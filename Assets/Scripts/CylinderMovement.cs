using UnityEngine;

public class CylinderMovement : MonoBehaviour
{
    public float speed, distance;
    private float minX, maxX;
    public bool right, dontMove, stop;


    void Start()
    {
        maxX = transform.position.x + distance;
        minX = transform.position.x - distance;
    }

    void Update()
    {
        if (!stop && !dontMove)
        {
            if (right)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;

                if (transform.position.x >= maxX)
                {
                    right = false;
                }
            }

            else
            {
                transform.position += Vector3.left * speed * Time.deltaTime;

                if (transform.position.x <= minX)
                {
                    right = true;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "White" && collision.gameObject.GetComponent<Rigidbody>().linearVelocity.magnitude > 1 || collision.gameObject.tag == "Player")
        {
            stop = true;
            GetComponent<Rigidbody>().freezeRotation = false;
        }
    }
}
