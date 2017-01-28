using UnityEngine;
using System.Collections;

public class ShipSeeker : ShipMovement {

	Vector3 ultForce;
	public GameObject display;
	Grid grid;

	public override void Start(){

		grid = GameObject.Find ("NodeManager").GetComponent<Grid> ();
		base.Start();
	}

	public override void CalculateSteering(){
		ultForce = Vector3.zero;
		ultForce += Seek (grid.Next);
		display.transform.position = grid.Next;
		ApplyForce (ultForce);
	}
}
