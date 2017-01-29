using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;

	public float gridSpacing;
	Vector3 worldBottomLeft;

	Node[,] grid;
	
	int gridSizeX, gridSizeY;
	float nodeDiameter;
	float nodeRadius;
	public List<Node> path;
	Vector3 current;
	//Node active;

	//properties
	//public Vector3 Next{ get { return (path.Count > 1) ? path [1].Position : current; } }
	public Vector3 Next {
		get {
			switch (path.Count) {
			case 1:
				return path[0].Position;
				break;
			case 0:
				return GameObject.Find("SeekTarget").transform.position;
				break;
			default:
				return path[1].Position;
				break;
			}
		}
	}
	//public Node Next{ get { return path [1]; } }

	void Start(){
		current = Vector3.zero;
		path = new List<Node> ();
		nodeDiameter = gridSpacing;
		nodeRadius = nodeDiameter / 2;
		gridSizeX = Mathf.RoundToInt (gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt (gridWorldSize.y/nodeDiameter);
		CreateGrid ();
	}
	void Update(){
		print (path.Count);
		//active = NodeFromWorldPoint (GameObject.Find ("Player").transform.position);
	}
	void CreateGrid(){
		grid = new Node[gridSizeX, gridSizeY];
		worldBottomLeft = transform.position - transform.right * gridWorldSize.x/2 - transform.forward * gridWorldSize.y/2;

		for (int x = 0; x < gridSizeX; x++) {
			for(int y = 0; y < gridSizeY; y++){
				Vector3 worldPoint = worldBottomLeft + transform.right*(x*nodeDiameter + nodeRadius) + transform.forward *(y * nodeDiameter + nodeRadius);
				bool walkable = (!Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				grid[x,y] = new Node(walkable,worldPoint,x,y);
			}
		}
	}
	public Node NodeFromWorldPoint(Vector3 agent){
		float percentX = (agent.x - worldBottomLeft.x) / gridWorldSize.x;
		float percentY = (agent.z - worldBottomLeft.z) / gridWorldSize.y;

		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		int x = Mathf.RoundToInt((gridSizeX-1)*percentX);
		int y = Mathf.RoundToInt((gridSizeY-1)*percentY);

		return grid [x, y];
	}
	public List<Node> FindNeighbors(Node node){
		List<Node> neighbors = new List<Node> ();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if(x == 0 && y == 0)
					continue;
				int checkX = node.x + x;
				int checkY = node.y + y;

				if(checkX >=0 && checkX < gridSizeX
				   && checkY >=0 && checkY < gridSizeY){
					neighbors.Add(grid[checkX,checkY]);
				}
			}
		}
		return neighbors;
	}

	void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position, new Vector3 (gridWorldSize.x, 1, gridWorldSize.y));
		if (grid != null) {
			Node active = NodeFromWorldPoint (GameObject.Find ("Player").transform.position);
			foreach(Node n in grid){
				Gizmos.color = (n.Walkable) ? Color.white : Color.red;
				Gizmos.color = (n == active) ? Color.green : Gizmos.color;
				if(n == active){
					Gizmos.color = Color.green;
					current = n.Position;
				}
				if(path != null)
					if(path.Contains(n))
						Gizmos.color = Color.blue;
				Gizmos.DrawCube(n.Position, new Vector3(nodeRadius*1.5f,1,nodeRadius*1.5f));
			}
		}
	}
}

