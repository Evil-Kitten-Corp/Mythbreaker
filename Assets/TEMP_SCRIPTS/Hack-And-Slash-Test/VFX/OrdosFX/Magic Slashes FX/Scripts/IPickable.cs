// Decompiled with JetBrains decompiler
// Type: IPickable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

#nullable disable
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;

public interface IPickable
{
  void Pickup(Character owner);

  void Drop();
}
