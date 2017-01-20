using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ShipMovement : MonoBehaviour {

	Vector3 position;
	Vector3 velocity;
	Vector3 acceleration;

	GameObject[] obstacles;
	List<GameObject> dangerO;

	public float mass;
	public float distanceDetect;
	public float radius;
	public float maxSpeed;

	// Use this for initialization
	void Start () {
		dangerO = new List<GameObject> ();
		velocity = Vector3.zero;
		acceleration = Vector3.zero;
		position = transform.position;
	}
	public virtual void CalculateSteering(){

	}
	// Update is called once per frame
	void Update () {
		CalculateSteering ();

		velocity += acceleration*Time.deltaTime;
		position += velocity*Time.deltaTime;
		UpdateTransformation ();

		acceleration = Vector3.zero;
	}
	public void ApplyForce(Vector3 force){
		acceleration += force / mass;
	}
	void UpdateTransformation(){
		transform.position = position;
		transform.forward = velocity.normalized;
	}
	public Vector3 Seek(Vector3 target){
		return ((target-position)-velocity).normalized * maxSpeed;
	}
	public Vector3 ObstacleAvoid(){
		obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		dangerO.Clear ();

		for (int i = 0; i< obstacles.Length; i++) {
			if(Vector3.SqrMagnitude(obstacles[i].transform.position-position) < distanceDetect*distanceDetect)
				dangerO.Add(obstacles[i]);
		}
		if(dangerO.Count < 1)
			return Vector3.zero;
		for (int i = 0; i < dangerO.Count; i++) {
			if(Vector3.Dot(transform.forward,dangerO[i].transform.position-position) < 0)
				dangerO.RemoveAt(i);
		}

		if(dangerO.Count < 1)
			return Vector3.zero;
		for (int i = 0; i < dangerO.Count; i++) {
			if(Mathf.Abs(Vector3.Dot(transform.right, dangerO[i].transform.position-position)) > radius/2)
				dangerO.RemoveAt(i);
			//print (Vector3.Dot(transform.right + position, dangerO[i].transform.position));
		}

		if(dangerO.Count < 1)
			return Vector3.zero;
		float recordDist = float.MaxValue;
		int obj = 0;
		for (int i = 0; i < dangerO.Count; i++) {
			if(Vector3.SqrMagnitude(obstacles[i].transform.position-position) < recordDist){
				recordDist = Vector3.SqrMagnitude(obstacles[i].transform.position-position);
				obj = i;
			}
		}
		if(Vector3.Dot(transform.right,dangerO[obj].transform.position-position) < 0)
			return transform.right * maxSpeed;
		else
			return transform.right * -maxSpeed;
	}

}
