using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float accuracy = .5f;
    [SerializeField] private float rotSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;

    private Rigidbody rb;
    private Vector3 goal;
    private bool jumped = false;

    private Vector3 scaleVector = new Vector3(1.5f, 1.5f, 1.5f);

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        goal = transform.position;
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
        {
            jumped = false;
            goal = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            //Debug.Log("Current position vector: " + goal.ToString());
        }

        Vector3 direction = goal - transform.position;

        if (Vector3.Distance(transform.position, goal) > accuracy)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * rotSpeed);

            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else
        {
            if(!jumped)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                StartCoroutine(WaitToScale());
                jumped = true;
            }
        }


    }

    IEnumerator WaitToScale()
    {
        yield return new WaitForSeconds(0.5f);
        transform.localScale = Vector3.Scale(transform.localScale, scaleVector);
        yield return new WaitForSeconds(0.5f);
        transform.localScale = Vector3.one;
    }
}
