using UnityEngine;

public class Move : MonoBehaviour
{
    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
        {
            Vector3 newPosition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
            this.transform.position = newPosition;

            Debug.Log("Current position vector: " + newPosition.ToString());
        }
    }
}
