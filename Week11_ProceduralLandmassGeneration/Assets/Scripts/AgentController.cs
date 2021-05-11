using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public GameObject child;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                this.GetComponent<NavMeshAgent>().SetDestination(hit.point);
            }

            
        }

        if (!(Vector3.Distance(transform.position, child.transform.position) < 1))
        {
            child.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        }
        
    }
}
