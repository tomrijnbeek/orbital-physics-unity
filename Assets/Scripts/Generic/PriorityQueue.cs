using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PriorityQueue <T> {
	private List<T> elementList = new List<T>();
	private List<float> priorityList = new List<float>();
	
	public void Add (T item, float priority) {
		elementList.Add(item);
		priorityList.Add(priority);
	}
	
	public void Sort () {
		Quicksort(0,elementList.Count-1);
	}

	public T Pop () {
		T item = elementList[0];
		RemoveAt(0);
		return item;
	}
	
	public void Remove (T item) {
		int index = elementList.IndexOf(item);
		elementList.RemoveAt(index);
		priorityList.RemoveAt(index);
	}
	
	public void RemoveAt (int index) {
		elementList.RemoveAt(index);
		priorityList.RemoveAt(index);
	}
	
	public float GetPriority (T item) {
		int index = elementList.IndexOf(item);
		return priorityList[index];
	}
	
	public float GetPriorityAt (int index) {
		return priorityList[index];
	}
	
	public void SetPriority (T item, float priority) {
		int index = elementList.IndexOf(item);
		priorityList[index] = priority;
	}
	
	public void SetPriorityAt (int index, float priority) {
		priorityList[index] = priority;
	}
	
	public int IndexOf (T item) {
		return elementList.IndexOf(item);
	}
	
	public bool Contains (T item) {
		return elementList.Contains(item);
	}
	
	public void Clear () {
		elementList.Clear();
		priorityList.Clear();
	}
	
	public T this[int index] {
		get {
			return elementList[index];
		}
		set {
			elementList[index] = value;
		}
	}
	
	public int Count {
		get {
			return elementList.Count;
		}
	}
	
	// Quicksort implementation by FERHAT from http://snipd.net/quicksort-in-c
	private void Quicksort(int left, int right) {
		int i = left, j = right;
		float pivot = priorityList[(left + right) / 2];
		
		while (i <= j) {
			while (priorityList[i] < pivot)
				i++;
			
			while (priorityList[j] > pivot)
				j--;
			
			if (i <= j) { //swap the elements and priorities
				T tmp = elementList[i];
				elementList[i] = elementList[j];
				elementList[j] = tmp;
				
				float ptmp = priorityList[i];
				priorityList[i] = priorityList[j];
				priorityList[j] = ptmp;
				
				i++;
				j--;
			}
		}
		
		if (left < j)
			Quicksort(left, j);
		
		if (i < right)
			Quicksort(i, right);
	}
}