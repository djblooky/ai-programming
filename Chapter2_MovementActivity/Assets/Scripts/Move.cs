using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float accuracy = .5f;
    [SerializeField] private float rotSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;

    private MeshRenderer meshRenderer;
    private Rigidbody rb;
    private Vector3 goal;
    private bool jumped = false;

    private Vector3 scaleVector = new Vector3(1.5f, 1.5f, 1.5f); //scale vector to increase scale by 50%

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        goal = transform.position;
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
        {
            jumped = false; //when setting new goal, set jump back to false
            goal = new Vector3(hit.point.x, transform.position.y, hit.point.z);
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
            if(!jumped) //if not jumped, jump, rotate, and start coroutune
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                StartCoroutine(ScaleAndColor());
                jumped = true;
            }
        }
    }

    IEnumerator ScaleAndColor() //change cube color and scale
    {
        yield return new WaitForSeconds(0.5f);
        transform.localScale = Vector3.Scale(transform.localScale, scaleVector);
        meshRenderer.material.color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.5f);
        transform.localScale = Vector3.one;
        meshRenderer.material.color = new Color(0,0,0);

    }
}
