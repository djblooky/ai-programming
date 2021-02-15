using UnityEngine;

public class Move : MonoBehaviour
{
    Vector3 goal;
    float speed = 1.0f;
    float accuracy = 1.0f;

    private void Start()
    {
        goal = this.transform.position;
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
        {
            goal = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(goal);

            if(Vector3.Distance(transform.position, goal) > accuracy)
            {
                transform.Translate(0, 0, speed * Time.deltaTime);
            }

            Debug.Log("Current position vector: " + goal.ToString());
        }
    }
}
