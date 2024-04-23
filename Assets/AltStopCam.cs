using UnityEngine;

public class AltStopCam : MonoBehaviour
{
    public KeyCode key;
    public vThirdPersonCamera tpc;
    public AttackControl atc;

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Cursor.visible = true;
            tpc.lockCamera = true;
            atc.enabled = false;
        }
        else if (Input.GetKeyUp(key))
        {
            Cursor.visible = false;
            tpc.lockCamera = false;
            atc.enabled = true; 
        }
    }
}
