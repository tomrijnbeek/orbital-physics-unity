using UnityEngine;
using System.Collections.Generic;

public class MonoBehaviourBase : MonoBehaviour
{
	public delegate void Task(float time);

	#region Methods
	/// <summary>
	/// Invoke the specified method with the given parameter.
	/// </summary>
	/// <param name="task">Task.</param>
	/// <param name="time">Time.</param>
	public void Invoke(Task task, float time)
	{
		this.Invoke(task.Method.Name, time);
	}

	/// <summary>
	/// Gets the component that implements an interface I.
	/// </summary>
	/// <returns>The component implementing I.</returns>
	/// <typeparam name="I">The interface that should be implemented by the component.</typeparam>
	public I GetInterfaceComponent<I>() where I : class
	{
		return this.GetComponent(typeof(I)) as I;
	}
	#endregion

	#region Static methods
	/// <summary>
	/// Finds all objects with a component that implements a certain interface I.
	/// </summary>
	/// <returns>The objects of with a component implementing interface I.</returns>
	/// <typeparam name="I">The interface that should be implemented by the components.</typeparam>
	public static List<I> FindObjectsOfInterface<I>() where I : class
	{
		var monoBehaviours = MonoBehaviour.FindObjectsOfType<MonoBehaviour>();
		var list = new List<I>();
		
		foreach (var behaviour in monoBehaviours)
		{
			I component = behaviour.GetComponent(typeof(I)) as I;
			
			if (component != null)
			{
				list.Add(component);
			}
		}
		
		return list;
	}
	#endregion
}
