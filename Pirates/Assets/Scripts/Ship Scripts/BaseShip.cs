using UnityEngine;
using System.Collections;

public class BaseShip : ShipMovement {

	Vector3 ultForce;

	GameObject target;

	public override void CalculateSteering ()
	{
		target = GameObject.Find ("Target");
		ultForce = Vector3.zero;
		if (Vector3.SqrMagnitude (transform.position - target.transform.position) < Mathf.Pow (destinationDist, 2)) {
			ultForce = Friction(target, 10);
			return;
		}
		ultForce = Seek (target.transform.position) +
			ObstacleAvoid () * 2;
		ultForce = Vector3.ClampMagnitude (ultForce, maxSpeed);

		ApplyForce (ultForce);
	}
}
