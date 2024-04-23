using Invector.vCharacterController;
using UnityEngine;

public class MovementLocker : MonoBehaviour
{
    public vThirdPersonController tpc;
    public vThirdPersonCamera tpcam;
    public AttackControl attack; 

    private void Update()
    {
        EnableMovement(false);
    }

    public void EnableMovement(bool enable) 
    {
        if(enable == false)
        {
            tpc.lockMovement = true;
            tpc.lockRotation = true;
            tpcam.lockCamera = true; 
            attack.enabled = false; 
            Cursor.visible = true;

        }
        else
        {
            tpc.lockMovement = false;
            tpc.lockRotation = false;
            tpcam.lockCamera = false;
            attack.enabled = true;
            Cursor.visible = false;
        }
    }
}
