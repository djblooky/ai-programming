using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float accuracy = .5f;
    [SerializeField] private float rotSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;

    private Rigidbody rigidbody;
    private Vector3 goal;
    private bool jumped = false;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
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
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumped = true;
            }
        }
    }
}
