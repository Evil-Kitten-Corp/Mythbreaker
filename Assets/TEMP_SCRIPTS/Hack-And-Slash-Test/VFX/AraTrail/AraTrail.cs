// Decompiled with JetBrains decompiler
// Type: Ara.AraTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

#nullable disable
namespace Ara
{
  [ExecuteInEditMode]
  public class AraTrail : MonoBehaviour
  {
    public const float epsilon = 1E-05f;
    [Header("Overall")]
    [Tooltip("Trail cross-section asset, determines the shape of the emitted trail. If no asset is specified, the trail will be a simple strip.")]
    public TrailSection section;
    [Tooltip("Whether to use world or local space to generate and simulate the trail.")]
    public AraTrail.TrailSpace space;
    [Tooltip("Custom space to use when generating and simulating the trail")]
    public Transform customSpace;
    [Tooltip("Whether to use regular time.")]
    public AraTrail.Timescale timescale;
    [Tooltip("How to align the trail geometry: facing the camera (view) of using the transform's rotation (local).")]
    public AraTrail.TrailAlignment alignment;
    [Tooltip("Determines the order in which trail points will be rendered.")]
    public AraTrail.TrailSorting sorting;
    [Tooltip("Thickness multiplier, in meters.")]
    public float thickness = 0.1f;
    [Range(1f, 8f)]
    [Tooltip("Amount of smoothing iterations applied to the trail shape.")]
    public int smoothness = 1;
    [Min(0.0f)]
    public float smoothingDistance = 0.05f;
    [Tooltip("Calculate accurate thickness at sharp corners.")]
    public bool highQualityCorners;
    [Range(0.0f, 12f)]
    public int cornerRoundness = 5;
    [Tooltip("How should the thickness of the curve evolve over its lenght. The horizontal axis is normalized lenght (in the [0,1] range) and the vertical axis is a thickness multiplier.")]
    [Header("Length")]
    [FormerlySerializedAs("thicknessOverLenght")]
    public AnimationCurve thicknessOverLength = AnimationCurve.Linear(0.0f, 1f, 0.0f, 1f);
    [Tooltip("How should vertex color evolve over the trail's length.")]
    [FormerlySerializedAs("colorOverLenght")]
    public Gradient colorOverLength = new Gradient();
    [Header("Time")]
    [Tooltip("How should the thickness of the curve evolve with its lifetime. The horizontal axis is normalized lifetime (in the [0,1] range) and the vertical axis is a thickness multiplier.")]
    public AnimationCurve thicknessOverTime = AnimationCurve.Linear(0.0f, 1f, 0.0f, 1f);
    [Tooltip("How should vertex color evolve over the trail's lifetime.")]
    public Gradient colorOverTime = new Gradient();
    [Header("Emission")]
    public bool emit = true;
    [Tooltip("Initial thickness of trail points when they are first spawned.")]
    public float initialThickness = 1f;
    [Tooltip("Initial color of trail points when they are first spawned.")]
    public Color initialColor = Color.white;
    [Tooltip("Initial velocity of trail points when they are first spawned.")]
    public Vector3 initialVelocity = Vector3.zero;
    [Tooltip("Minimum amount of time (in seconds) that must pass before spawning a new point.")]
    public float timeInterval = 0.025f;
    [Tooltip("Minimum distance (in meters) that must be left between consecutive points in the trail.")]
    public float minDistance = 0.025f;
    [Tooltip("Duration of the trail (in seconds).")]
    public float time = 2f;
    [Header("Physics")]
    [Tooltip("Toggles trail physics.")]
    public bool enablePhysics;
    [Tooltip("Amount of seconds pre-simulated before the trail appears. Useful when you want a trail to be already simulating when the game starts.")]
    public float warmup;
    [Tooltip("Gravity affecting the trail.")]
    public Vector3 gravity = Vector3.zero;
    [Range(0.0f, 1f)]
    [Tooltip("Amount of speed transferred from the transform to the trail. 0 means no velocity is transferred, 1 means 100% of the velocity is transferred.")]
    public float inertia;
    [Range(0.0f, 1f)]
    [Tooltip("Amount of temporal smoothing applied to the velocity transferred from the transform to the trail.")]
    public float velocitySmoothing = 0.75f;
    [Tooltip("Amount of damping applied to the trail's velocity. Larger values will slow down the trail more as time passes.")]
    [Range(0.0f, 1f)]
    public float damping = 0.75f;
    [Header("Rendering")]
    public Material[] materials = new Material[1];
    public ShadowCastingMode castShadows = ShadowCastingMode.On;
    public bool receiveShadows = true;
    public bool useLightProbes = true;
    [Tooltip("Quad mapping will send the shader an extra coordinate for each vertex, that can be used to correct UV distortion using tex2Dproj.")]
    [Header("Texture")]
    public bool quadMapping;
    [Tooltip("How to apply the texture over the trail: stretch it all over its lenght, or tile it.")]
    public AraTrail.TextureMode textureMode;
    [Tooltip("Defines how many times are U coords repeated across the length of the trail.")]
    public float uvFactor = 1f;
    [Tooltip("Defines how many times are V coords repeated trough the width of the trail.")]
    public float uvWidthFactor = 1f;
    [Tooltip("When the texture mode is set to 'Tile', defines where to begin tiling from: 0 means the start of the trail, 1 means the end.")]
    [Range(0.0f, 1f)]
    public float tileAnchor = 1f;
    [HideInInspector]
    public ElasticArray<AraTrail.Point> points = new ElasticArray<AraTrail.Point>();
    private ElasticArray<AraTrail.Point> renderablePoints = new ElasticArray<AraTrail.Point>();
    private List<int> discontinuities = new List<int>();
    private Mesh mesh_;
    private Vector3 velocity = Vector3.zero;
    private Vector3 prevPosition;
    private float accumTime;
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> normals = new List<Vector3>();
    private List<Vector4> tangents = new List<Vector4>();
    private List<Vector4> uvs = new List<Vector4>();
    private List<Color> vertColors = new List<Color>();
    private List<int> tris = new List<int>();
    private Vector3 nextV = Vector3.zero;
    private Vector3 prevV = Vector3.zero;
    private Vector3 vertex = Vector3.zero;
    private Vector3 normal = Vector3.zero;
    private Vector3 bitangent = Vector3.zero;
    private Vector4 tangent = new Vector4(0.0f, 0.0f, 0.0f, 1f);
    private Vector4 texTangent = Vector4.zero;
    private Vector4 uv = Vector4.zero;
    private Color color;
    private Action<ScriptableRenderContext, Camera> renderCallback;

