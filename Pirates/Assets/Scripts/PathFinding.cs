using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour {

	public Transform seeker, target;
	private float counter;
	Grid grid;



	void Awake(){
		counter = 0;
		grid = GetComponent<Grid> ();
	}
	void Update(){
		counter += Time.deltaTime;
		if (counter > 1) {
			FindPath (seeker.position, target.position);
			counter = 0;
		}
	}

	void FindPath(Vector3 start, Vector3 target){
		Node startNode = grid.NodeFromWorldPoint (start);
		Node targetNode = grid.NodeFromWorldPoint (target);

		List<Node> openSet = new List<Node> ();
		HashSet<Node> closedSet = new HashSet<Node> ();

		openSet.Add (startNode);

		while (openSet.Count > 0) {
			Node currentNode = openSet[0];
			for(int i = 0; i < openSet.Count; i++){
				if(openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)){
					currentNode = openSet[i];
				}
			}
			openSet.Remove(currentNode);
			closedSet.Add(currentNode);

			if(currentNode == targetNode){
				RetracePath(startNode,targetNode);
				return;
			}
			foreach(Node neighbor in grid.FindNeighbors(currentNode)){
				if(!neighbor.Walkable || closedSet.Contains(neighbor))
					continue;
				int newCost = currentNode.GCost + GetDistance(currentNode, neighbor);
				if(newCost < neighbor.GCost || !openSet.Contains(neighbor)){
					neighbor.GCost = newCost;
					neighbor.HCost = GetDistance(neighbor,targetNode);
					neighbor.parent = currentNode;

					if(!openSet.Contains(neighbor)){
						openSet.Add(neighbor);
					}
				}
			}
		}
	}
	void RetracePath(Node start, Node end){
		List<Node> path = new List<Node> ();
		Node currentNode = end;
		while (currentNode != start) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse ();

		grid.path = path;
	}

	int GetDistance(Node a, Node b){
		int distX = Mathf.Abs (a.x - b.x);
		int distY = Mathf.Abs (a.y - b.y);
		if (distX > distY)
			return 14 * distY + 10 * (distX - distY);
		return 14 * distX + 10 * (distY - distX);
	}
}
