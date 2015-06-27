using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public Body parent;
	Physics physics;
	Body body;

	// Use this for initialization
	void Start () {
		physics = Physics.Instance;
		body = this.gameObject.GetComponent<Body>();
	}

	void Update () {
		// vector pointing from parent's com towards body's com
		var rv = this.transform.position - parent.transform.position;
		// distance between parent's com and body's com
		var r = rv.magnitude;

		// orbital speed
		var v = Mathf.Sqrt(physics.G * (body.mass + parent.mass) / r);
		// translation this frame
		var dp = v * Time.deltaTime;
		// rotation this frame
		var da = dp / r * Mathf.Rad2Deg;

		this.transform.RotateAround(parent.transform.position, -Vector3.up, da);
	}
}
