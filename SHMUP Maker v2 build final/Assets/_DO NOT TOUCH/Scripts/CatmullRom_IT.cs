///Original integration of Catmull-Rom spline in Unity by JPBotelho under MIT license (https://github.com/JPBotelho/Catmull-Rom-Splines); simplified by Ian Thecleric

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*  
	Catmull-Rom splines are Hermite curves with special tangent values.
	Hermite curve formula:
	(2t^3 - 3t^2 + 1) * p0 + (t^3 - 2t^2 + t) * m0 + (-2t^3 + 3t^2) * p1 + (t^3 - t^2) * m1
	For points p0 and p1 passing through points m0 and m1 interpolated over t = [0, 1]
	Tangent M[k] = (P[k+1] - P[k-1]) / 2
*/
public class CatmullRom_IT
{
	public Vector2[] splinePoints; //Generated spline points
	public int resolution; //Amount of points between control points. [Tesselation factor]

	private Vector2[] controlPoints;


	public CatmullRom_IT(Vector2[] ctrlPts)
	{
		controlPoints = ctrlPts;

		resolution = 6;

		GenerateSplinePoints();
	}


	//Sets the length of the point array based on resolution
	private void InitializeSplinePointsArray()
	{
		int pointsToCreate = resolution * (controlPoints.Length - 1);
		splinePoints = new Vector2[pointsToCreate];
	}


	//Math stuff to generate the spline points
	private void GenerateSplinePoints()
	{
		InitializeSplinePointsArray();

		Vector2 p0, p1; //for each segment: start point, end point
		Vector2 m0, m1; //for each segment: tangents

		// First for loop goes through each individual control point and connects it to the next, so 0-1, 1-2, 2-3 and so on
		for (int currentPoint = 0; currentPoint < controlPoints.Length - 1; currentPoint++)
		{
			p0 = controlPoints[currentPoint];
			p1 = controlPoints[currentPoint + 1];

			// Tangent M[k] = (P[k+1] - P[k-1]) / 2
			m0 = (currentPoint == 0 ? (p1 - p0) : (p1 - controlPoints[currentPoint - 1])) * .5f;
			m1 = (currentPoint < controlPoints.Length - 2 ? (controlPoints[(currentPoint + 2) % controlPoints.Length] - p0) : (p1 - p0)) * .5f;

			float pointStep = 1.0f / (resolution - (currentPoint != controlPoints.Length - 2 ? 0 : 1));
			//...test w last point coz last point of last segment should reach p1

			// Creates [resolution] points between this control point and the next
			for (int tesselatedPoint = 0; tesselatedPoint < resolution; tesselatedPoint++)
			{
				float t = tesselatedPoint * pointStep;

				Vector2 point = CalculatePosition(p0, p1, m0, m1, t);

				splinePoints[currentPoint * resolution + tesselatedPoint] = point;
			}
		}
	}


	//Calculates curve position at t[0, 1] ([0, 1] means clamped between 0 and 1)
	private static Vector2 CalculatePosition(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t)
	{
		// Hermite curve formula:
		// (2t^3 - 3t^2 + 1) * p0 + (t^3 - 2t^2 + t) * m0 + (-2t^3 + 3t^2) * p1 + (t^3 - t^2) * m1
		Vector2 position = (2f * t * t * t - 3f * t * t + 1f) * start
			+ (t * t * t - 2f * t * t + t) * tanPoint1
			+ (-2f * t * t * t + 3f * t * t) * end
			+ (t * t * t - t * t) * tanPoint2;

		return position;
	}

}
