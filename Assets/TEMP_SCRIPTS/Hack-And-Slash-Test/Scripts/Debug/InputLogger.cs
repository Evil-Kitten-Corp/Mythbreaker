// Decompiled with JetBrains decompiler
// Type: InputLogger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class InputLogger : MonoBehaviour
{
  [Header("[Input Logger]")]
  public Transform content;
  public LoggerItem loggerPrefab;
  public TMP_Text comboName;
  public List<LoggerData> loggerDatas;

  public void SpawnItem(string comboName, EKeystroke keystroke)
  {
    this.comboName.text = "Combo Name : " + comboName;
    Object.Instantiate<LoggerItem>(this.loggerPrefab, this.content).SetItem(string.Format("{0}", (object) keystroke), this.loggerDatas[(int) (keystroke - 1)].inputSprite);
  }

  public void ResetItem()
  {
    foreach (Component componentsInChild in this.content.GetComponentsInChildren<LoggerItem>())
      Object.Destroy((Object) componentsInChild.gameObject);
    this.comboName.text = "Combo Name : ";
  }
}
