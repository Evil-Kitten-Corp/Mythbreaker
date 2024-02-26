// Decompiled with JetBrains decompiler
// Type: Balrond3PersonMovements.Balrond3pMainCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Balrond3PersonMovements
{
  public class Balrond3pMainCamera : MonoBehaviour
  {
    private Transform target;
    private float rotationSmoothing = 0.1f;
    private float distanceToTarget;
    private float velocityX;
    private float velocityY;
    private float rotationYAxis;
    private float rotationXAxis;
    public float zoomSpeed = 0.5f;
    public float distance;
    public float minDistance;
    public float maxDistance = 7f;
    private Balrond3pCameraFollow follow;

    private void Start()
    {
      this.follow = this.transform.parent.GetComponent<Balrond3pCameraFollow>();
      this.setBasePosition();
    }

    private void FixedUpdate() => this.rotation();

    private void setBasePosition()
    {
      this.target = this.transform.parent.transform;
      this.distanceToTarget = Vector3.Distance(this.target.position, this.transform.position) + this.follow.maxDistance;
      Vector3 eulerAngles = this.transform.eulerAngles;
      this.rotationYAxis = eulerAngles.y;
      this.rotationXAxis = eulerAngles.x;
      if (!(bool) (Object) this.GetComponent<Rigidbody>())
        return;
      this.GetComponent<Rigidbody>().freezeRotation = true;
    }

    private void rotation()
    {
      if (!(bool) (Object) this.target)
        return;
      if (Input.GetMouseButton(0))
      {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.velocityX += (float) ((double) this.rotationSmoothing * 150.0 * (double) Input.GetAxis("Mouse X") * (double) this.distanceToTarget * 10.0 * 0.019999999552965164);
        this.velocityY += (float) ((double) this.rotationSmoothing * 150.0 * (double) Input.GetAxis("Mouse Y") * 0.019999999552965164);
      }
      else
      {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
      }
      this.rotationYAxis += this.velocityX;
      this.rotationXAxis -= this.velocityY;
      this.rotationXAxis = Balrond3pMainCamera.ClampAngle(this.rotationXAxis, -90f, 90f);
      Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, 0.0f);
      Quaternion quaternion = Quaternion.Euler(this.rotationXAxis, this.rotationYAxis, 0.0f);
      Vector3 vector3_1 = new Vector3(0.0f, 0.0f, -this.distanceToTarget);
      Vector3 vector3_2 = quaternion * vector3_1 + this.target.position;
      this.transform.rotation = quaternion;
      this.transform.position = vector3_2;
      this.velocityX = Mathf.Lerp(this.velocityX, 0.0f, this.rotationSmoothing * 20f);
      this.velocityY = Mathf.Lerp(this.velocityY, 0.0f, this.rotationSmoothing * 20f);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
      if ((double) angle < -360.0)
        angle += 360f;
      if ((double) angle > 360.0)
        angle -= 360f;
      return Mathf.Clamp(angle, min, max);
    }
  }
}
