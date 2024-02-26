// Decompiled with JetBrains decompiler
// Type: PSMeshRendererUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class PSMeshRendererUpdater : MonoBehaviour
{
  public GameObject MeshObject;
  public float StartScaleMultiplier = 1f;
  public Color Color = Color.black;
  private const string materialName = "MeshEffect";
  private List<Material[]> rendererMaterials = new List<Material[]>();
  private List<Material[]> skinnedMaterials = new List<Material[]>();
  public bool IsActive = true;
  public float FadeTime = 1.5f;
  private bool currentActiveStatus;
  private bool needUpdateAlpha;
  private Color oldColor = Color.black;
  private float currentAlphaTime;
  private string[] colorProperties = new string[9]
  {
    "_TintColor",
    "_Color",
    "_EmissionColor",
    "_BorderColor",
    "_ReflectColor",
    "_RimColor",
    "_MainColor",
    "_CoreColor",
    "_FresnelColor"
  };
  private float alpha;
  private float prevAlpha;
  private Dictionary<string, float> startAlphaColors;
  private bool previousActiveStatus;
  private bool needUpdate;
  private bool needLastUpdate;
  private Dictionary<ParticleSystem, PSMeshRendererUpdater.ParticleStartInfo> startParticleParameters;

  private void OnEnable()
  {
    this.alpha = 0.0f;
    this.prevAlpha = 0.0f;
    this.IsActive = true;
  }

  private void Update()
  {
    if (!Application.isPlaying)
      return;
    if (this.startAlphaColors == null)
      this.InitStartAlphaColors();
    if (this.IsActive && (double) this.alpha < 1.0)
      this.alpha += Time.deltaTime / this.FadeTime;
    if (!this.IsActive && (double) this.alpha > 0.0)
      this.alpha -= Time.deltaTime / this.FadeTime;
    if ((double) this.alpha > 0.0 && (double) this.alpha < 1.0)
    {
      this.needUpdate = true;
    }
    else
    {
      this.needUpdate = false;
      this.alpha = Mathf.Clamp01(this.alpha);
      if ((double) Mathf.Abs(this.prevAlpha - this.alpha) >= (double) Mathf.Epsilon)
        this.UpdateVisibleStatus();
    }
    this.prevAlpha = this.alpha;
    if (this.needUpdate)
      this.UpdateVisibleStatus();
    if (!(this.Color != this.oldColor))
      return;
    this.oldColor = this.Color;
    this.UpdateColor(this.Color);
  }

  private void InitStartAlphaColors()
  {
    this.startAlphaColors = new Dictionary<string, float>();
    foreach (Renderer componentsInChild in this.GetComponentsInChildren<Renderer>(true))
    {
      Material[] materials = componentsInChild.materials;
      for (int materialNumber = 0; materialNumber < materials.Length; ++materialNumber)
      {
        if (materials[materialNumber].name.Contains("MeshEffect"))
          this.GetStartAlphaByProperties(componentsInChild.GetHashCode().ToString(), materialNumber, materials[materialNumber]);
      }
    }
    foreach (SkinnedMeshRenderer componentsInChild in this.GetComponentsInChildren<SkinnedMeshRenderer>(true))
    {
      Material[] materials = componentsInChild.materials;
      for (int materialNumber = 0; materialNumber < materials.Length; ++materialNumber)
      {
        if (materials[materialNumber].name.Contains("MeshEffect"))
          this.GetStartAlphaByProperties(componentsInChild.GetHashCode().ToString(), materialNumber, materials[materialNumber]);
      }
    }
    Light[] componentsInChildren = this.GetComponentsInChildren<Light>(true);
    for (int index = 0; index < componentsInChildren.Length; ++index)
    {
      ME_LightCurves component = componentsInChildren[index].GetComponent<ME_LightCurves>();
      float num = 1f;
      if ((Object) component != (Object) null)
        num = component.GraphIntensityMultiplier;
      this.startAlphaColors.Add(componentsInChildren[index].GetHashCode().ToString() + index.ToString(), num);
    }
    foreach (Renderer componentsInChild in this.MeshObject.GetComponentsInChildren<Renderer>(true))
    {
      Material[] materials = componentsInChild.materials;
      for (int materialNumber = 0; materialNumber < materials.Length; ++materialNumber)
      {
        if (materials[materialNumber].name.Contains("MeshEffect"))
          this.GetStartAlphaByProperties(componentsInChild.GetHashCode().ToString(), materialNumber, materials[materialNumber]);
      }
    }
    foreach (SkinnedMeshRenderer componentsInChild in this.MeshObject.GetComponentsInChildren<SkinnedMeshRenderer>(true))
    {
      Material[] materials = componentsInChild.materials;
      for (int materialNumber = 0; materialNumber < materials.Length; ++materialNumber)
      {
        if (materials[materialNumber].name.Contains("MeshEffect"))
          this.GetStartAlphaByProperties(componentsInChild.GetHashCode().ToString(), materialNumber, materials[materialNumber]);
      }
    }
  }

  private void InitStartParticleParameters()
  {
    this.startParticleParameters = new Dictionary<ParticleSystem, PSMeshRendererUpdater.ParticleStartInfo>();
    foreach (ParticleSystem componentsInChild in this.GetComponentsInChildren<ParticleSystem>(true))
      this.startParticleParameters.Add(componentsInChild, new PSMeshRendererUpdater.ParticleStartInfo()
      {
        StartSize = componentsInChild.main.startSize,
        StartSpeed = componentsInChild.main.startSpeed
      });
  }

  private void UpdateVisibleStatus()
  {
    foreach (Renderer componentsInChild in this.GetComponentsInChildren<Renderer>(true))
    {
      Material[] materials = componentsInChild.materials;
      for (int materialNumber = 0; materialNumber < materials.Length; ++materialNumber)
      {
        if (materials[materialNumber].name.Contains("MeshEffect"))
          this.UpdateAlphaByProperties(componentsInChild.GetHashCode().ToString(), materialNumber, materials[materialNumber], this.alpha);
      }
    }
    foreach (Renderer componentsInChild in this.GetComponentsInChildren<Renderer>(true))
    {
      Material[] materials = componentsInChild.materials;
      for (int materialNumber = 0; materialNumber < materials.Length; ++materialNumber)
      {
        if (materials[materialNumber].name.Contains("MeshEffect"))
          this.UpdateAlphaByProperties(componentsInChild.GetHashCode().ToString(), materialNumber, materials[materialNumber], this.alpha);
      }
    }
    foreach (Renderer componentsInChild in this.MeshObject.GetComponentsInChildren<Renderer>(true))
    {
      Material[] materials = componentsInChild.materials;
      for (int materialNumber = 0; materialNumber < materials.Length; ++materialNumber)
      {
        if (materials[materialNumber].name.Contains("MeshEffect"))
          this.UpdateAlphaByProperties(componentsInChild.GetHashCode().ToString(), materialNumber, materials[materialNumber], this.alpha);
      }
    }
    foreach (Renderer componentsInChild in this.MeshObject.GetComponentsInChildren<Renderer>(true))
    {
      Material[] materials = componentsInChild.materials;
      for (int materialNumber = 0; materialNumber < materials.Length; ++materialNumber)
      {
        if (materials[materialNumber].name.Contains("MeshEffect"))
          this.UpdateAlphaByProperties(componentsInChild.GetHashCode().ToString(), materialNumber, materials[materialNumber], this.alpha);
      }
    }
    foreach (Behaviour componentsInChild in this.GetComponentsInChildren<ME_LightCurves>(true))
      componentsInChild.enabled = this.IsActive;
    Light[] componentsInChildren = this.GetComponentsInChildren<Light>(true);
    for (int index = 0; index < componentsInChildren.Length; ++index)
    {
      if (!this.IsActive)
      {
        float startAlphaColor = this.startAlphaColors[componentsInChildren[index].GetHashCode().ToString() + index.ToString()];
        componentsInChildren[index].intensity = this.alpha * startAlphaColor;
      }
    }
    foreach (ParticleSystem componentsInChild in this.GetComponentsInChildren<ParticleSystem>(true))
    {
      if (!this.IsActive && !componentsInChild.isStopped)
        componentsInChild.Stop();
      if (this.IsActive && componentsInChild.isStopped)
        componentsInChild.Play();
    }
    foreach (ME_TrailRendererNoise componentsInChild in this.GetComponentsInChildren<ME_TrailRendererNoise>())
      componentsInChild.IsActive = this.IsActive;
  }

  private void UpdateAlphaByProperties(string rendName, int materialNumber, Material mat, float alpha)
  {
    string[] array = colorProperties;
    foreach (string prop in array)
    {
      if (mat.HasProperty(prop))
      {
        float startAlpha = startAlphaColors[rendName + materialNumber + prop.ToString()];
        Color color = mat.GetColor(prop);
        color.a = alpha * startAlpha;
        mat.SetColor(prop, color);
      }
    }
  }

  private void GetStartAlphaByProperties(string rendName, int materialNumber, Material mat)
  {
    foreach (string colorProperty in this.colorProperties)
    {
      if (mat.HasProperty(colorProperty) && !this.startAlphaColors.ContainsKey(rendName + materialNumber.ToString() + colorProperty.ToString()))
        this.startAlphaColors.Add(rendName + materialNumber.ToString() + colorProperty.ToString(), mat.GetColor(colorProperty).a);
    }
  }

  public void UpdateColor(Color color)
  {
    if ((Object) this.MeshObject == (Object) null)
      return;
    ME_ColorHelper.ChangeObjectColorByHUE(this.MeshObject, ME_ColorHelper.ColorToHSV(color).H);
  }

  public void UpdateColor(float HUE)
  {
    if ((Object) this.MeshObject == (Object) null)
      return;
    ME_ColorHelper.ChangeObjectColorByHUE(this.MeshObject, HUE);
  }

  public void UpdateMeshEffect()
  {
    this.transform.localPosition = Vector3.zero;
    this.transform.localRotation = new Quaternion();
    this.rendererMaterials.Clear();
    this.skinnedMaterials.Clear();
    if ((Object) this.MeshObject == (Object) null)
      return;
    this.UpdatePSMesh(this.MeshObject);
    this.AddMaterialToMesh(this.MeshObject);
  }

  private void CheckScaleIncludedParticles()
  {
  }

  public void UpdateMeshEffect(GameObject go)
  {
    this.rendererMaterials.Clear();
    this.skinnedMaterials.Clear();
    if ((Object) go == (Object) null)
    {
      Debug.Log((object) "You need set a gameObject");
    }
    else
    {
      this.MeshObject = go;
      this.UpdatePSMesh(this.MeshObject);
      this.AddMaterialToMesh(this.MeshObject);
    }
  }

  private void UpdatePSMesh(GameObject go)
  {
    if (this.startParticleParameters == null)
      this.InitStartParticleParameters();
    ParticleSystem[] componentsInChildren1 = this.GetComponentsInChildren<ParticleSystem>();
    MeshRenderer componentInChildren1 = go.GetComponentInChildren<MeshRenderer>();
    SkinnedMeshRenderer componentInChildren2 = go.GetComponentInChildren<SkinnedMeshRenderer>();
    Light[] componentsInChildren2 = this.GetComponentsInChildren<Light>();
    float num1 = 1f;
    float num2 = 1f;
    Vector3 vector3;
    if ((Object) componentInChildren1 != (Object) null)
    {
      vector3 = componentInChildren1.bounds.size;
      num1 = vector3.magnitude;
      vector3 = componentInChildren1.transform.lossyScale;
      num2 = vector3.magnitude;
    }
    if ((Object) componentInChildren2 != (Object) null)
    {
      vector3 = componentInChildren2.bounds.size;
      num1 = vector3.magnitude;
      vector3 = componentInChildren2.transform.lossyScale;
      num2 = vector3.magnitude;
    }
    foreach (ParticleSystem key in componentsInChildren1)
    {
      key.transform.gameObject.SetActive(false);
      ParticleSystem.ShapeModule shape = key.shape;
      if (shape.enabled)
      {
        if ((Object) componentInChildren1 != (Object) null)
        {
          shape.shapeType = ParticleSystemShapeType.MeshRenderer;
          shape.meshRenderer = componentInChildren1;
        }
        if ((Object) componentInChildren2 != (Object) null)
        {
          shape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
          shape.skinnedMeshRenderer = componentInChildren2;
        }
      }
      ParticleSystem.MainModule main = key.main;
      PSMeshRendererUpdater.ParticleStartInfo particleParameter = this.startParticleParameters[key];
      main.startSize = this.UpdateParticleParam(particleParameter.StartSize, main.startSize, num1 / num2 * this.StartScaleMultiplier);
      main.startSpeed = this.UpdateParticleParam(particleParameter.StartSpeed, main.startSpeed, num1 / num2 * this.StartScaleMultiplier);
      key.transform.gameObject.SetActive(true);
    }
    if ((Object) componentInChildren1 != (Object) null)
    {
      foreach (Component component in componentsInChildren2)
        component.transform.position = componentInChildren1.bounds.center;
    }
    if (!((Object) componentInChildren2 != (Object) null))
      return;
    foreach (Component component in componentsInChildren2)
      component.transform.position = componentInChildren2.bounds.center;
  }

  private ParticleSystem.MinMaxCurve UpdateParticleParam(
    ParticleSystem.MinMaxCurve startParam,
    ParticleSystem.MinMaxCurve currentParam,
    float scale)
  {
    if (currentParam.mode == ParticleSystemCurveMode.TwoConstants)
    {
      currentParam.constantMin = startParam.constantMin * scale;
      currentParam.constantMax = startParam.constantMax * scale;
    }
    else if (currentParam.mode == ParticleSystemCurveMode.Constant)
      currentParam.constant = startParam.constant * scale;
    return currentParam;
  }

  private void AddMaterialToMesh(GameObject go)
  {
    ME_MeshMaterialEffect componentInChildren1 = this.GetComponentInChildren<ME_MeshMaterialEffect>();
    if ((Object) componentInChildren1 == (Object) null)
      return;
    MeshRenderer componentInChildren2 = go.GetComponentInChildren<MeshRenderer>();
    SkinnedMeshRenderer componentInChildren3 = go.GetComponentInChildren<SkinnedMeshRenderer>();
    if ((Object) componentInChildren2 != (Object) null)
    {
      this.rendererMaterials.Add(componentInChildren2.sharedMaterials);
      componentInChildren2.sharedMaterials = this.AddToSharedMaterial(componentInChildren2.sharedMaterials, componentInChildren1);
    }
    if (!((Object) componentInChildren3 != (Object) null))
      return;
    this.skinnedMaterials.Add(componentInChildren3.sharedMaterials);
    componentInChildren3.sharedMaterials = this.AddToSharedMaterial(componentInChildren3.sharedMaterials, componentInChildren1);
  }

  private Material[] AddToSharedMaterial(
    Material[] sharedMaterials,
    ME_MeshMaterialEffect meshMatEffect)
  {
    if (meshMatEffect.IsFirstMaterial)
      return new Material[1]{ meshMatEffect.Material };
    List<Material> list = ((IEnumerable<Material>) sharedMaterials).ToList<Material>();
    for (int index = 0; index < list.Count; ++index)
    {
      if (list[index].name.Contains("MeshEffect"))
        list.RemoveAt(index);
    }
    list.Add(meshMatEffect.Material);
    return list.ToArray();
  }

  private void OnDestroy()
  {
    if ((Object) this.MeshObject == (Object) null)
      return;
    MeshRenderer[] componentsInChildren1 = this.MeshObject.GetComponentsInChildren<MeshRenderer>();
    SkinnedMeshRenderer[] componentsInChildren2 = this.MeshObject.GetComponentsInChildren<SkinnedMeshRenderer>();
    for (int index1 = 0; index1 < componentsInChildren1.Length; ++index1)
    {
      if (this.rendererMaterials.Count == componentsInChildren1.Length)
        componentsInChildren1[index1].sharedMaterials = this.rendererMaterials[index1];
      List<Material> list = ((IEnumerable<Material>) componentsInChildren1[index1].sharedMaterials).ToList<Material>();
      for (int index2 = 0; index2 < list.Count; ++index2)
      {
        if (list[index2].name.Contains("MeshEffect"))
          list.RemoveAt(index2);
      }
      componentsInChildren1[index1].sharedMaterials = list.ToArray();
    }
    for (int index3 = 0; index3 < componentsInChildren2.Length; ++index3)
    {
      if (this.skinnedMaterials.Count == componentsInChildren2.Length)
        componentsInChildren2[index3].sharedMaterials = this.skinnedMaterials[index3];
      List<Material> list = ((IEnumerable<Material>) componentsInChildren2[index3].sharedMaterials).ToList<Material>();
      for (int index4 = 0; index4 < list.Count; ++index4)
      {
        if (list[index4].name.Contains("MeshEffect"))
          list.RemoveAt(index4);
      }
      componentsInChildren2[index3].sharedMaterials = list.ToArray();
    }
    this.rendererMaterials.Clear();
    this.skinnedMaterials.Clear();
  }

  private class ParticleStartInfo
  {
    public ParticleSystem.MinMaxCurve StartSize;
    public ParticleSystem.MinMaxCurve StartSpeed;
  }
}
