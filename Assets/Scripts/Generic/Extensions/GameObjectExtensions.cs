using UnityEngine;
using System.Linq;
using System.Collections;

public static class GameObjectExtensions
{
	public static T GetSafeComponent<T>(this GameObject obj) where T : MonoBehaviour
	{
		T component = obj.GetComponent<T>();
		
		if(component == null)
		{
			Debug.LogError("Expected to find component of type " 
			               + typeof(T) + " but found none", obj);
		}
		
		return component;
	}

	public static I GetInterfaceComponent<I>(this GameObject obj) where I : class
	{
		return obj.GetComponent(typeof(I)) as I;
	}

	public static I[] GetInterfaceComponents<I>(this GameObject obj) where I : class
	{
		return obj.GetComponents(typeof(I)).Select (c => c as I).ToArray();
	}
}
