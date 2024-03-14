using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
		Object.Instantiate<LoggerItem>(loggerPrefab, content).SetItem($"{keystroke}", loggerDatas[(int)(keystroke - 1)].inputSprite);
	}

	public void ResetItem()
	{
		LoggerItem[] componentsInChildren = content.GetComponentsInChildren<LoggerItem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.Destroy(componentsInChildren[i].gameObject);
		}
		comboName.text = "Combo Name : ";
	}
}
