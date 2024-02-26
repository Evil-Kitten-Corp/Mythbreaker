// Decompiled with JetBrains decompiler
// Type: AraSamples.ObjectDragger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace AraSamples
{
  public class ObjectDragger : MonoBehaviour
  {
    private Vector3 screenPoint;
    private Vector3 offset;

    private void OnMouseDown()
    {
      this.screenPoint = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
      this.offset = this.gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.screenPoint.z));
    }

    private void OnMouseDrag()
    {
      this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.screenPoint.z)) + this.offset;
    }
  }
}
