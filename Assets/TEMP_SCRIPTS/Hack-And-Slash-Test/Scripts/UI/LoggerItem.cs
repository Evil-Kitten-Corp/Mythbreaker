// Decompiled with JetBrains decompiler
// Type: LoggerItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LoggerItem : MonoBehaviour
{
  [Header("[Logger Item]")]
  public TMP_Text itemText;
  public Image itemImage;

  public void SetItem(string itemName, Sprite itemSprite)
  {
    this.itemText.text = itemName;
    this.itemImage.sprite = itemSprite;
  }
}
