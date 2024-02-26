// Decompiled with JetBrains decompiler
// Type: Util
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;

#nullable disable
public class Util
{
  public static bool CompareToCharacter(Character a, Character b) => (Object) a == (Object) b;

  public static GameObject GetNearestToObject(GameObject owner, float radius, LayerMask layerMask)
  {
    Collider[] colliderArray = Physics.OverlapSphere(owner.transform.position, radius, layerMask.value);
    float num1 = float.PositiveInfinity;
    GameObject nearestToObject = (GameObject) null;
    foreach (Collider collider in colliderArray)
    {
      if ((Object) owner.gameObject != (Object) collider.gameObject)
      {
        float num2 = Vector3.Distance(owner.transform.position, collider.transform.position);
        if ((double) num1 > (double) num2)
        {
          num1 = num2;
          nearestToObject = collider.gameObject;
        }
      }
    }
    return nearestToObject;
  }

  public static GameObject GetDirectionToObject(
    GameObject owner,
    float radius,
    float angle,
    LayerMask layerMask)
  {
    Collider[] colliderArray = Physics.OverlapSphere(owner.transform.position, radius, layerMask.value);
    GameObject directionToObject = (GameObject) null;
    foreach (Collider collider in colliderArray)
    {
      Vector3 direction = Util.GetDirection(collider.transform.position, owner.transform.position);
      if ((double) Vector3.Angle(owner.GetComponent<CharacterMovement>().GetInputDirection, -direction) < (double) angle * 0.5 && (Object) owner.gameObject != (Object) collider.gameObject)
        directionToObject = collider.gameObject;
    }
    return directionToObject;
  }

  public static Vector3 GetDirection(Vector3 a, Vector3 b, bool ignoreY = true)
  {
    Vector3 normalized = (b - a).normalized;
    if (ignoreY)
      normalized.y = 0.0f;
    return normalized;
  }

  public static float GetDistance(Vector3 a, Vector3 b) => Vector3.Distance(a, b);
}
