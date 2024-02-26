// Decompiled with JetBrains decompiler
// Type: Ara.TrailSection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Ara
{
  [CreateAssetMenu(menuName = "Ara Trails/Trail Section")]
  public class TrailSection : ScriptableObject
  {
    [HideInInspector]
    public List<Vector2> vertices;
    public int snapX;
    public int snapY;

    public int Segments => this.vertices.Count - 1;

    public void OnEnable()
    {
      if (this.vertices != null)
        return;
      this.vertices = new List<Vector2>();
      this.CirclePreset(8);
    }

    public void CirclePreset(int segments)
    {
      this.vertices.Clear();
      for (int index = 0; index <= segments; ++index)
      {
        float f = 6.28318548f / (float) segments * (float) index;
        this.vertices.Add(Mathf.Cos(f) * Vector2.right + Mathf.Sin(f) * Vector2.up);
      }
    }

    public static int SnapTo(float val, int snapInterval, int threshold)
    {
      int num1 = (int) val;
      if (snapInterval <= 0)
        return num1;
      int num2 = Mathf.FloorToInt(val / (float) snapInterval) * snapInterval;
      int num3 = num2 + snapInterval;
      if (num1 - num2 < threshold)
        return num2;
      return num3 - num1 < threshold ? num3 : num1;
    }
  }
}
