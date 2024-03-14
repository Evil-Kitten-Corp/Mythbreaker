using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ME_TrailRendererNoise : MonoBehaviour
{
	[Range(0.01f, 10f)]
	public float MinVertexDistance = 0.1f;

	public float VertexTime = 1f;

	public float TotalLifeTime = 3f;

	public bool SmoothCurves;

	public bool IsRibbon;

	public bool IsActive = true;

	[Range(0.001f, 10f)]
	public float Frequency = 1f;

	[Range(0.001f, 10f)]
	public float TimeScale = 0.1f;

	[Range(0.001f, 10f)]
	public float Amplitude = 1f;

	public float Gravity = 1f;

	public float TurbulenceStrength = 1f;

	public bool AutodestructWhenNotActive;

	private LineRenderer lineRenderer;

	private Transform t;

	private Vector3 prevPos;

	private List<Vector3> points = new List<Vector3>(500);

	private List<float> lifeTimes = new List<float>(500);

	private List<Vector3> velocities = new List<Vector3>(500);

	private float randomOffset;

	private List<Vector3> controlPoints = new List<Vector3>();

	private int curveCount;

	private const float MinimumSqrDistance = 0.01f;

	private const float DivisionThreshold = -0.99f;

	private const float SmoothCurvesScale = 0.5f;

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = true;
		t = base.transform;
		prevPos = t.position;
		points.Insert(0, t.position);
		lifeTimes.Insert(0, VertexTime);
		velocities.Insert(0, Vector3.zero);
		randomOffset = (float)Random.Range(0, 10000000) / 1000000f;
	}

	private void OnEnable()
	{
		points.Clear();
		lifeTimes.Clear();
		velocities.Clear();
	}

	private void Update()
	{
		if (IsActive)
		{
			AddNewPoints();
		}
		UpdatetPoints();
		if (SmoothCurves && points.Count > 2)
		{
			UpdateLineRendererBezier();
		}
		else
		{
			UpdateLineRenderer();
		}
		if (AutodestructWhenNotActive && !IsActive && points.Count <= 1)
		{
			Object.Destroy(base.gameObject, TotalLifeTime);
		}
	}

	private void AddNewPoints()
	{
		if ((t.position - prevPos).magnitude > MinVertexDistance || (IsRibbon && points.Count == 0) || (IsRibbon && points.Count > 0 && (t.position - points[0]).magnitude > MinVertexDistance))
		{
			prevPos = t.position;
			points.Insert(0, t.position);
			lifeTimes.Insert(0, VertexTime);
			velocities.Insert(0, Vector3.zero);
		}
	}

	private void UpdatetPoints()
	{
		for (int i = 0; i < lifeTimes.Count; i++)
		{
			lifeTimes[i] -= Time.deltaTime;
			if (lifeTimes[i] <= 0f)
			{
				int removedRange = lifeTimes.Count - i;
				lifeTimes.RemoveRange(i, removedRange);
				points.RemoveRange(i, removedRange);
				velocities.RemoveRange(i, removedRange);
				break;
			}
			CalculateTurbuelence(points[i], TimeScale, Frequency, Amplitude, Gravity, i);
		}
	}

	private void UpdateLineRendererBezier()
	{
		if (SmoothCurves && points.Count > 2)
		{
			InterpolateBezier(points, 0.5f);
			List<Vector3> bezierPositions = GetDrawingPoints();
			lineRenderer.positionCount = bezierPositions.Count - 1;
			lineRenderer.SetPositions(bezierPositions.ToArray());
		}
	}

	private void UpdateLineRenderer()
	{
		lineRenderer.positionCount = Mathf.Clamp(points.Count - 1, 0, int.MaxValue);
		lineRenderer.SetPositions(points.ToArray());
	}

	private void CalculateTurbuelence(Vector3 position, float speed, float scale, float height, float gravity, int index)
	{
		float sTime = Time.timeSinceLevelLoad * speed + randomOffset;
		float xCoord = position.x * scale + sTime;
		float yCoord = position.y * scale + sTime + 10f;
		float zCoord = position.z * scale + sTime + 25f;
		position.x = (Mathf.PerlinNoise(yCoord, zCoord) - 0.5f) * height * Time.deltaTime;
		position.y = (Mathf.PerlinNoise(xCoord, zCoord) - 0.5f) * height * Time.deltaTime - gravity * Time.deltaTime;
		position.z = (Mathf.PerlinNoise(xCoord, yCoord) - 0.5f) * height * Time.deltaTime;
		points[index] += position * TurbulenceStrength;
	}

	public void InterpolateBezier(List<Vector3> segmentPoints, float scale)
	{
		controlPoints.Clear();
		if (segmentPoints.Count < 2)
		{
			return;
		}
		for (int i = 0; i < segmentPoints.Count; i++)
		{
			if (i == 0)
			{
				Vector3 p2 = segmentPoints[i];
				Vector3 tangent2 = segmentPoints[i + 1] - p2;
				Vector3 q3 = p2 + scale * tangent2;
				controlPoints.Add(p2);
				controlPoints.Add(q3);
			}
			else if (i == segmentPoints.Count - 1)
			{
				Vector3 p = segmentPoints[i - 1];
				Vector3 p4 = segmentPoints[i];
				Vector3 tangent3 = p4 - p;
				Vector3 q = p4 - scale * tangent3;
				controlPoints.Add(q);
				controlPoints.Add(p4);
			}
			else
			{
				Vector3 p0 = segmentPoints[i - 1];
				Vector3 p3 = segmentPoints[i];
				Vector3 p5 = segmentPoints[i + 1];
				Vector3 tangent = (p5 - p0).normalized;
				Vector3 q0 = p3 - scale * tangent * (p3 - p0).magnitude;
				Vector3 q2 = p3 + scale * tangent * (p5 - p3).magnitude;
				controlPoints.Add(q0);
				controlPoints.Add(p3);
				controlPoints.Add(q2);
			}
		}
		curveCount = (controlPoints.Count - 1) / 3;
	}

	public List<Vector3> GetDrawingPoints()
	{
		List<Vector3> drawingPoints = new List<Vector3>();
		for (int curveIndex = 0; curveIndex < curveCount; curveIndex++)
		{
			List<Vector3> bezierCurveDrawingPoints = FindDrawingPoints(curveIndex);
			if (curveIndex != 0)
			{
				bezierCurveDrawingPoints.RemoveAt(0);
			}
			drawingPoints.AddRange(bezierCurveDrawingPoints);
		}
		return drawingPoints;
	}

	private List<Vector3> FindDrawingPoints(int curveIndex)
	{
		List<Vector3> pointList = new List<Vector3>();
		Vector3 left = CalculateBezierPoint(curveIndex, 0f);
		Vector3 right = CalculateBezierPoint(curveIndex, 1f);
		pointList.Add(left);
		pointList.Add(right);
		FindDrawingPoints(curveIndex, 0f, 1f, pointList, 1);
		return pointList;
	}

	private int FindDrawingPoints(int curveIndex, float t0, float t1, List<Vector3> pointList, int insertionIndex)
	{
		Vector3 left = CalculateBezierPoint(curveIndex, t0);
		Vector3 right = CalculateBezierPoint(curveIndex, t1);
		if ((left - right).sqrMagnitude < 0.01f)
		{
			return 0;
		}
		float tMid = (t0 + t1) / 2f;
		Vector3 mid = CalculateBezierPoint(curveIndex, tMid);
		Vector3 normalized = (left - mid).normalized;
		Vector3 rightDirection = (right - mid).normalized;
		if (Vector3.Dot(normalized, rightDirection) > -0.99f || Mathf.Abs(tMid - 0.5f) < 0.0001f)
		{
			int pointsAddedCount = 0;
			pointsAddedCount += FindDrawingPoints(curveIndex, t0, tMid, pointList, insertionIndex);
			pointList.Insert(insertionIndex + pointsAddedCount, mid);
			pointsAddedCount++;
			return pointsAddedCount + FindDrawingPoints(curveIndex, tMid, t1, pointList, insertionIndex + pointsAddedCount);
		}
		return 0;
	}

	public Vector3 CalculateBezierPoint(int curveIndex, float t)
	{
		int nodeIndex = curveIndex * 3;
		Vector3 p0 = controlPoints[nodeIndex];
		Vector3 p1 = controlPoints[nodeIndex + 1];
		Vector3 p2 = controlPoints[nodeIndex + 2];
		Vector3 p3 = controlPoints[nodeIndex + 3];
		return CalculateBezierPoint(t, p0, p1, p2, p3);
	}

	private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float u = 1f - t;
		float tt = t * t;
		float uu = u * u;
		float num = uu * u;
		float ttt = tt * t;
		return num * p0 + 3f * uu * t * p1 + 3f * u * tt * p2 + ttt * p3;
	}
}
