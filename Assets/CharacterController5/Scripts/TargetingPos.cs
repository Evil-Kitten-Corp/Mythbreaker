using UnityEngine;
using UnityEngine.UI;

namespace CharacterController5.Scripts
{
    public class TargetingPos : MonoBehaviour
    {
        TargetTracker tracker;
        Image targetImage;
 
        // Start is called before the first frame update
        void Start()
        {
            // find the tracker object
            tracker = FindObjectOfType<TargetTracker>();
            // get the image
            targetImage = GetComponent<Image>();
 
        }
 
        // Update is called once per frame
        void Update()
        {
            // if we are locked on and there is a target
            if (tracker.activeTarget != null && tracker.lockedOn)
            {
                // enable the image if it's not enabled already
                if (targetImage.enabled == false)
                {
                    targetImage.enabled = true;
                }
                // move the position of the target in screenspace
                transform.position = Camera.main!.WorldToScreenPoint(tracker.activeTarget.bounds.center);
            }
            // if not locked on, turn off the image if it's still enabled
            else if (targetImage.enabled == true)
            {
                targetImage.enabled = false;
            }
        }
 
    }
}