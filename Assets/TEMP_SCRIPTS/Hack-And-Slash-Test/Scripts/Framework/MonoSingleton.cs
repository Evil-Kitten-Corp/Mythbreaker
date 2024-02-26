// Decompiled with JetBrains decompiler
// Type: PersistentAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class PersistentAttribute : Attribute
{
  public readonly bool Persistent;

  public PersistentAttribute(bool persistent) => this.Persistent = persistent;
}
