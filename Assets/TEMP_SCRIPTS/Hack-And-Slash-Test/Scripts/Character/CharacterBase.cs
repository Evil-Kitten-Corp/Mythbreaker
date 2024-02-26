// Decompiled with JetBrains decompiler
// Type: CharacterBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class CharacterBase : MonoBehaviour
{
  [Header("[ReadOnly Variable]")]
  public readonly float idleSpeed;
  public readonly float walkSpeed = 0.5f;
  public readonly float sprintSpeed = 1f;
  [Header("[Component]")]
  [HideInInspector]
  public Animator characterAnim;
  [HideInInspector]
  public Rigidbody characterRig;
  [HideInInspector]
  public Collider characterCollider;
  [Header("[Character Base]")]
  public CharacterState characterState;
  public CharacterOptional characterOptional;
  public AnimationCurves animationCurves;

  private void Start() => this.OnStart();

  private void Update() => this.OnUpdate();

  private void FixedUpdate() => this.OnFixedUpdate();

  protected virtual void OnStart() => this.OnInitialize();

  protected virtual void OnUpdate()
  {
  }

  protected virtual void OnFixedUpdate()
  {
  }

  protected virtual void OnInitialize()
  {
    this.characterAnim = this.GetComponent<Animator>();
    this.characterRig = this.GetComponent<Rigidbody>();
    this.characterCollider = this.GetComponent<Collider>();
  }

  protected abstract void SetGravity();

  protected abstract void CheckGround();

  protected abstract void AirControl();
}
