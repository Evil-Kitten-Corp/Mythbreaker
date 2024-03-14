using System;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	protected static T _instance;

	private static bool instantiated;

	private bool persistent;

	[SerializeField]
	private bool isDontDestroy;

	public static bool isInitialize => (UnityEngine.Object)_instance != (UnityEngine.Object)null;

	public static T instance
	{
		get
		{
			if ((UnityEngine.Object)_instance == (UnityEngine.Object)null)
			{
				Create();
			}
			return _instance;
		}
		set
		{
			_instance = value;
		}
	}

	public static void Create()
	{
		if (!((UnityEngine.Object)_instance == (UnityEngine.Object)null))
		{
			return;
		}
		T[] objects = UnityEngine.Object.FindObjectsOfType<T>();
		if (objects.Length != 0)
		{
			_instance = objects[0];
			for (int i = 1; i < objects.Length; i++)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(objects[i].gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(objects[i].gameObject);
				}
			}
		}
		else
		{
			_instance = new GameObject($"{typeof(T).Name}").AddComponent<T>();
		}
		if (!instantiated && Attribute.GetCustomAttribute(typeof(T), typeof(PersistentAttribute)) is PersistentAttribute attribute && attribute.Persistent)
		{
			_instance.persistent = attribute.Persistent;
			UnityEngine.Object.DontDestroyOnLoad(_instance.gameObject);
		}
		instantiated = true;
	}

	private void Awake()
	{
		Create();
		if (isDontDestroy)
		{
			if (instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				UnityEngine.Object.DontDestroyOnLoad(this);
			}
		}
		OnAwake();
	}

	protected virtual void OnDestroy()
	{
		if (!persistent)
		{
			instantiated = false;
			_instance = null;
		}
	}

	protected virtual void OnAwake()
	{
	}
}
