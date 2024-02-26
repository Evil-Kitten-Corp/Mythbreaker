// Decompiled with JetBrains decompiler
// Type: Slash_UIEffectList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Slash_UIEffectList : MonoBehaviour
{
  public GameObject[] Prefabs;
  private int currentNomber;
  private GameObject currentInstance;

  private void Start()
  {
  }

  private void Update()
  {
  }

  public void ChangeCurrent(int delta)
  {
    this.currentNomber += delta;
    if (this.currentNomber > this.Prefabs.Length - 1)
      this.currentNomber = 0;
    else if (this.currentNomber < 0)
      this.currentNomber = this.Prefabs.Length - 1;
    if ((Object) this.currentInstance != (Object) null)
    {
      Object.Destroy((Object) this.currentInstance);
      this.RemoveClones();
    }
    this.currentInstance = Object.Instantiate<GameObject>(this.Prefabs[this.currentNomber]);
  }

  private void RemoveClones()
  {
    foreach (GameObject gameObject in Object.FindObjectsOfType<GameObject>())
    {
      if (gameObject.name.Contains("(Clone)"))
        Object.Destroy((Object) gameObject);
    }
  }
}
