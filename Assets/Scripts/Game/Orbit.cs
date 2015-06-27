using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public Body parent;

	public bool autoInitialize = true;

	public float radius;
	public float inclination;
	public float longitudeOfAscendingNode;

	public Vector3 rotationAxis {
	    get {
			return Quaternion.AngleAxis(-inclination,
            	new Vector3(Mathf.Cos (Mathf.Deg2Rad * longitudeOfAscendingNode), 
			            0,
			            Mathf.Sin (Mathf.Deg2Rad * longitudeOfAscendingNode))) * Vector3.down; } }

	Physics physics;
	Body body;

	LineRenderer lineRenderer;
	const int circleParts = 32;

	// Use this for initialization
	void Start () {
		physics = Physics.Instance;
		body = this.gameObject.GetComponent<Body>();

		if (autoInitialize)
			OrbitFromState();
	}

	void OrbitFromState() {
		// vector pointing from parent's com towards body's com
		var rv = this.transform.position - parent.transform.position;
		// distance between parent's com and body's com
		radius = rv.magnitude;
		// normalised vector pointing from parent's com towards body's com
		var rn = rv / radius;

		var upDot = Vector3.Dot (rn, Vector3.up);

		if (upDot == 0)
		{
			longitudeOfAscendingNode = 0;
			inclination = 0;
		}
		else if (Mathf.Abs (upDot) < 1)
		{
			var side = Vector3.Cross (Vector3.up, rn);
			longitudeOfAscendingNode = Mathf.Rad2Deg * Mathf.Sign (upDot) * Mathf.Atan2(side.x, side.z);

			var projected = Vector3.Cross (side, Vector3.up);
			inclination = Mathf.Rad2Deg * Mathf.Acos (Vector3.Dot (projected, rn));
		}
	}

	void Update () {
		// vector pointing from parent's com towards body's com
		var rv = this.transform.position - parent.transform.position;

		// orbital speed
		var v = Mathf.Sqrt(physics.G * (body.mass + parent.mass) / radius);
		// translation this frame
		var dp = v * Time.deltaTime;
		// rotation this frame
		var da = dp / radius * Mathf.Rad2Deg;

		this.transform.RotateAround(parent.transform.position, rotationAxis, da);
	}

	void LateUpdate() {
		var rv = this.transform.position - parent.transform.position;

		if (lineRenderer)
		{
			for (int i = 0; i < circleParts + 1; i++)
			{
				lineRenderer.SetPosition(i, parent.transform.position + Quaternion.AngleAxis(i * 360 / circleParts, rotationAxis) * rv);
			}
		}
	}

	public void Visualise()
	{
		lineRenderer = gameObject.AddComponent<LineRenderer>();

		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(new Color(1,1,1,.8f), new Color(1,1,1,.8f));
		lineRenderer.SetWidth(.02f, .02f);
		lineRenderer.SetVertexCount(circleParts + 1);

	}

	public void EndVisualise()
	{
		Destroy(lineRenderer);
	}
}
