using UnityEngine;
using System.Collections;

public class Node{
	private bool walkable;
	private Vector3 position;
	private bool visited;

	//properties
	public bool Walkable{ get { return walkable; } }
	public Vector3 Position{ get { return position; } }
	public bool Visited{ get { return visited; } set{ visited = value; }}

	//Constructor
	public Node(bool walkable, Vector3 position){
		visited = false;
		this.walkable = walkable;
		this.position = position;
	}
}
