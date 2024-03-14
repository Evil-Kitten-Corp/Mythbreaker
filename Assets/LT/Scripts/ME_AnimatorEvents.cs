using UnityEngine;

public class ME_AnimatorEvents : MonoBehaviour
{
	public GameObject EffectPrefab;

	public GameObject SwordPrefab;

	public Transform SwordPosition;

	public Transform StartSwordPosition;

	private GameObject EffectInstance;

	private GameObject SwordInstance;

	private void Start()
	{
		if (SwordInstance != null)
		{
			Object.Destroy(SwordInstance);
		}
		SwordInstance = Object.Instantiate(SwordPrefab, StartSwordPosition.position, StartSwordPosition.rotation);
		SwordInstance.transform.parent = StartSwordPosition.transform;
	}

	public void ActivateEffect()
	{
		if (!(EffectPrefab == null) && !(SwordInstance == null))
		{
			if (EffectInstance != null)
			{
				Object.Destroy(EffectInstance);
			}
			EffectInstance = Object.Instantiate(EffectPrefab);
			EffectInstance.transform.parent = SwordInstance.transform;
			EffectInstance.transform.localPosition = Vector3.zero;
			EffectInstance.transform.localRotation = default(Quaternion);
			EffectInstance.GetComponent<PSMeshRendererUpdater>().UpdateMeshEffect(SwordInstance);
		}
	}

	public void ActivateSword()
	{
		SwordInstance.transform.parent = SwordPosition.transform;
		SwordInstance.transform.position = SwordPosition.position;
		SwordInstance.transform.rotation = SwordPosition.rotation;
	}

	public void UpdateColor(float HUE)
	{
		if (!(EffectInstance == null))
		{
			ME_EffectSettingColor settingColor = EffectInstance.GetComponent<ME_EffectSettingColor>();
			if (settingColor == null)
			{
				settingColor = EffectInstance.AddComponent<ME_EffectSettingColor>();
			}
			ME_ColorHelper.HSBColor hsv = ME_ColorHelper.ColorToHSV(settingColor.Color);
			hsv.H = HUE;
			settingColor.Color = ME_ColorHelper.HSVToColor(hsv);
		}
	}
}
