﻿using UnityEngine;
using System.Collections;

public static class ColorExtensions {

	public static Color WithAlpha(this Color color, float a)
	{
		return new Color(color.r, color.g, color.b, a);
	}
}
