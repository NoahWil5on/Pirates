using UnityEngine;
using System.Collections;

public class BaseShip : ShipMovement {

	Vector3 ultForce;

	GameObject target;

	public override void CalculateSteering ()
	{
		target = GameObject.Find ("Target");
		ultForce = Vector3.zero;

		ultForce = Seek(target.transform.position) +
			ObstacleAvoid()*2;
		ultForce = Vector3.ClampMagnitude (ultForce, maxSpeed);

		ApplyForce (ultForce);
	}
}
