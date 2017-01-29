using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ShipMovement : MonoBehaviour {

	protected Vector3 position;
	protected Vector3 velocity;
	protected Vector3 acceleration;

	GameObject[] obstacles;
	List<GameObject> dangerO;

	public float mass;
	public float distanceDetect;
	public float radius;
	public float maxSpeed;
	public float destinationDist;

	public Material green;
	public bool debug;
	// Use this for initialization
	public virtual void Start () {
		dangerO = new List<GameObject> ();
		velocity = Vector3.zero;
		acceleration = Vector3.zero;
	}
	public virtual void CalculateSteering(){

	}
	// Update is called once per frame
	void Update () {
		position = transform.position;
		CalculateSteering ();

		velocity += acceleration*Time.deltaTime;
		velocity = Vector3.ClampMagnitude (velocity, maxSpeed);
		position += velocity*Time.deltaTime;
		UpdateTransformation ();

		acceleration = Vector3.zero;
	}
	public void ApplyForce(Vector3 force){
		acceleration += force / mass;
	}
	void UpdateTransformation(){
		/*if (velocity.sqrMagnitude < .05f)
			return;*/
		transform.position = position;
		transform.forward = Vector3.Lerp(transform.forward,velocity.normalized,.1f);
		transform.forward = new Vector3 (transform.forward.x, 0, transform.forward.z);
	}
	public Vector3 Seek(Vector3 target){
		return ((target-position)-velocity).normalized * maxSpeed;
	}
	public Vector3 ObstacleAvoid(){
		Vector3 avoidVec = Vector3.zero;
		obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		dangerO.Clear ();

		//Obstacles within range
		for (int i = 0; i< obstacles.Length; i++) {
			if(Vector3.SqrMagnitude(obstacles[i].transform.position-position) < distanceDetect*distanceDetect)
				dangerO.Add(obstacles[i]);
		}
		if(dangerO.Count < 1)
			return Vector3.zero;

		//Obstacles in front
		for (int i = 0; i < dangerO.Count; i++) {
			if (Vector3.Dot (transform.forward, dangerO [i].transform.position - position) < 0)
				dangerO.RemoveAt (i);
		}
		if(dangerO.Count < 1)
			return Vector3.zero;

		//Obstacles distance from velocity perpindicular
		for (int i = 0; i < dangerO.Count; i++) {
			float dist = radius + dangerO[i].GetComponent<IslandProperties>().radius;
			if (Mathf.Abs (Vector3.Dot (transform.right, dangerO [i].transform.position - position)) > dist)
				dangerO.RemoveAt (i);
		}
		if(dangerO.Count < 1)
			return Vector3.zero;


		//Closest Obstacle
		float recordDist = float.MaxValue;
		//int obj = 0;
		for (int i = 0; i < dangerO.Count; i++) {
			if(Vector3.Magnitude(obstacles[i].transform.position-position) < recordDist){
				recordDist = Vector3.Magnitude(obstacles[i].transform.position-position);
				//obj = i;
			}
		}

		//Add avoid vectors based on distance from object
		for (int i = 0; i < dangerO.Count; i++) {
			if (Vector3.Dot (transform.right, dangerO [i].transform.position - position) < 0){
				avoidVec += transform.right * maxSpeed
					*(Vector3.Magnitude(obstacles[i].transform.position-position)/recordDist);
			}
			else{
				avoidVec += transform.right * -maxSpeed 
					*(Vector3.Magnitude(obstacles[i].transform.position-position)/recordDist);
			}
		}
		return avoidVec;
		/*
		//Turn left
		if(Vector3.Dot(transform.right,dangerO[obj].transform.position-position) < 0)
			return transform.right * maxSpeed;
		//Turn right
		else
			return transform.right * -maxSpeed;
		*/
	}
	public Vector3 Friction(GameObject target,float coefficient){
		return (velocity.normalized * -1) * coefficient;
	}
	void OnRenderObject(){
		if (!debug)
			return;
		green.SetPass (0);
		GL.Begin (GL.LINES);
		GL.Vertex (position);
		GL.Vertex (position + velocity);
		GL.End ();
	}

}
