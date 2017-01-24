using UnityEngine;
using System.Collections;

public class IslandProperties : MonoBehaviour {
	public float radius;
	public bool debug;

	public Material red;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnRenderObject(){
		red.SetPass (0);

		if (!debug)
			return;
		GL.Begin (GL.LINES);
		float rounds = 100;
		for (int i = 0; i < rounds; i++) {
			float pi2 = Mathf.PI*2;

			float rad = pi2*((float)i/rounds);
			float rad2 = pi2*((float)(i+1)/rounds);

			float x1 = Mathf.Sin(rad)*radius;
			float z1 = Mathf.Cos(rad)*radius;
			float x2 = Mathf.Sin(rad2)*radius;
			float z2 = Mathf.Cos(rad2)*radius;

			GL.Vertex(new Vector3(x1+transform.position.x,transform.position.y,z1+transform.position.z));
			GL.Vertex(new Vector3(x2+transform.position.x,transform.position.y,z2+transform.position.z));
		}
		GL.End ();
	}
}
