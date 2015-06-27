using UnityEngine;
using System.Collections;

public class OrbitVisualiser : MonoBehaviour {

	Orbit currentOrbit;

	bool visualiseAll;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (visualiseAll)
			return;

		if (Input.GetMouseButton(1))
		{
			if (currentOrbit)
			{
				currentOrbit.EndVisualise();
				currentOrbit = null;
			}
			return;
		}

		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		var hit = UnityEngine.Physics.Raycast(ray.origin, ray.direction, out hitInfo);

		Orbit orbit;

		if (hit && (orbit = hitInfo.collider.gameObject.GetComponent<Orbit>()) != null)
		{
			if (orbit == currentOrbit)
				return;
			else if (currentOrbit != null)
				currentOrbit.EndVisualise();

			currentOrbit = orbit;
			orbit.Visualise();
		}
		else if (currentOrbit)
		{
			currentOrbit.EndVisualise();
			currentOrbit = null;
		}
	}

	public void ToggleAllVisualise()
	{
		if (currentOrbit)
			currentOrbit.EndVisualise();
		currentOrbit = null;

		var orbits = FindObjectsOfType<Orbit>();

		visualiseAll = !visualiseAll;

		foreach (var o in orbits)
		{
			if (visualiseAll) o.Visualise();
			else o.EndVisualise();
		}
	}
}
