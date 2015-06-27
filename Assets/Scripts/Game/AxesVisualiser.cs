using UnityEngine;
using System.Collections;

public class AxesVisualiser : MonoBehaviour {

	public Color[] colors = new Color[] { Color.red, Color.green, Color.blue };
	public float drawLength = 1;

	LineRenderer[] lines;

	void Start() {
		lines = new LineRenderer[3];

		for (int i = 0; i < lines.Length; i++)
		{
			var go = new GameObject(string.Format("axis{0}", i));
			go.transform.parent = this.transform;

			lines[i] = go.AddComponent<LineRenderer>();
			lines[i].material = new Material(Shader.Find("Particles/Additive"));
			lines[i].SetColors(colors[i], colors[i]);
			lines[i].SetWidth(.05f, .05f);
			lines[i].SetVertexCount(2);
			lines[i].SetPosition(0, Vector3.zero);
		}

		lines[0].SetPosition(1, drawLength * new Vector3(1,0,0));
		lines[1].SetPosition(1, drawLength * new Vector3(0,1,0));
		lines[2].SetPosition(1, drawLength * new Vector3(0,0,1));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
