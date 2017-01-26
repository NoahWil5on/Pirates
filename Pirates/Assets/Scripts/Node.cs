using UnityEngine;
using System.Collections;

public class Node{
	private bool walkable;
	private Vector3 position;
	private bool visited;

	public int x;
	public int y;

	private int hCost;
	private int gCost;

	public Node parent;

	//properties
	public bool Walkable{ get { return walkable; } }
	public Vector3 Position{ get { return position; } }
	public bool Visited{ get { return visited; } set{ visited = value; }}

	//Cost properties
	public int FCost{ get { return hCost + gCost; } }
	public int GCost{ get { return gCost; } set { gCost = value; } }
	public int HCost{ get { return hCost; } set { hCost = value; } }

	//Constructor
	public Node(bool walkable, Vector3 position, int x, int y){
		visited = false;

		this.walkable = walkable;
		this.position = position;
		this.x = x;
		this.y = y;

		gCost = 0;
		hCost = 0;
	}

}
