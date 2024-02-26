// Decompiled with JetBrains decompiler
// Type: ME_TrailRendererNoise
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
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
    this.lineRenderer = this.GetComponent<LineRenderer>();
    this.lineRenderer.useWorldSpace = true;
    this.t = this.transform;
    this.prevPos = this.t.position;
    this.points.Insert(0, this.t.position);
    this.lifeTimes.Insert(0, this.VertexTime);
    this.velocities.Insert(0, Vector3.zero);
    this.randomOffset = (float) Random.Range(0, 10000000) / 1000000f;
  }

  private void OnEnable()
  {
    this.points.Clear();
    this.lifeTimes.Clear();
    this.velocities.Clear();
  }

  private void Update()
  {
    if (this.IsActive)
      this.AddNewPoints();
    this.UpdatetPoints();
    if (this.SmoothCurves && this.points.Count > 2)
      this.UpdateLineRendererBezier();
    else
      this.UpdateLineRenderer();
    if (!this.AutodestructWhenNotActive || this.IsActive || this.points.Count > 1)
      return;
    Object.Destroy((Object) this.gameObject, this.TotalLifeTime);
  }

  private void AddNewPoints()
  {
    if ((double) (this.t.position - this.prevPos).magnitude <= (double) this.MinVertexDistance && (!this.IsRibbon || this.points.Count != 0) && (!this.IsRibbon || this.points.Count <= 0 || (double) (this.t.position - this.points[0]).magnitude <= (double) this.MinVertexDistance))
      return;
    this.prevPos = this.t.position;
    this.points.Insert(0, this.t.position);
    this.lifeTimes.Insert(0, this.VertexTime);
    this.velocities.Insert(0, Vector3.zero);
  }

  private void UpdatetPoints()
  {
    for (int index = 0; index < this.lifeTimes.Count; ++index)
    {
      this.lifeTimes[index] -= Time.deltaTime;
      if ((double) this.lifeTimes[index] <= 0.0)
      {
        int count = this.lifeTimes.Count - index;
        this.lifeTimes.RemoveRange(index, count);
        this.points.RemoveRange(index, count);
        this.velocities.RemoveRange(index, count);
        break;
      }
      this.CalculateTurbuelence(this.points[index], this.TimeScale, this.Frequency, this.Amplitude, this.Gravity, index);
    }
  }

  private void UpdateLineRendererBezier()
  {
    if (!this.SmoothCurves || this.points.Count <= 2)
      return;
    this.InterpolateBezier(this.points, 0.5f);
    List<Vector3> drawingPoints = this.GetDrawingPoints();
    this.lineRenderer.positionCount = drawingPoints.Count - 1;
    this.lineRenderer.SetPositions(drawingPoints.ToArray());
  }

  private void UpdateLineRenderer()
  {
    this.lineRenderer.positionCount = Mathf.Clamp(this.points.Count - 1, 0, int.MaxValue);
    this.lineRenderer.SetPositions(this.points.ToArray());
  }

  private void CalculateTurbuelence(
    Vector3 position,
    float speed,
    float scale,
    float height,
    float gravity,
    int index)
  {
    float num1 = Time.timeSinceLevelLoad * speed + this.randomOffset;
    float x = position.x * scale + num1;
    float num2 = (float) ((double) position.y * (double) scale + (double) num1 + 10.0);
    float y = (float) ((double) position.z * (double) scale + (double) num1 + 25.0);
    position.x = (Mathf.PerlinNoise(num2, y) - 0.5f) * height * Time.deltaTime;
    position.y = (float) (((double) Mathf.PerlinNoise(x, y) - 0.5) * (double) height * (double) Time.deltaTime - (double) gravity * (double) Time.deltaTime);
    position.z = (Mathf.PerlinNoise(x, num2) - 0.5f) * height * Time.deltaTime;
    this.points[index] += position * this.TurbulenceStrength;
  }

  public void InterpolateBezier(List<Vector3> segmentPoints, float scale)
  {
    this.controlPoints.Clear();
    if (segmentPoints.Count < 2)
      return;
    for (int index = 0; index < segmentPoints.Count; ++index)
    {
      if (index == 0)
      {
        Vector3 segmentPoint = segmentPoints[index];
        Vector3 vector3_1 = segmentPoints[index + 1] - segmentPoint;
        Vector3 vector3_2 = segmentPoint + scale * vector3_1;
        this.controlPoints.Add(segmentPoint);
        this.controlPoints.Add(vector3_2);
      }
      else if (index == segmentPoints.Count - 1)
      {
        Vector3 segmentPoint1 = segmentPoints[index - 1];
        Vector3 segmentPoint2 = segmentPoints[index];
        Vector3 vector3 = segmentPoint2 - segmentPoint1;
        this.controlPoints.Add(segmentPoint2 - scale * vector3);
        this.controlPoints.Add(segmentPoint2);
      }
      else
      {
        Vector3 segmentPoint3 = segmentPoints[index - 1];
        Vector3 segmentPoint4 = segmentPoints[index];
        Vector3 segmentPoint5 = segmentPoints[index + 1];
        Vector3 normalized = (segmentPoint5 - segmentPoint3).normalized;
        Vector3 vector3_3 = segmentPoint4 - scale * normalized * (segmentPoint4 - segmentPoint3).magnitude;
        Vector3 vector3_4 = segmentPoint4 + scale * normalized * (segmentPoint5 - segmentPoint4).magnitude;
        this.controlPoints.Add(vector3_3);
        this.controlPoints.Add(segmentPoint4);
        this.controlPoints.Add(vector3_4);
      }
    }
    this.curveCount = (this.controlPoints.Count - 1) / 3;
  }

  public List<Vector3> GetDrawingPoints()
  {
    List<Vector3> drawingPoints1 = new List<Vector3>();
    for (int curveIndex = 0; curveIndex < this.curveCount; ++curveIndex)
    {
      List<Vector3> drawingPoints2 = this.FindDrawingPoints(curveIndex);
      if (curveIndex != 0)
        drawingPoints2.RemoveAt(0);
      drawingPoints1.AddRange((IEnumerable<Vector3>) drawingPoints2);
    }
    return drawingPoints1;
  }

  private List<Vector3> FindDrawingPoints(int curveIndex)
  {
    List<Vector3> pointList = new List<Vector3>();
    Vector3 bezierPoint1 = this.CalculateBezierPoint(curveIndex, 0.0f);
    Vector3 bezierPoint2 = this.CalculateBezierPoint(curveIndex, 1f);
    pointList.Add(bezierPoint1);
    pointList.Add(bezierPoint2);
    this.FindDrawingPoints(curveIndex, 0.0f, 1f, pointList, 1);
    return pointList;
  }

  private int FindDrawingPoints(
    int curveIndex,
    float t0,
    float t1,
    List<Vector3> pointList,
    int insertionIndex)
  {
    Vector3 bezierPoint1 = this.CalculateBezierPoint(curveIndex, t0);
    Vector3 bezierPoint2 = this.CalculateBezierPoint(curveIndex, t1);
    if ((double) (bezierPoint1 - bezierPoint2).sqrMagnitude < 0.0099999997764825821)
      return 0;
    float num1 = (float) (((double) t0 + (double) t1) / 2.0);
    Vector3 bezierPoint3 = this.CalculateBezierPoint(curveIndex, num1);
    if ((double) Vector3.Dot((bezierPoint1 - bezierPoint3).normalized, (bezierPoint2 - bezierPoint3).normalized) <= -0.99000000953674316 && (double) Mathf.Abs(num1 - 0.5f) >= 9.9999997473787516E-05)
      return 0;
    int num2 = 0 + this.FindDrawingPoints(curveIndex, t0, num1, pointList, insertionIndex);
    pointList.Insert(insertionIndex + num2, bezierPoint3);
    int num3 = num2 + 1;
    return num3 + this.FindDrawingPoints(curveIndex, num1, t1, pointList, insertionIndex + num3);
  }

  public Vector3 CalculateBezierPoint(int curveIndex, float t)
  {
    int index = curveIndex * 3;
    Vector3 controlPoint1 = this.controlPoints[index];
    Vector3 controlPoint2 = this.controlPoints[index + 1];
    Vector3 controlPoint3 = this.controlPoints[index + 2];
    Vector3 controlPoint4 = this.controlPoints[index + 3];
    return this.CalculateBezierPoint(t, controlPoint1, controlPoint2, controlPoint3, controlPoint4);
  }

  private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
  {
    float num1 = 1f - t;
    float num2 = t * t;
    float num3 = num1 * num1;
    double num4 = (double) num3 * (double) num1;
    float num5 = num2 * t;
    Vector3 vector3 = p0;
    return (float) num4 * vector3 + 3f * num3 * t * p1 + 3f * num1 * num2 * p2 + num5 * p3;
  }
}
