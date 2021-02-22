using System.Collections.Generic;
using UnityEngine;

public class GridWP : MonoBehaviour
{
    public Node[,] grid;
    private List<Node> path = new List<Node>();
    private int currentNode;

    public GameObject prefabWaypoint;
    public Material goalMaterial, wallMaterial;
    private Vector3 goal;

    private float speed = 4f;
    private float accuracy = 0.5f;
    private float rotationSpeed = 4f;
    private int spacing = 5;

    private Node startNode, endNode;

    private void Start()
    {
        //create grid

        grid = new Node[,]
        {
            {
            new Node(),
            new Node(),
            new Node(false),
            new Node(),
            new Node(),
            new Node(),
            },

            { 
            new Node(false),
            new Node(),
            new Node(),
            new Node(),
            new Node(),
            new Node(false),
            },

            { 
            new Node(),
            new Node(),
            new Node(),
            new Node(false),
            new Node(),
            new Node(),
            },

            {
            new Node(),
            new Node(false),
            new Node(),
            new Node(),
            new Node(),
            new Node(),
            },

            {
            new Node(),
            new Node(),
            new Node(),
            new Node(false),
            new Node(),
            new Node(),
            },

            {
            new Node(false),
            new Node(),
            new Node(),
            new Node(),
            new Node(),
            new Node(),
            },

            {
            new Node(),
            new Node(),
            new Node(),
            new Node(),
            new Node(),
            new Node(false),
            },

        };

        //initialize grid points

        for(int i = 0; i < grid.GetLength(0); i++)
        {
            for(int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j].Waypoint = Instantiate(prefabWaypoint, 
                    new Vector3(i * spacing, 
                        transform.position.y, 
                        j * spacing), 
                        Quaternion.identity);

                if (!grid[i, j].Walkable)
                {
                    grid[i, j].Waypoint.GetComponent<Renderer>().material = wallMaterial;
                }
                else
                {
                    grid[i, j].Neighbors = getAdjacentNode(grid, i, j);
                }
            }
        }

        startNode = grid[0, 0];
        endNode = grid[6, 5];
        startNode.Walkable = true;
        endNode.Waypoint.GetComponent<Renderer>().material = goalMaterial;

        transform.position = new Vector3(startNode.Waypoint.transform.position.x,
            transform.position.y,
            startNode.Waypoint.transform.position.z);
    }

    List<Node> getAdjacentNode(Node[,] m, int i, int j)
    {
        List<Node> l = new List<Node>();

        //node above this one
        if(i - 1 >= 0)
        {
            if (m[i - 1, j].Walkable) //if node is walkable add to our temp list
            {
                l.Add(m[i - 1, j]);
            }
        }

        //node below

        if(i+1 < m.GetLength(0))
        {
            if (m[i + 1, j].Walkable) //if node is walkable add to our temp list
            {
                l.Add(m[i + 1, j]);
            }
        }

        //node left

        if(j-1 >= 0)
        {
            if (m[i, j - 1].Walkable)
            {
                l.Add(m[i, j - 1]);
            }
        }

        //node right

        if (j + 1 < m.GetLength(1))
        {
            if (m[i, j + 1].Walkable)
            {
                l.Add(m[i, j + 1]);
            }
        }

        return l;
    }

    public class Node
    {
        private int depth;
        private bool walkable;

        private GameObject waypoint = new GameObject();
        private List<Node> neighbors = new List<Node>();

        public int Depth { get => depth; set => depth = value; }
        public bool Walkable { get => walkable; set => walkable = value; }
        public GameObject Waypoint { get => waypoint; set => waypoint = value; }
        public List<Node> Neighbors { get => neighbors; set => neighbors = value; }

        public Node()
        {
            depth = -1;
            walkable = true;
        }

        public Node(bool walkable)
        {
            depth = -1;
            this.walkable = walkable;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Node n = obj as Node;

            if (n == null) return false;

            if (this.waypoint.transform.position.x == waypoint.transform.position.x
                && this.waypoint.transform.position.z == waypoint.transform.position.z)
                return true;

            return false;
        }

    }
}
