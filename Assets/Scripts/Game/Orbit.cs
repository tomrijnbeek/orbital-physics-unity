using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public Body parent;
	Physics physics;
	Body body;

	LineRenderer lineRenderer;
	const int circleParts = 32;

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

	void LateUpdate() {
		var rv = this.transform.position - parent.transform.position;

		if (lineRenderer)
		{
			for (int i = 0; i < circleParts + 1; i++)
			{
				lineRenderer.SetPosition(i, parent.transform.position + Quaternion.AngleAxis(i * 360 / circleParts, -Vector3.up) * rv);
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
