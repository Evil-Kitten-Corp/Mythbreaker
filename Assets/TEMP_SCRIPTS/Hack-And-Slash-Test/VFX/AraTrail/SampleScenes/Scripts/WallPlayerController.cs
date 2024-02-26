// Decompiled with JetBrains decompiler
// Type: AraSamples.WallPlayerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using Ara;
using UnityEngine;

#nullable disable
namespace AraSamples
{
  [RequireComponent(typeof (AraTrail))]
  public class WallPlayerController : MonoBehaviour
  {
    public float speed = 10f;
    public int boardSize = 5;
    public int maxTrailLenght = 10;
    public Color[] colors = new Color[4];
    private int coordX;
    private int coordZ;
    private AraTrail trail;

    private void Awake() => this.trail = this.GetComponent<AraTrail>();

    private void Update()
    {
      float maxDistanceDelta = Time.deltaTime * this.speed;
      Vector3 position = this.transform.position;
      Vector3 vector3 = new Vector3((float) this.coordX, position.y, (float) this.coordZ);
      this.transform.position = Vector3.MoveTowards(position, vector3, maxDistanceDelta);
      if (this.trail.points.Count == 0)
      {
        this.trail.initialColor = this.colors[0];
        this.trail.EmitPoint(position, false);
      }
      if ((double) Vector3.Distance(position, vector3) < (double) maxDistanceDelta)
      {
        this.transform.position = vector3;
        if (Input.GetKeyDown(KeyCode.W))
        {
          this.trail.initialColor = this.colors[0];
          this.trail.EmitPoint(position, false);
          ++this.coordX;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
          this.trail.initialColor = this.colors[1];
          this.trail.EmitPoint(position, false);
          --this.coordX;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
          this.trail.initialColor = this.colors[2];
          this.trail.EmitPoint(position, false);
          ++this.coordZ;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
          this.trail.initialColor = this.colors[3];
          this.trail.EmitPoint(position, false);
          --this.coordZ;
        }
        this.coordX = Mathf.Clamp(this.coordX, -this.boardSize, this.boardSize);
        this.coordZ = Mathf.Clamp(this.coordZ, -this.boardSize, this.boardSize);
      }
      int num = Mathf.Max(0, this.trail.points.Count - this.maxTrailLenght);
      if (num <= 0)
        return;
      this.trail.points.RemoveRange(0, num);
    }
  }
}
