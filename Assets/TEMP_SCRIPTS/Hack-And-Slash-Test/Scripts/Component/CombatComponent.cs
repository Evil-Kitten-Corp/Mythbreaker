// Decompiled with JetBrains decompiler
// Type: CombatComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;

#nullable disable
public class CombatComponent : MonoBehaviour
{
  public Character Character;
  [SerializeField]
  [Header("[Combat Component]")]
  private EKeystroke Keystroke;
  public bool HasInput;
  [SerializeField]
  private int InputCount;
  [SerializeField]
  private List<EKeystroke> CurrentInput;
  [Header("[Timer Handler]")]
  private float InputDelayTime;
  [SerializeField]
  private float ResetTime = 1f;
  [SerializeField]
  private float InputTime = 0.1f;
  [Header("[Combo System]")]
  public EComboState ComboState;
  [SerializeField]
  private List<SO_Combo> Combos;
  [SerializeField]
  private List<SO_Combo> FilterCombos;
  [SerializeField]
  private List<AnimationClip> SaveCombos;
  [Header("[Coroutine]")]
  private Coroutine C_ComboSequence;
  [Header("[Debug]")]
  public InputLogger InputLogger;

  private void Awake() => this.Initialize();

  private void Initialize()
  {
    this.Character = this.GetComponent<Character>();
    foreach (SO_Combo soCombo in Resources.LoadAll<SO_Combo>("Combo"))
      this.Combos.Add(soCombo);
  }

  public void SetComboInput(EKeystroke keystroke)
  {
    if ((double) this.InputDelayTime > (double) Time.time)
      return;
    this.InputDelayTime = Time.time + this.InputTime;
    this.Keystroke = keystroke;
    this.CurrentInput.Add(keystroke);
    this.SaveCombo();
  }

  private void SaveCombo()
  {
    if (this.FilterCombo().Count <= 0)
      return;
    if (this.FilterCombo()[0].comboData.comboInputs[this.InputCount] == this.Keystroke)
    {
      this.SaveCombos.Add(this.FilterCombo()[0].comboData.comboClips[this.InputCount]);
      this.InputLogger.SpawnItem(this.FilterCombo()[0].comboData.comboName, this.Keystroke);
      ++this.InputCount;
    }
    if (this.C_ComboSequence != null)
      this.StopCoroutine(this.C_ComboSequence);
    this.C_ComboSequence = this.StartCoroutine(this.ComboSequence());
  }

  private List<SO_Combo> FilterCombo()
  {
    this.FilterCombos = this.Combos;
    this.FilterCombos.RemoveAll((Predicate<SO_Combo>) (data => data.comboData.comboInputs.Count < this.CurrentInput.Count || data.comboData.comboInputs[this.InputCount] != this.CurrentInput[this.InputCount]));
    if (this.FilterCombos.Count <= 0)
      Debug.LogError((object) "Combo Data is null");
    return this.FilterCombos;
  }

  public void ResetCombo()
  {
    this.ComboState = EComboState.Stop;
    this.Character.CharacterAnim.applyRootMotion = false;
    this.CurrentInput.Clear();
    this.SaveCombos.Clear();
    this.InputCount = 0;
    this.InputLogger.ResetItem();
    this.Combos.Clear();
    foreach (SO_Combo soCombo in Resources.LoadAll<SO_Combo>("Combo"))
      this.Combos.Add(soCombo);
  }

  private IEnumerator ComboSequence()
  {
    yield return new WaitWhile(() => ComboState == EComboState.Playing);
    while (SaveCombos.Count > 0)
    {
      Character.CharacterAnim.CrossFadeInFixedTime(SaveCombos[0].name, 0.1f);
      SaveCombos.RemoveAt(0);
      yield return new WaitForEndOfFrame();
      yield return new WaitWhile(() => ComboState == EComboState.Playing);
    }
    yield return new WaitForSeconds(ResetTime);
    ResetCombo();
  }

  private void OnComboBegin()
  {
    this.ComboState = EComboState.Playing;
    this.Character.CharacterAnim.applyRootMotion = true;
  }

  private void OnComboEnd(int comboEnd)
  {
    this.ComboState = EComboState.Stop;
    this.Character.CharacterAnim.applyRootMotion = false;
    if (comboEnd <= 0)
      return;
    this.ResetCombo();
  }
}
