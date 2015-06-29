using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public Body parent;

	public bool autoInitialize = true;

	public float periapsis, apoapsis;
	public float inclination;
	public float longitudeOfAscendingNode;
	public float argumentOfPeriapsis;

	public Vector3 rotationAxis {
	    get {
			return Quaternion.AngleAxis(-inclination,
            	new Vector3(Mathf.Cos (Mathf.Deg2Rad * longitudeOfAscendingNode), 
			            0,
			            Mathf.Sin (Mathf.Deg2Rad * longitudeOfAscendingNode))) * Vector3.down; } }
	public float semiMajorAxis { get { return .5f * (apoapsis + periapsis); } }
	public float semiMinorAxis { get { return semiMajorAxis * Mathf.Sqrt(1 - eccentricity * eccentricity); } }
	public float eccentricity { get { return (apoapsis - periapsis) / (apoapsis + periapsis); } }
	public float longitudeOfPeriapsis { get { return longitudeOfAscendingNode + argumentOfPeriapsis; } }

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
		apoapsis = periapsis = rv.magnitude;
		// normalised vector pointing from parent's com towards body's com
		var rn = rv / periapsis;

		var upDot = Vector3.Dot (rn, Vector3.up);

		if (upDot == 0)
		{
			longitudeOfAscendingNode = Mathf.Rad2Deg * Mathf.Atan2(rv.z, rv.x);
			argumentOfPeriapsis = 0;
			inclination = 0;
		}
		else
		{
			var side = Vector3.Cross (Vector3.up, rn);
			longitudeOfAscendingNode = Mathf.Rad2Deg * Mathf.Sign (upDot) * Mathf.Atan2(side.x, side.z);

			argumentOfPeriapsis = 90;

			var projected = Vector3.Cross (side, Vector3.up);
			inclination = Mathf.Rad2Deg * Mathf.Acos (Vector3.Dot (projected, rn));
		}
	}

	void Update () {
		var a = semiMajorAxis;
		var b = semiMinorAxis;
		var e = eccentricity;

		var toPeriapsis = new Vector3(Mathf.Cos (Mathf.Deg2Rad * longitudeOfPeriapsis) * Mathf.Cos (Mathf.Deg2Rad * inclination), Mathf.Sin (Mathf.Deg2Rad * inclination), Mathf.Sin (Mathf.Deg2Rad * longitudeOfPeriapsis) * Mathf.Cos (Mathf.Deg2Rad * inclination));
		var toBody = (transform.position - parent.transform.position).normalized;

		var center = parent.transform.position - (a - periapsis) * toPeriapsis;
		var normal = Vector3.Cross (Vector3.up, toPeriapsis);

		// mean motion
		var n = Mathf.Sqrt(physics.G * (body.mass + parent.mass) / (a * a * a));

		// current true anomaly - angle between periapsis and position w.r.t. planet
		var v = Mathf.Acos (Vector3.Dot(toPeriapsis, toBody));
		if (Vector3.Dot (toBody, normal) < 0)
			v = Mathf.PI * 2 - v;

		// current eccentric anomaly - angle between periapsis and position w.r.t. center
		var sinE = Mathf.Sin(v)*Mathf.Sqrt(1 - e*e)/(1 + e * Mathf.Cos(v));
		var cosE = (e + Mathf.Cos(v))/(1 + e * Mathf.Cos(v));
		var E = Mathf.Atan2(sinE, cosE);

		// current mean anomaly
		var M = E - e * Mathf.Sin(E);

		// advance the mean anomaly
		M += n * Time.deltaTime;

		// make sure the mean anomaly lies in [-pi,pi]
		M %= 2 * Mathf.PI;
		if (M < -Mathf.PI)
			M += 2 * Mathf.PI;
		else if (M > Mathf.PI)
			M -= 2 * Mathf.PI;

		// calculate the new mean anomaly	
		if (M < 0)
			E = M - e;
		else
			E = M + e;
			
		var Enew = E;
		var first = true;

		do
		{
			E = Enew;
			Enew = E + (M - E + e*Mathf.Sin(E))/(1 - e*Mathf.Cos(E));
		} while ((Enew - E) > Mathf.Epsilon);
		E = Enew;

		// calculate new new current anomaly
//		var sinv = Mathf.Sin(E)*Mathf.Sqrt(1 - e^2)/(1 - e * Mathf.Cos(E));
//		var cosv = (Mathf.Cos(E) - e)/(1 - e * Mathf.Cos(E));
//		v = Mathf.Atan2(sinv, cosv);

        transform.position = center + a * Mathf.Cos (E) * toPeriapsis + b * Mathf.Sin (E) * normal;
	}



	void LateUpdate() {
		if (lineRenderer)
		{
			var a = semiMajorAxis;
			var b = semiMinorAxis;
			var toPeriapsis = new Vector3(Mathf.Cos (Mathf.Deg2Rad * longitudeOfPeriapsis) * Mathf.Cos (Mathf.Deg2Rad * inclination), Mathf.Sin (Mathf.Deg2Rad * inclination), Mathf.Sin (Mathf.Deg2Rad * longitudeOfPeriapsis) * Mathf.Cos (Mathf.Deg2Rad * inclination));
			var center = parent.transform.position - (semiMajorAxis - periapsis) * toPeriapsis;
			var normal = Vector3.Cross (Vector3.up, toPeriapsis);

			for (int i = 0; i < circleParts + 1; i++)
			{
				lineRenderer.SetPosition(i, center + a * Mathf.Cos (i * 2 * Mathf.PI / circleParts) * toPeriapsis + b * Mathf.Sin (i * 2 * Mathf.PI / circleParts) * normal);
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
