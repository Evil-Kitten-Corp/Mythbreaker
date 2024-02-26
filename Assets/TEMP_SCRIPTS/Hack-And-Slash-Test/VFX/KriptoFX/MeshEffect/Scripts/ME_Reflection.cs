// Decompiled with JetBrains decompiler
// Type: ME_Reflection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
public class ME_Reflection : MonoBehaviour
{
  public RenderTexture tex;
  private ReflectionProbe reflectionProbe;
  private List<Light> dirLight;
  private List<float> lightIntencity;

  private void Awake()
  {
    Light[] objectsOfType = UnityEngine.Object.FindObjectsOfType<Light>();
    this.dirLight = new List<Light>();
    this.lightIntencity = new List<float>();
    foreach (Light light in objectsOfType)
    {
      if (light.type == LightType.Directional)
      {
        this.dirLight.Add(light);
        this.lightIntencity.Add(light.intensity);
      }
    }
    this.reflectionProbe = this.GetComponent<ReflectionProbe>();
    this.tex = new RenderTexture(this.reflectionProbe.resolution, this.reflectionProbe.resolution, 0);
    this.tex.dimension = TextureDimension.Cube;
    this.tex.useMipMap = true;
    Shader.SetGlobalTexture(nameof (ME_Reflection), (Texture) this.tex);
    this.reflectionProbe.RenderProbe(this.tex);
  }

  private void Update()
  {
    bool flag = false;
    for (int index = 0; index < this.dirLight.Count; ++index)
    {
      if ((double) Math.Abs(this.dirLight[index].intensity - this.lightIntencity[index]) > 1.0 / 1000.0)
      {
        flag = true;
        this.lightIntencity[index] = this.dirLight[index].intensity;
      }
    }
    if (!flag)
      return;
    this.reflectionProbe.RenderProbe(this.tex);
  }
}
