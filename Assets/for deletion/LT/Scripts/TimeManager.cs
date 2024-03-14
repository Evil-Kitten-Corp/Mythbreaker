using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
	[Header("[Time Manager]")]
	public float SlowMotionTime;

	public bool IsSlowMotion;

	private void Update()
	{
		SlowMotionTimer();
	}

	private void SlowMotionTimer()
	{
		if (IsSlowMotion && SlowMotionTime > 0f)
		{
			SlowMotionTime -= Time.unscaledDeltaTime;
			if (SlowMotionTime <= 0f)
			{
				OffSlowMotion();
			}
		}
	}

	public void OnSlowMotion(float timeScale, float timer = 0f)
	{
		Time.timeScale = timeScale;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		IsSlowMotion = true;
		SlowMotionTime = timer;
	}

	public void OffSlowMotion()
	{
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		IsSlowMotion = false;
	}
}
