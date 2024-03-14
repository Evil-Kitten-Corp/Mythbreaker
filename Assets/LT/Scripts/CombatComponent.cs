using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private void Awake()
	{
		Initialize();
	}

	private void Initialize()
	{
		Character = GetComponent<Character>();
		SO_Combo[] array = Resources.LoadAll<SO_Combo>("Combo");
		foreach (SO_Combo data in array)
		{
			Combos.Add(data);
		}
	}

	public void SetComboInput(EKeystroke keystroke)
	{
		if (InputDelayTime <= Time.time)
		{
			InputDelayTime = Time.time + InputTime;
			Keystroke = keystroke;
			CurrentInput.Add(keystroke);
			SaveCombo();
		}
	}

	private void SaveCombo()
	{
		if (FilterCombo().Count > 0)
		{
			if (FilterCombo()[0].comboData.comboInputs[InputCount] == Keystroke)
			{
				SaveCombos.Add(FilterCombo()[0].comboData.comboClips[InputCount]);
				InputLogger.SpawnItem(FilterCombo()[0].comboData.comboName, Keystroke);
				InputCount++;
			}
			if (C_ComboSequence != null)
			{
				StopCoroutine(C_ComboSequence);
			}
			C_ComboSequence = StartCoroutine(ComboSequence());
		}
	}

	private List<SO_Combo> FilterCombo()
	{
		FilterCombos = Combos;
		FilterCombos.RemoveAll((SO_Combo data) => data.comboData.comboInputs.Count < CurrentInput.Count || data.comboData.comboInputs[InputCount] != CurrentInput[InputCount]);
		if (FilterCombos.Count <= 0)
		{
			Debug.LogError("Combo Data is null");
		}
		return FilterCombos;
	}

	public void ResetCombo()
	{
		ComboState = EComboState.Stop;
		Character.CharacterAnim.applyRootMotion = false;
		CurrentInput.Clear();
		SaveCombos.Clear();
		InputCount = 0;
		InputLogger.ResetItem();
		Combos.Clear();
		SO_Combo[] array = Resources.LoadAll<SO_Combo>("Combo");
		foreach (SO_Combo data in array)
		{
			Combos.Add(data);
		}
	}

	private IEnumerator ComboSequence()
	{
		yield return (object)new WaitWhile((Func<bool>)(() => ComboState == EComboState.Playing));
		while (SaveCombos.Count > 0)
		{
			Character.CharacterAnim.CrossFadeInFixedTime(SaveCombos[0].name, 0.1f);
			SaveCombos.RemoveAt(0);
			yield return new WaitForEndOfFrame();
			yield return (object)new WaitWhile((Func<bool>)(() => ComboState == EComboState.Playing));
		}
		yield return new WaitForSeconds(ResetTime);
		ResetCombo();
	}

	private void OnComboBegin()
	{
		ComboState = EComboState.Playing;
		Character.CharacterAnim.applyRootMotion = true;
	}

	private void OnComboEnd(int comboEnd)
	{
		ComboState = EComboState.Stop;
		Character.CharacterAnim.applyRootMotion = false;
		if (comboEnd > 0)
		{
			ResetCombo();
		}
	}
}