    public event Action onUpdatePoints;

    public Vector3 Velocity => this.velocity;

    private float DeltaTime
    {
      get
      {
        return this.timescale != AraTrail.Timescale.Unscaled ? Time.deltaTime : Time.unscaledDeltaTime;
      }
    }

    private float FixedDeltaTime
    {
      get
      {
        return this.timescale != AraTrail.Timescale.Unscaled ? Time.fixedDeltaTime : Time.fixedUnscaledDeltaTime;
      }
    }

    public Mesh mesh => this.mesh_;

    public Matrix4x4 worldToTrail
    {
      get
      {
        switch (this.space)
        {
          case AraTrail.TrailSpace.World:
            return Matrix4x4.identity;
          case AraTrail.TrailSpace.Self:
            return this.transform.worldToLocalMatrix;
          case AraTrail.TrailSpace.Custom:
            return !((UnityEngine.Object) this.customSpace != (UnityEngine.Object) null) ? Matrix4x4.identity : this.customSpace.worldToLocalMatrix;
          default:
            return Matrix4x4.identity;
        }
      }
    }

    public void OnValidate()
    {
      this.time = Mathf.Max(this.time, 1E-05f);
      this.warmup = Mathf.Max(0.0f, this.warmup);
    }

    public void Awake() => this.Warmup();

    private void OnEnable()
    {
      this.prevPosition = this.transform.position;
      this.velocity = Vector3.zero;
      this.mesh_ = new Mesh();
      this.mesh_.name = "ara_trail_mesh";
      this.mesh_.MarkDynamic();
      this.AttachToCameraRendering();
    }

