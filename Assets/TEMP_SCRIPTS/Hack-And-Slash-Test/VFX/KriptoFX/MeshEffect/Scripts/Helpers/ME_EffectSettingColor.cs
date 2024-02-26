// Decompiled with JetBrains decompiler
// Type: ME_EffectSettingColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ME_EffectSettingColor : MonoBehaviour
{
  public Color Color = Color.red;
  private Color previousColor;

  private void OnEnable() => this.Update();

  private void Update()
  {
    if (!(this.previousColor != this.Color))
      return;
    this.UpdateColor();
  }

  private void UpdateColor()
  {
    ME_ColorHelper.ChangeObjectColorByHUE(this.gameObject, ME_ColorHelper.ColorToHSV(this.Color).H);
    this.previousColor = this.Color;
  }
}
