using UnityEngine;

[CreateAssetMenu(fileName = "Finisher", menuName = "Scriptable Object/Finisher", order = int.MaxValue)]
public class SO_Finisher : ScriptableObject
{
	[Header("[SO_Finisher]")]
	public FinisherData finisherData;
}
