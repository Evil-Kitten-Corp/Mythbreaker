// Decompiled with JetBrains decompiler
// Type: AraSamples.CarController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace AraSamples
{
  public class CarController : MonoBehaviour
  {
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
      if (collider.transform.childCount == 0)
        return;
      Transform child = collider.transform.GetChild(0);
      Vector3 pos;
      Quaternion quat;
      collider.GetWorldPose(out pos, out quat);
      child.transform.position = pos;
      child.transform.rotation = quat;
    }

    public void FixedUpdate()
    {
      float num1 = this.maxMotorTorque * Input.GetAxis("Vertical");
      float num2 = this.maxSteeringAngle * Input.GetAxis("Horizontal");
      foreach (AxleInfo axleInfo in this.axleInfos)
      {
        if (axleInfo.steering)
        {
          axleInfo.leftWheel.steerAngle = num2;
          axleInfo.rightWheel.steerAngle = num2;
        }
        if (axleInfo.motor)
        {
          axleInfo.leftWheel.motorTorque = num1;
          axleInfo.rightWheel.motorTorque = num1;
        }
        this.ApplyLocalPositionToVisuals(axleInfo.leftWheel);
        this.ApplyLocalPositionToVisuals(axleInfo.rightWheel);
      }
    }
  }
}
