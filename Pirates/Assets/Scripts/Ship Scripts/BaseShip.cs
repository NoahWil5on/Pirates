using UnityEngine;
using System.Collections;

public class BaseShip : ShipMovement {

	Vector3 ultForce;
	float mag;
	GameObject target;

	public override void CalculateSteering ()
	{
		target = GameObject.Find ("Target");
		ultForce = Vector3.zero;
		/*if (Vector3.SqrMagnitude (transform.position - target.transform.position) < Mathf.Pow (destinationDist, 2)) {
			ultForce = Friction(target, 10);
			return;
		}*/

		float dist = Vector3.Magnitude (transform.position - target.transform.position);
		if(dist > .1f)
			ultForce = Seek (target.transform.position) +
				ObstacleAvoid () * 2;
		if (dist < destinationDist) {
			Vector3 slowVel = velocity.normalized*mag;
			velocity = (slowVel-slowVel*((destinationDist-dist)/destinationDist));
			if(velocity.magnitude < .2f){
				velocity = Vector3.zero;
			}
		} else {

			mag = velocity.magnitude;
		}

		ApplyForce (ultForce);
	}
}