    private void OnDisable()
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.mesh_);
      this.DetachFromCameraRendering();
    }

    private void AttachToCameraRendering()
    {
      this.renderCallback = (Action<ScriptableRenderContext, Camera>) ((cntxt, cam) => this.UpdateTrailMesh(cam));
      RenderPipelineManager.beginCameraRendering += this.renderCallback;
      Camera.onPreCull += new Camera.CameraCallback(this.UpdateTrailMesh);
    }

    private void DetachFromCameraRendering()
    {
      RenderPipelineManager.beginCameraRendering -= this.renderCallback;
      Camera.onPreCull -= new Camera.CameraCallback(this.UpdateTrailMesh);
    }

    public void Clear() => this.points.Clear();

    private void UpdateVelocity()
    {
      if ((double) this.DeltaTime > 0.0)
        this.velocity = Vector3.Lerp((this.transform.position - this.prevPosition) / this.DeltaTime, this.velocity, this.velocitySmoothing);
      this.prevPosition = this.transform.position;
    }

    private void LateUpdate()
    {
      this.UpdateVelocity();
      this.EmissionStep(this.DeltaTime);
      this.SnapLastPointToTransform();
      this.UpdatePointsLifecycle();
      if (this.onUpdatePoints == null)
        return;
      this.onUpdatePoints();
    }

    private void EmissionStep(float time)
    {
      this.accumTime += time;
      if ((double) this.accumTime < (double) this.timeInterval || !this.emit)
        return;
      Vector3 vector3 = this.worldToTrail.MultiplyPoint3x4(this.transform.position);
      if (this.points.Count >= 2 && (double) Vector3.Distance(vector3, this.points[this.points.Count - 2].position) < (double) this.minDistance)
        return;
      this.EmitPoint(vector3);
      this.accumTime = 0.0f;
    }

    private void Warmup()
    {
      if (!Application.isPlaying || !this.enablePhysics)
        return;
      for (float warmup = this.warmup; (double) warmup > (double) this.FixedDeltaTime; warmup -= this.FixedDeltaTime)
      {
        this.PhysicsStep(this.FixedDeltaTime);
        this.EmissionStep(this.FixedDeltaTime);
        this.SnapLastPointToTransform();
        this.UpdatePointsLifecycle();
        if (this.onUpdatePoints != null)
          this.onUpdatePoints();
      }
    }

    private void PhysicsStep(float timestep)
    {
      float num = Mathf.Pow(1f - Mathf.Clamp01(this.damping), timestep);
      for (int index = 0; index < this.points.Count; ++index)
      {
        AraTrail.Point point = this.points[index];
        point.velocity += this.gravity * timestep;
        point.velocity *= num;
        point.position += point.velocity * timestep;
        this.points[index] = point;
      }
    }

    private void FixedUpdate()
    {
      if (!this.enablePhysics)
        return;
      this.PhysicsStep(this.FixedDeltaTime);
    }

    public void EmitPoint(Vector3 position, bool adjustEnd = true)
    {
      float texcoord = 0.0f;
      if (this.points.Count > 0)
        texcoord = this.points[this.points.Count - 1].texcoord + Vector3.Distance(position, this.points[this.points.Count - 1].position);
      Vector3 normal = this.worldToTrail.MultiplyVector(this.transform.forward);
      Vector3 tangent = this.worldToTrail.MultiplyVector(this.transform.right);
      this.points.Add(new AraTrail.Point(position, this.initialVelocity + this.velocity * this.inertia, tangent, normal, this.initialColor, this.initialThickness, texcoord, this.time));
    }

    private void SnapLastPointToTransform()
    {
      if (this.points.Count <= 0)
        return;
      AraTrail.Point point = this.points[this.points.Count - 1];
      if (!this.emit)
        point.discontinuous = true;
      if (!point.discontinuous)
      {
        ref AraTrail.Point local1 = ref point;
        Matrix4x4 worldToTrail = this.worldToTrail;
        Vector3 vector3_1 = worldToTrail.MultiplyPoint3x4(this.transform.position);
        local1.position = vector3_1;
        ref AraTrail.Point local2 = ref point;
        worldToTrail = this.worldToTrail;
        Vector3 vector3_2 = worldToTrail.MultiplyVector(this.transform.forward);
        local2.normal = vector3_2;
        ref AraTrail.Point local3 = ref point;
        worldToTrail = this.worldToTrail;
        Vector3 vector3_3 = worldToTrail.MultiplyVector(this.transform.right);
        local3.tangent = vector3_3;
        if (this.points.Count > 1)
          point.texcoord = this.points[this.points.Count - 2].texcoord + Vector3.Distance(point.position, this.points[this.points.Count - 2].position);
      }
      this.points[this.points.Count - 1] = point;
    }

    private void UpdatePointsLifecycle()
    {
      for (int index = this.points.Count - 1; index >= 0; --index)
      {
        AraTrail.Point point = this.points[index];
        point.life -= this.DeltaTime;
        this.points[index] = point;
        if ((double) point.life <= 0.0)
        {
          if (this.smoothness <= 1)
            this.points.RemoveAt(index);
          else if ((double) this.points[Mathf.Min(index + 1, this.points.Count - 1)].life <= 0.0 && (double) this.points[Mathf.Min(index + 2, this.points.Count - 1)].life <= 0.0)
            this.points.RemoveAt(index);
        }
      }
    }

    private void ClearMeshData()
    {
      this.mesh_.Clear();
      this.vertices.Clear();
      this.normals.Clear();
      this.tangents.Clear();
      this.uvs.Clear();
      this.vertColors.Clear();
      this.tris.Clear();
    }

    private void CommitMeshData()
    {
      this.mesh_.SetVertices(this.vertices);
      this.mesh_.SetNormals(this.normals);
      this.mesh_.SetTangents(this.tangents);
      this.mesh_.SetColors(this.vertColors);
      this.mesh_.SetUVs(0, this.uvs);
      this.mesh_.SetTriangles(this.tris, 0, true);
    }

    private void RenderMesh(Camera cam)
    {
      Matrix4x4 inverse = this.worldToTrail.inverse;
      for (int index = 0; index < this.materials.Length; ++index)
        Graphics.DrawMesh(this.mesh_, inverse, this.materials[index], this.gameObject.layer, cam, 0, (MaterialPropertyBlock) null, this.castShadows, this.receiveShadows, (Transform) null, this.useLightProbes);
    }

    private ElasticArray<AraTrail.Point> GetRenderablePoints(int start, int end)
    {
      this.renderablePoints.Clear();
      if (this.smoothness <= 1)
      {
        for (int index = start; index <= end; ++index)
          this.renderablePoints.Add(this.points[index]);
        return this.renderablePoints;
      }
      AraTrail.Point[] data = this.points.Data;
      float num1 = 1f / (float) this.smoothness;
      AraTrail.Point point = new AraTrail.Point(Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Color.white, 0.0f, 0.0f, 0.0f);
      for (int index1 = start; index1 < end; ++index1)
      {
        int index2 = index1 == start ? start : index1 - 1;
        int index3 = index1 == end - 1 ? end : index1 + 2;
        int index4 = index1 + 1;
        float p0_1 = data[index2].position[0];
        float p0_2 = data[index2].position[1];
        float p0_3 = data[index2].position[2];
        float p0_4 = data[index2].velocity[0];
        float p0_5 = data[index2].velocity[1];
        float p0_6 = data[index2].velocity[2];
        float p0_7 = data[index2].tangent[0];
        float p0_8 = data[index2].tangent[1];
        float p0_9 = data[index2].tangent[2];
        float p0_10 = data[index2].normal[0];
        float p0_11 = data[index2].normal[1];
        float p0_12 = data[index2].normal[2];
        float p0_13 = data[index2].color[0];
        float p0_14 = data[index2].color[1];
        float p0_15 = data[index2].color[2];
        float p0_16 = data[index2].color[3];
        float p1_1 = data[index1].position[0];
        float p1_2 = data[index1].position[1];
        float p1_3 = data[index1].position[2];
        float p1_4 = data[index1].velocity[0];
        float p1_5 = data[index1].velocity[1];
        float p1_6 = data[index1].velocity[2];
        float p1_7 = data[index1].tangent[0];
        float p1_8 = data[index1].tangent[1];
        float p1_9 = data[index1].tangent[2];
        float p1_10 = data[index1].normal[0];
        float p1_11 = data[index1].normal[1];
        float p1_12 = data[index1].normal[2];
        float p1_13 = data[index1].color[0];
        float p1_14 = data[index1].color[1];
        float p1_15 = data[index1].color[2];
        float p1_16 = data[index1].color[3];
        float p2_1 = data[index4].position[0];
        float p2_2 = data[index4].position[1];
        float p2_3 = data[index4].position[2];
        float p2_4 = data[index4].velocity[0];
        float p2_5 = data[index4].velocity[1];
        float p2_6 = data[index4].velocity[2];
        float p2_7 = data[index4].tangent[0];
        float p2_8 = data[index4].tangent[1];
        float p2_9 = data[index4].tangent[2];
        float p2_10 = data[index4].normal[0];
        float p2_11 = data[index4].normal[1];
        float p2_12 = data[index4].normal[2];
        float p2_13 = data[index4].color[0];
        float p2_14 = data[index4].color[1];
        float p2_15 = data[index4].color[2];
        float p2_16 = data[index4].color[3];
        float p3_1 = data[index3].position[0];
        float p3_2 = data[index3].position[1];
        float p3_3 = data[index3].position[2];
        float p3_4 = data[index3].velocity[0];
        float p3_5 = data[index3].velocity[1];
        float p3_6 = data[index3].velocity[2];
        float p3_7 = data[index3].tangent[0];
        float p3_8 = data[index3].tangent[1];
        float p3_9 = data[index3].tangent[2];
        float p3_10 = data[index3].normal[0];
        float p3_11 = data[index3].normal[1];
        float p3_12 = data[index3].normal[2];
        float p3_13 = data[index3].color[0];
        float p3_14 = data[index3].color[1];
        float p3_15 = data[index3].color[2];
        float p3_16 = data[index3].color[3];
        for (int index5 = 0; index5 < this.smoothness; ++index5)
        {
          float t = (float) index5 * num1;
          point.life = float.IsInfinity(data[index2].life) || float.IsInfinity(data[index1].life) || float.IsInfinity(data[index4].life) || float.IsInfinity(data[index3].life) ? float.PositiveInfinity : AraTrail.Point.CatmullRom(data[index2].life, data[index1].life, data[index4].life, data[index3].life, t);
          double num2 = (double) p2_1 - (double) p1_1;
          float num3 = p2_2 - p1_2;
          float num4 = p2_3 - p1_3;
          if (num2 * num2 + (double) num3 * (double) num3 + (double) num4 * (double) num4 < (double) this.smoothingDistance * (double) this.smoothingDistance)
          {
            this.renderablePoints.Add(data[index1]);
            break;
          }
          if ((double) point.life > 0.0)
          {
            point.position.x = AraTrail.Point.CatmullRom(p0_1, p1_1, p2_1, p3_1, t);
            point.position.y = AraTrail.Point.CatmullRom(p0_2, p1_2, p2_2, p3_2, t);
            point.position.z = AraTrail.Point.CatmullRom(p0_3, p1_3, p2_3, p3_3, t);
            point.velocity.x = AraTrail.Point.CatmullRom(p0_4, p1_4, p2_4, p3_4, t);
            point.velocity.y = AraTrail.Point.CatmullRom(p0_5, p1_5, p2_5, p3_5, t);
            point.velocity.z = AraTrail.Point.CatmullRom(p0_6, p1_6, p2_6, p3_6, t);
            point.tangent.x = AraTrail.Point.CatmullRom(p0_7, p1_7, p2_7, p3_7, t);
            point.tangent.y = AraTrail.Point.CatmullRom(p0_8, p1_8, p2_8, p3_8, t);
            point.tangent.z = AraTrail.Point.CatmullRom(p0_9, p1_9, p2_9, p3_9, t);
            point.normal.x = AraTrail.Point.CatmullRom(p0_10, p1_10, p2_10, p3_10, t);
            point.normal.y = AraTrail.Point.CatmullRom(p0_11, p1_11, p2_11, p3_11, t);
            point.normal.z = AraTrail.Point.CatmullRom(p0_12, p1_12, p2_12, p3_12, t);
            point.color.r = AraTrail.Point.CatmullRom(p0_13, p1_13, p2_13, p3_13, t);
            point.color.g = AraTrail.Point.CatmullRom(p0_14, p1_14, p2_14, p3_14, t);
            point.color.b = AraTrail.Point.CatmullRom(p0_15, p1_15, p2_15, p3_15, t);
            point.color.a = AraTrail.Point.CatmullRom(p0_16, p1_16, p2_16, p3_16, t);
            point.thickness = AraTrail.Point.CatmullRom(data[index2].thickness, data[index1].thickness, data[index4].thickness, data[index3].thickness, t);
            point.texcoord = AraTrail.Point.CatmullRom(data[index2].texcoord, data[index1].texcoord, data[index4].texcoord, data[index3].texcoord, t);
            this.renderablePoints.Add(point);
          }
        }
      }
      if ((double) this.points[end].life > 0.0)
        this.renderablePoints.Add(this.points[end]);
      return this.renderablePoints;
    }

    private AraTrail.CurveFrame InitializeCurveFrame(Vector3 point, Vector3 nextPoint)
    {
      Vector3 tangent = nextPoint - point;
      if (Mathf.Approximately(Mathf.Abs(Vector3.Dot(tangent.normalized, this.transform.forward)), 1f))
        tangent += this.transform.right * 0.01f;
      return new AraTrail.CurveFrame(point, this.transform.forward, this.transform.up, tangent);
    }

    private void UpdateTrailMesh(Camera cam)
    {
      if ((cam.cullingMask & 1 << this.gameObject.layer) == 0)
        return;
      this.ClearMeshData();
      if (this.points.Count <= 1)
        return;
      Vector3 localCamPosition = this.worldToTrail.MultiplyPoint3x4(cam.transform.position);
      this.discontinuities.Clear();
      for (int index = 0; index < this.points.Count; ++index)
      {
        if (this.points[index].discontinuous || index == this.points.Count - 1)
          this.discontinuities.Add(index);
      }
      int start = 0;
      for (int index = 0; index < this.discontinuities.Count; ++index)
      {
        this.UpdateSegmentMesh(start, this.discontinuities[index], localCamPosition);
        start = this.discontinuities[index] + 1;
      }
      this.CommitMeshData();
      this.RenderMesh(cam);
    }

    private void UpdateSegmentMesh(int start, int end, Vector3 localCamPosition)
    {
      ElasticArray<AraTrail.Point> renderablePoints = this.GetRenderablePoints(start, end);
      if (this.sorting == AraTrail.TrailSorting.NewerOnTop)
        renderablePoints.Reverse();
      AraTrail.Point[] data = renderablePoints.Data;
      if (renderablePoints.Count <= 1)
        return;
      float a = 0.0f;
      for (int index = 0; index < renderablePoints.Count - 1; ++index)
        a += Vector3.Distance(data[index].position, data[index + 1].position);
      float num1 = Mathf.Max(a, 1E-05f);
      float num2 = 0.0f;
      float vCoord = this.textureMode == AraTrail.TextureMode.Stretch ? 0.0f : -this.uvFactor * num1 * this.tileAnchor;
      if (this.sorting == AraTrail.TrailSorting.NewerOnTop)
        vCoord = 1f - vCoord;
      AraTrail.CurveFrame frame = this.InitializeCurveFrame(data[renderablePoints.Count - 1].position, data[renderablePoints.Count - 2].position);
      int va = 1;
      int vb = 0;
      for (int i = renderablePoints.Count - 1; i >= 0; --i)
      {
        int index1 = Mathf.Max(i - 1, 0);
        int index2 = Mathf.Min(i + 1, renderablePoints.Count - 1);
        this.nextV.x = data[index1].position.x - data[i].position.x;
        this.nextV.y = data[index1].position.y - data[i].position.y;
        this.nextV.z = data[index1].position.z - data[i].position.z;
        this.prevV.x = data[i].position.x - data[index2].position.x;
        this.prevV.y = data[i].position.y - data[index2].position.y;
        this.prevV.z = data[i].position.z - data[index2].position.z;
        float num3 = index1 == i ? this.prevV.magnitude : this.nextV.magnitude;
        this.nextV.Normalize();
        this.prevV.Normalize();
        if (this.alignment == AraTrail.TrailAlignment.Local)
        {
          this.tangent = (Vector4) data[i].tangent.normalized;
        }
        else
        {
          this.tangent.x = (float) (((double) this.nextV.x + (double) this.prevV.x) * 0.5);
          this.tangent.y = (float) (((double) this.nextV.y + (double) this.prevV.y) * 0.5);
          this.tangent.z = (float) (((double) this.nextV.z + (double) this.prevV.z) * 0.5);
        }
        this.normal = data[i].normal;
        if (this.alignment != AraTrail.TrailAlignment.Local)
          this.normal = this.alignment == AraTrail.TrailAlignment.View ? localCamPosition - data[i].position : frame.Transport((Vector3) this.tangent, data[i].position);
        this.normal.Normalize();
        if (this.alignment == AraTrail.TrailAlignment.Velocity)
        {
          this.bitangent = frame.bitangent;
        }
        else
        {
          this.bitangent.x = (float) ((double) this.tangent.y * (double) this.normal.z - (double) this.tangent.z * (double) this.normal.y);
          this.bitangent.y = (float) ((double) this.tangent.z * (double) this.normal.x - (double) this.tangent.x * (double) this.normal.z);
          this.bitangent.z = (float) ((double) this.tangent.x * (double) this.normal.y - (double) this.tangent.y * (double) this.normal.x);
        }
        this.bitangent.Normalize();
        float time1 = this.sorting == AraTrail.TrailSorting.OlderOnTop ? num2 / num1 : (num1 - num2) / num1;
        float time2 = float.IsInfinity(this.time) ? 1f : Mathf.Clamp01((float) (1.0 - (double) data[i].life / (double) this.time));
        num2 += num3;
        this.color = data[i].color * this.colorOverTime.Evaluate(time2) * this.colorOverLength.Evaluate(time1);
        float sectionThickness = this.thickness * data[i].thickness * this.thicknessOverTime.Evaluate(time2) * this.thicknessOverLength.Evaluate(time1);
        if (this.textureMode == AraTrail.TextureMode.WorldTile)
          vCoord = this.tileAnchor + data[i].texcoord * this.uvFactor;
        if ((UnityEngine.Object) this.section != (UnityEngine.Object) null)
          this.AppendSection(data, ref frame, i, renderablePoints.Count, sectionThickness, vCoord);
        else
          this.AppendFlatTrail(data, ref frame, i, renderablePoints.Count, sectionThickness, vCoord, ref va, ref vb);
        float num4 = this.textureMode == AraTrail.TextureMode.Stretch ? num3 / num1 : num3;
        vCoord += this.uvFactor * (this.sorting == AraTrail.TrailSorting.NewerOnTop ? -num4 : num4);
      }
    }

    private void AppendSection(
      AraTrail.Point[] data,
      ref AraTrail.CurveFrame frame,
      int i,
      int count,
      float sectionThickness,
      float vCoord)
    {
      int segments = this.section.Segments;
      int num = segments + 1;
      int count1 = this.vertices.Count;
      for (int index = 0; index <= segments; ++index)
      {
        this.normal.x = (float) ((double) this.section.vertices[index].x * (double) this.bitangent.x + (double) this.section.vertices[index].y * (double) this.tangent.x) * sectionThickness;
        this.normal.y = (float) ((double) this.section.vertices[index].x * (double) this.bitangent.y + (double) this.section.vertices[index].y * (double) this.tangent.y) * sectionThickness;
        this.normal.z = (float) ((double) this.section.vertices[index].x * (double) this.bitangent.z + (double) this.section.vertices[index].y * (double) this.tangent.z) * sectionThickness;
        this.vertex.x = data[i].position.x + this.normal.x;
        this.vertex.y = data[i].position.y + this.normal.y;
        this.vertex.z = data[i].position.z + this.normal.z;
        this.texTangent.x = (float) -((double) this.normal.y * (double) frame.tangent.z - (double) this.normal.z * (double) frame.tangent.y);
        this.texTangent.y = (float) -((double) this.normal.z * (double) frame.tangent.x - (double) this.normal.x * (double) frame.tangent.z);
        this.texTangent.z = (float) -((double) this.normal.x * (double) frame.tangent.y - (double) this.normal.y * (double) frame.tangent.x);
        this.texTangent.w = 1f;
        this.uv.x = (float) index / (float) segments * this.uvWidthFactor;
        this.uv.y = vCoord;
        this.uv.z = 0.0f;
        this.uv.w = 1f;
        this.vertices.Add(this.vertex);
        this.normals.Add(this.normal);
        this.tangents.Add(this.texTangent);
        this.uvs.Add(this.uv);
        this.vertColors.Add(this.color);
        if (index < segments && i < count - 1)
        {
          this.tris.Add(count1 + index);
          this.tris.Add(count1 + (index + 1));
          this.tris.Add(count1 - num + index);
          this.tris.Add(count1 + (index + 1));
          this.tris.Add(count1 - num + (index + 1));
          this.tris.Add(count1 - num + index);
        }
      }
    }

    private void AppendFlatTrail(
      AraTrail.Point[] data,
      ref AraTrail.CurveFrame frame,
      int i,
      int count,
      float sectionThickness,
      float vCoord,
      ref int va,
      ref int vb)
    {
      int num1 = !this.highQualityCorners ? 0 : (this.alignment != AraTrail.TrailAlignment.Local ? 1 : 0);
      Quaternion quaternion = Quaternion.identity;
      Vector3 vector3_1 = Vector3.zero;
      float num2 = 0.0f;
      float num3 = sectionThickness;
      Vector3 rhs = this.bitangent;
      if (num1 != 0)
      {
        Vector3 vector3_2;
        Vector3 vector3_3;
        if (i != 0)
        {
          vector3_2 = Vector3.Cross(this.nextV, Vector3.Cross(this.bitangent, (Vector3) this.tangent));
          vector3_3 = vector3_2.normalized;
        }
        else
          vector3_3 = this.bitangent;
        Vector3 vector3_4 = vector3_3;
        if (this.cornerRoundness > 0)
        {
          Vector3 vector3_5;
          if (i != count - 1)
          {
            vector3_2 = Vector3.Cross(this.prevV, Vector3.Cross(this.bitangent, (Vector3) this.tangent));
            vector3_5 = vector3_2.normalized;
          }
          else
            vector3_5 = -this.bitangent;
          rhs = vector3_5;
          num2 = i == 0 || i == count - 1 ? 1f : Mathf.Sign(Vector3.Dot(this.nextV, -rhs));
          quaternion = Quaternion.AngleAxis(57.29578f * (i == 0 || i == count - 1 ? 3.14159274f : Mathf.Acos(Mathf.Clamp(Vector3.Dot(vector3_4, rhs), -1f, 1f))) / (float) this.cornerRoundness, this.normal * num2);
          vector3_1 = rhs * sectionThickness * num2;
        }
        if ((double) vector3_4.sqrMagnitude > 0.10000000149011612)
          num3 = sectionThickness / Mathf.Max(Vector3.Dot(this.bitangent, vector3_4), 0.15f);
      }
      if (num1 != 0 && this.cornerRoundness > 0)
      {
        if ((double) num2 > 0.0)
        {
          this.vertices.Add(data[i].position + rhs * sectionThickness);
          this.vertices.Add(data[i].position - this.bitangent * num3);
        }
        else
        {
          this.vertices.Add(data[i].position + this.bitangent * num3);
          this.vertices.Add(data[i].position - rhs * sectionThickness);
        }
      }
      else
      {
        this.vertices.Add(data[i].position + this.bitangent * num3);
        this.vertices.Add(data[i].position - this.bitangent * num3);
      }
      this.normals.Add(this.normal);
      this.normals.Add(this.normal);
      this.tangents.Add(this.tangent);
      this.tangents.Add(this.tangent);
      this.vertColors.Add(this.color);
      this.vertColors.Add(this.color);
      if (this.quadMapping)
      {
        this.uv.Set(vCoord * sectionThickness, this.sorting == AraTrail.TrailSorting.NewerOnTop ? this.uvWidthFactor * sectionThickness : 0.0f, 0.0f, sectionThickness);
        this.uvs.Add(this.uv);
        this.uv.Set(vCoord * sectionThickness, this.sorting == AraTrail.TrailSorting.NewerOnTop ? 0.0f : this.uvWidthFactor * sectionThickness, 0.0f, sectionThickness);
        this.uvs.Add(this.uv);
      }
      else
      {
        this.uv.Set(vCoord, this.sorting == AraTrail.TrailSorting.NewerOnTop ? this.uvWidthFactor : 0.0f, 0.0f, 1f);
        this.uvs.Add(this.uv);
        this.uv.Set(vCoord, this.sorting == AraTrail.TrailSorting.NewerOnTop ? 0.0f : this.uvWidthFactor, 0.0f, 1f);
        this.uvs.Add(this.uv);
      }
      if (i < count - 1)
      {
        int num4 = this.vertices.Count - 1;
        this.tris.Add(num4);
        this.tris.Add(va);
        this.tris.Add(vb);
        this.tris.Add(vb);
        this.tris.Add(num4 - 1);
        this.tris.Add(num4);
      }
      va = this.vertices.Count - 1;
      vb = this.vertices.Count - 2;
      if (num1 == 0 || this.cornerRoundness <= 0)
        return;
      for (int index = 0; index <= this.cornerRoundness; ++index)
      {
        this.vertices.Add(data[i].position + vector3_1);
        this.normals.Add(this.normal);
        this.tangents.Add(this.tangent);
        this.vertColors.Add(this.color);
        this.uv.Set(vCoord, (double) num2 > 0.0 ? 0.0f : 1f, 0.0f, 1f);
        this.uvs.Add(this.uv);
        int num5 = this.vertices.Count - 1;
        this.tris.Add(num5);
        this.tris.Add(va);
        this.tris.Add(vb);
        if ((double) num2 > 0.0)
          vb = num5;
        else
          va = num5;
        vector3_1 = quaternion * vector3_1;
      }
    }

    public enum TrailAlignment
    {
      View,
      Velocity,
      Local,
    }

    public enum TrailSpace
    {
      World,
      Self,
      Custom,
    }

    public enum TrailSorting
    {
      OlderOnTop,
      NewerOnTop,
    }

    public enum Timescale
    {
      Normal,
      Unscaled,
    }

    public enum TextureMode
    {
      Stretch,
      Tile,
      WorldTile,
    }

    public struct CurveFrame
    {
      public Vector3 position;
      public Vector3 normal;
      public Vector3 bitangent;
      public Vector3 tangent;

      public CurveFrame(Vector3 position, Vector3 normal, Vector3 bitangent, Vector3 tangent)
      {
        this.position = position;
        this.normal = normal;
        this.bitangent = bitangent;
        this.tangent = tangent;
      }

      public Vector3 Transport(Vector3 newTangent, Vector3 newPosition)
      {
        Vector3 vector3_1 = newPosition - this.position;
        float num1 = Vector3.Dot(vector3_1, vector3_1);
        Vector3 rhs1 = this.normal - (float) (2.0 / ((double) num1 + 9.9999997473787516E-06)) * Vector3.Dot(vector3_1, this.normal) * vector3_1;
        Vector3 vector3_2 = this.tangent - (float) (2.0 / ((double) num1 + 9.9999997473787516E-06)) * Vector3.Dot(vector3_1, this.tangent) * vector3_1;
        Vector3 vector3_3 = newTangent - vector3_2;
        float num2 = Vector3.Dot(vector3_3, vector3_3);
        Vector3 rhs2 = rhs1 - (float) (2.0 / ((double) num2 + 9.9999997473787516E-06)) * Vector3.Dot(vector3_3, rhs1) * vector3_3;
        Vector3 vector3_4 = Vector3.Cross(newTangent, rhs2);
        this.normal = rhs2;
        this.bitangent = vector3_4;
        this.tangent = newTangent;
        this.position = newPosition;
        return this.normal;
      }
    }

    public struct Point
    {
      public Vector3 position;
      public Vector3 velocity;
      public Vector3 tangent;
      public Vector3 normal;
      public Color color;
      public float thickness;
      public float life;
      public float texcoord;
      public bool discontinuous;

      public Point(
        Vector3 position,
        Vector3 velocity,
        Vector3 tangent,
        Vector3 normal,
        Color color,
        float thickness,
        float texcoord,
        float lifetime)
      {
        this.position = position;
        this.velocity = velocity;
        this.tangent = tangent;
        this.normal = normal;
        this.color = color;
        this.thickness = thickness;
        this.life = lifetime;
        this.texcoord = texcoord;
        this.discontinuous = false;
      }

      public static float CatmullRom(float p0, float p1, float p2, float p3, float t)
      {
        float num = t * t;
        return (float) (0.5 * (2.0 * (double) p1 + (-(double) p0 + (double) p2) * (double) t + (2.0 * (double) p0 - 5.0 * (double) p1 + 4.0 * (double) p2 - (double) p3) * (double) num + (-(double) p0 + 3.0 * (double) p1 - 3.0 * (double) p2 + (double) p3) * (double) num * (double) t));
      }

      public static AraTrail.Point operator +(AraTrail.Point p1, AraTrail.Point p2)
      {
        return new AraTrail.Point(p1.position + p2.position, p1.velocity + p2.velocity, p1.tangent + p2.tangent, p1.normal + p2.normal, p1.color + p2.color, p1.thickness + p2.thickness, p1.texcoord + p2.texcoord, p1.life + p2.life);
      }

      public static AraTrail.Point operator -(AraTrail.Point p1, AraTrail.Point p2)
      {
        return new AraTrail.Point(p1.position - p2.position, p1.velocity - p2.velocity, p1.tangent - p2.tangent, p1.normal - p2.normal, p1.color - p2.color, p1.thickness - p2.thickness, p1.texcoord - p2.texcoord, p1.life - p2.life);
      }
    }
  }
}
