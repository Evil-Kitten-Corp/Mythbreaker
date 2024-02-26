// Decompiled with JetBrains decompiler
// Type: MonoSingleton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using System.Reflection;
using UnityEngine;

#nullable disable
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
  protected static T _instance;
  private static bool instantiated;
  private bool persistent;
  [SerializeField]
  private bool isDontDestroy;

  public static bool isInitialize => (UnityEngine.Object) MonoSingleton<T>._instance != (UnityEngine.Object) null;

  public static T instance
  {
    get
    {
      if ((UnityEngine.Object) MonoSingleton<T>._instance == (UnityEngine.Object) null)
        MonoSingleton<T>.Create();
      return MonoSingleton<T>._instance;
    }
    set => MonoSingleton<T>._instance = value;
  }

  public static void Create()
  {
    if (!((UnityEngine.Object) MonoSingleton<T>._instance == (UnityEngine.Object) null))
      return;
    T[] objectsOfType = UnityEngine.Object.FindObjectsOfType<T>();
    if (objectsOfType.Length != 0)
    {
      MonoSingleton<T>._instance = objectsOfType[0];
      for (int index = 1; index < objectsOfType.Length; ++index)
      {
        if (Application.isPlaying)
          UnityEngine.Object.Destroy((UnityEngine.Object) objectsOfType[index].gameObject);
        else
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) objectsOfType[index].gameObject);
      }
    }
    else
      MonoSingleton<T>._instance = new GameObject(string.Format("{0}", (object) typeof (T).Name)).AddComponent<T>();
    if (!MonoSingleton<T>.instantiated && Attribute.GetCustomAttribute((MemberInfo) typeof (T), typeof (PersistentAttribute)) is PersistentAttribute customAttribute && customAttribute.Persistent)
    {
      MonoSingleton<T>._instance.persistent = customAttribute.Persistent;
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) MonoSingleton<T>._instance.gameObject);
    }
    MonoSingleton<T>.instantiated = true;
  }

  private void Awake()
  {
    MonoSingleton<T>.Create();
    if (this.isDontDestroy)
    {
      if ((UnityEngine.Object) MonoSingleton<T>.instance != (UnityEngine.Object) this)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      else
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this);
    }
    this.OnAwake();
  }

  protected virtual void OnDestroy()
  {
    if (this.persistent)
      return;
    MonoSingleton<T>.instantiated = false;
    MonoSingleton<T>._instance = default (T);
  }

  protected virtual void OnAwake()
  {
  }
}
