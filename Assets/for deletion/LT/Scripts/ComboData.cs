using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ComboData
{
	[Header("[Combo Data]")]
	public string rowName;

	public string comboName;

	public List<EKeystroke> comboInputs;

	public List<AnimationClip> comboClips;
}
