using UnityEngine;

public class WalkWP : MonoBehaviour
{
    public Transform[] path;

    private int currentNode;
    private Vector3 goal;

    private float speed = 4f;
    private float accuracy = 0.5f;
    private float rotSpeed = 4f;

    private void Start()
    {

    }

    private void Update()
    {
        goal = new Vector3(path[currentNode].position.x, 
            path[currentNode].position.y, 
            path[currentNode].position.z);

        Vector3 direction = goal - transform.position;

        if(direction.magnitude > accuracy)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(direction), 
                Time.deltaTime * rotSpeed);

            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else
        {
            if(currentNode < path.Length - 1)
                currentNode++;
            else
            {
                currentNode = Random.Range(1, path.Length - 1); //before it completes circle will go to a random node
                //currentNode = 0 instead if you want it to  keep circling only
            }

        }

    }
}
