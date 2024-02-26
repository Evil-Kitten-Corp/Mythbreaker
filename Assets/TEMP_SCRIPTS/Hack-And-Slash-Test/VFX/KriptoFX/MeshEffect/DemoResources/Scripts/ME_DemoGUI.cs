// Decompiled with JetBrains decompiler
// Type: ME_DemoGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ME_DemoGUI : MonoBehaviour
{
  public GameObject Character;
  public GameObject Model;
  public int Current;
  public GameObject[] Prefabs;
  public Light Sun;
  public ReflectionProbe ReflectionProbe;
  public Light[] NightLights = new Light[0];
  public Texture HUETexture;
  public bool UseMobileVersion;
  public GameObject MobileCharacter;
  public GameObject Target;
  public Color guiColor = Color.red;
  private int currentNomber;
  private GameObject characterInstance;
  private GameObject modelInstance;
  private GUIStyle guiStyleHeader = new GUIStyle();
  private GUIStyle guiStyleHeaderMobile = new GUIStyle();
  private float dpiScale;
  private bool isDay;
  private float colorHUE;
  private float startSunIntensity;
  private Quaternion startSunRotation;
  private Color startAmbientLight;
  private float startAmbientIntencity;
  private float startReflectionIntencity;
  private LightShadows startLightShadows;
  private bool isButtonPressed;
  private GameObject instanceShieldProjectile;

  private void Start()
  {
    if ((double) Screen.dpi < 1.0)
      this.dpiScale = 1f;
    this.dpiScale = (double) Screen.dpi >= 200.0 ? Screen.dpi / 200f : 1f;
    this.guiStyleHeader.fontSize = (int) (15.0 * (double) this.dpiScale);
    this.guiStyleHeader.normal.textColor = this.guiColor;
    this.guiStyleHeaderMobile.fontSize = (int) (17.0 * (double) this.dpiScale);
    this.ChangeCurrent(this.Current);
    this.startSunIntensity = this.Sun.intensity;
    this.startSunRotation = this.Sun.transform.rotation;
    this.startAmbientLight = RenderSettings.ambientLight;
    this.startAmbientIntencity = RenderSettings.ambientIntensity;
    this.startReflectionIntencity = RenderSettings.reflectionIntensity;
    this.startLightShadows = this.Sun.shadows;
  }

  private void OnGUI()
  {
    if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow))
      this.isButtonPressed = false;
    if (GUI.Button(new Rect(10f * this.dpiScale, 15f * this.dpiScale, 135f * this.dpiScale, 37f * this.dpiScale), "PREVIOUS EFFECT") || !this.isButtonPressed && Input.GetKeyDown(KeyCode.LeftArrow))
    {
      this.isButtonPressed = true;
      this.ChangeCurrent(-1);
    }
    if (GUI.Button(new Rect(160f * this.dpiScale, 15f * this.dpiScale, 135f * this.dpiScale, 37f * this.dpiScale), "NEXT EFFECT") || !this.isButtonPressed && Input.GetKeyDown(KeyCode.RightArrow))
    {
      this.isButtonPressed = true;
      this.ChangeCurrent(1);
    }
    float num1 = 0.0f;
    if (GUI.Button(new Rect(10f * this.dpiScale, 63f * this.dpiScale + num1, 285f * this.dpiScale, 37f * this.dpiScale), "Day / Night") || !this.isButtonPressed && Input.GetKeyDown(KeyCode.DownArrow))
    {
      this.isButtonPressed = true;
      if ((Object) this.ReflectionProbe != (Object) null)
        this.ReflectionProbe.RenderProbe();
      this.Sun.intensity = !this.isDay ? 0.05f : this.startSunIntensity;
      this.Sun.shadows = this.isDay ? this.startLightShadows : LightShadows.None;
      foreach (Light nightLight in this.NightLights)
        nightLight.shadows = !this.isDay ? this.startLightShadows : LightShadows.None;
      this.Sun.transform.rotation = this.isDay ? this.startSunRotation : Quaternion.Euler(350f, 30f, 90f);
      RenderSettings.ambientLight = !this.isDay ? new Color(0.2f, 0.2f, 0.2f) : this.startAmbientLight;
      float num2 = !this.UseMobileVersion ? 1f : 0.3f;
      RenderSettings.ambientIntensity = this.isDay ? this.startAmbientIntencity : num2;
      RenderSettings.reflectionIntensity = this.isDay ? this.startReflectionIntencity : 0.2f;
      this.isDay = !this.isDay;
    }
    GUI.Label(new Rect(400f * this.dpiScale, (float) (15.0 * (double) this.dpiScale + (double) num1 / 2.0), 100f * this.dpiScale, 20f * this.dpiScale), "Prefab name is \"" + this.Prefabs[this.currentNomber].name + "\"  \r\nHold any mouse button that would move the camera", this.guiStyleHeader);
    GUI.DrawTexture(new Rect(12f * this.dpiScale, 140f * this.dpiScale + num1, 285f * this.dpiScale, 15f * this.dpiScale), this.HUETexture, ScaleMode.StretchToFill, false, 0.0f);
    double colorHue1 = (double) this.colorHUE;
    this.colorHUE = GUI.HorizontalSlider(new Rect(12f * this.dpiScale, 147f * this.dpiScale + num1, 285f * this.dpiScale, 15f * this.dpiScale), this.colorHUE, 0.0f, 360f);
    double colorHue2 = (double) this.colorHUE;
    if ((double) Mathf.Abs((float) (colorHue1 - colorHue2)) <= 0.001)
      return;
    PSMeshRendererUpdater componentInChildren1 = this.characterInstance.GetComponentInChildren<PSMeshRendererUpdater>();
    if ((Object) componentInChildren1 != (Object) null)
      componentInChildren1.UpdateColor(this.colorHUE / 360f);
    PSMeshRendererUpdater componentInChildren2 = this.modelInstance.GetComponentInChildren<PSMeshRendererUpdater>();
    if (!((Object) componentInChildren2 != (Object) null))
      return;
    componentInChildren2.UpdateColor(this.colorHUE / 360f);
  }

  private void ChangeCurrent(int delta)
  {
    this.currentNomber += delta;
    if (this.currentNomber > this.Prefabs.Length - 1)
      this.currentNomber = 0;
    else if (this.currentNomber < 0)
      this.currentNomber = this.Prefabs.Length - 1;
    if ((Object) this.characterInstance != (Object) null)
    {
      Object.Destroy((Object) this.characterInstance);
      this.RemoveClones();
    }
    if ((Object) this.modelInstance != (Object) null)
    {
      Object.Destroy((Object) this.modelInstance);
      this.RemoveClones();
    }
    this.characterInstance = Object.Instantiate<GameObject>(this.Character);
    this.characterInstance.GetComponent<ME_AnimatorEvents>().EffectPrefab = this.Prefabs[this.currentNomber];
    this.modelInstance = Object.Instantiate<GameObject>(this.Model);
    GameObject gameObject = Object.Instantiate<GameObject>(this.Prefabs[this.currentNomber]);
    gameObject.transform.parent = this.modelInstance.transform;
    gameObject.transform.localPosition = Vector3.zero;
    gameObject.transform.localRotation = new Quaternion();
    gameObject.GetComponent<PSMeshRendererUpdater>().UpdateMeshEffect(this.modelInstance);
    if (!this.UseMobileVersion)
      return;
    this.CancelInvoke("ReactivateEffect");
  }

  private void RemoveClones()
  {
    foreach (GameObject gameObject in Object.FindObjectsOfType<GameObject>())
    {
      if (gameObject.name.Contains("(Clone)"))
        Object.Destroy((Object) gameObject);
    }
  }

  private void ReactivateEffect()
  {
    this.characterInstance.SetActive(false);
    this.characterInstance.SetActive(true);
    this.modelInstance.SetActive(false);
    this.modelInstance.SetActive(true);
  }
}
