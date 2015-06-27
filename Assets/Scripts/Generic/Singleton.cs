using UnityEngine;

/// <summary>
/// Every script inheriting from this class will automatically be a singleton.
/// </summary>
public class Singleton<T> : MonoBehaviourBase where T : MonoBehaviourBase
{
	protected static T instance;
	
	/// <summary>
	/// Returns an instance of the type.
	/// </summary>
	/// <value>The instance.</value>
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));
				
				if (instance == null)
				{
					Debug.LogError("An instance of " + typeof(T) + 
					               " is needed in the scene, but there is none.");
				}
			}
			
			return instance;
		}
	}
}