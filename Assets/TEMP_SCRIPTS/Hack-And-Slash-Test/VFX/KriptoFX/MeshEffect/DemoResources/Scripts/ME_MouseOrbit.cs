// Decompiled with JetBrains decompiler
// Type: ME_MouseOrbit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ME_MouseOrbit : MonoBehaviour
{
  public GameObject target;
  public float distance = 10f;
  public float xSpeed = 250f;
  public float ySpeed = 120f;
  public float yMinLimit = -20f;
  public float yMaxLimit = 80f;
  private float x;
  private float y;
  private float prevDistance;

  private void Start()
  {
    Vector3 eulerAngles = this.transform.eulerAngles;
    this.x = eulerAngles.y;
    this.y = eulerAngles.x;
  }

  private void LateUpdate()
  {
    if ((double) this.distance < 2.0)
      this.distance = 2f;
    this.distance -= Input.GetAxis("Mouse ScrollWheel") * 2f;
    if ((bool) (UnityEngine.Object) this.target && (Input.GetMouseButton(0) || Input.GetMouseButton(1)))
    {
      Vector3 mousePosition = Input.mousePosition;
      float num1 = 1f;
      if ((double) Screen.dpi < 1.0)
        num1 = 1f;
      float num2 = (double) Screen.dpi >= 200.0 ? Screen.dpi / 200f : 1f;
      if ((double) mousePosition.x < 380.0 * (double) num2 && (double) Screen.height - (double) mousePosition.y < 250.0 * (double) num2)
        return;
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
      this.x += (float) ((double) Input.GetAxis("Mouse X") * (double) this.xSpeed * 0.019999999552965164);
      this.y -= (float) ((double) Input.GetAxis("Mouse Y") * (double) this.ySpeed * 0.019999999552965164);
      this.y = ME_MouseOrbit.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
      Quaternion quaternion = Quaternion.Euler(this.y, this.x, 0.0f);
      Vector3 vector3 = quaternion * new Vector3(0.0f, 0.0f, -this.distance) + this.target.transform.position;
      this.transform.rotation = quaternion;
      this.transform.position = vector3;
    }
    else
    {
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
    }
    if ((double) Math.Abs(this.prevDistance - this.distance) <= 1.0 / 1000.0)
      return;
    this.prevDistance = this.distance;
    Quaternion quaternion1 = Quaternion.Euler(this.y, this.x, 0.0f);
    Vector3 vector3_1 = quaternion1 * new Vector3(0.0f, 0.0f, -this.distance) + this.target.transform.position;
    this.transform.rotation = quaternion1;
    this.transform.position = vector3_1;
  }

  private static float ClampAngle(float angle, float min, float max)
  {
    if ((double) angle < -360.0)
      angle += 360f;
    if ((double) angle > 360.0)
      angle -= 360f;
    return Mathf.Clamp(angle, min, max);
  }
}
