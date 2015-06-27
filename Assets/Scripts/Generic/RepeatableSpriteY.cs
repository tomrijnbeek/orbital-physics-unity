using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SpriteRenderer))]
public class RepeatableSpriteY : MonoBehaviourBase
{
	SpriteRenderer sprite;

	void Awake()
	{
		sprite = this.GetComponent<SpriteRenderer>();

		//Vector2 size = new Vector2(sprite.bounds.size.x / transform.localScale.x, sprite.bounds.size.y / transform.localScale.y);

		// Generate prefab
		var childPrefab = new GameObject();
		var childSprite = childPrefab.AddComponent<SpriteRenderer>();
		childPrefab.transform.position = transform.position;
		childSprite.sprite = sprite.sprite;

		GameObject child;
		for (int i = 1, m = Mathf.RoundToInt(transform.localScale.x); i < m; i++)
		{
			child = (GameObject)Instantiate(childPrefab);
			child.transform.position = transform.position - (new Vector3(1, 0, 0) * i);
			child.transform.parent = transform;
		}

		childPrefab.transform.parent = transform;

		sprite.enabled = false;
	}
}
