using System;
using UnityEngine;

public class ME_MouseOrbit : MonoBehaviour
{
	public GameObject target;

	public float distance = 10f;

	public float xSpeed = 250f;

	public float ySpeed = 120f;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	private float x;

	private float y;

	private float prevDistance;

	private void Start()
	{
		Vector3 angles = base.transform.eulerAngles;
		x = angles.y;
		y = angles.x;
	}

	private void LateUpdate()
	{
		if (distance < 2f)
		{
			distance = 2f;
		}
		distance -= Input.GetAxis("Mouse ScrollWheel") * 2f;
		if ((bool)target && (Input.GetMouseButton(0) || Input.GetMouseButton(1)))
		{
			Vector3 pos = Input.mousePosition;
			float dpiScale = 1f;
			if (Screen.dpi < 1f)
			{
				dpiScale = 1f;
			}
			dpiScale = ((!(Screen.dpi < 200f)) ? (Screen.dpi / 200f) : 1f);
			if (pos.x < 380f * dpiScale && (float)Screen.height - pos.y < 250f * dpiScale)
			{
				return;
			}
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			Quaternion rotation = Quaternion.Euler(y, x, 0f);
			Vector3 position = rotation * new Vector3(0f, 0f, 0f - distance) + target.transform.position;
			base.transform.rotation = rotation;
			base.transform.position = position;
		}
		else
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		if (Math.Abs(prevDistance - distance) > 0.001f)
		{
			prevDistance = distance;
			Quaternion rot = Quaternion.Euler(y, x, 0f);
			Vector3 po = rot * new Vector3(0f, 0f, 0f - distance) + target.transform.position;
			base.transform.rotation = rot;
			base.transform.position = po;
		}
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
