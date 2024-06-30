using UnityEngine;
using UnityEngine.UI;

namespace FinalScripts
{
    public class TeleportIndicator : MonoBehaviour
    {
        public Sprite targetIndicatorImage;
        public Sprite offScreenTargetIndicator;
        public float outOfSightOffset = 20f;

        public GameObject target;
        public Camera mainCamera;
        public RectTransform canvasRect;
        
        public RectTransform rectTransform;
        
        private Image _img;
        public bool canUpdate = true;

        private void Awake()
        {
            _img = rectTransform.GetComponent<Image>();
        }

        public void Update()
        {
            if (canUpdate)
            {
                SetIndicatorPosition();
            }
        }

        public void StopUpdate()
        {
            canUpdate = false;
            _img.gameObject.SetActive(false);
        }

        private void SetIndicatorPosition()
        {
            _img.gameObject.SetActive(true);
            Vector3 indicatorPosition = mainCamera.WorldToScreenPoint(target.transform.position);

            if (indicatorPosition.z >= 0f & indicatorPosition.x <= canvasRect.rect.width * canvasRect.localScale.x
             & indicatorPosition.y <= canvasRect.rect.height * canvasRect.localScale.x & indicatorPosition.x >= 0f 
             & indicatorPosition.y >= 0f)
            {
                indicatorPosition.z = 0f;

                TargetOutOfSight(false, indicatorPosition);
            }
            else if (indicatorPosition.z >= 0f)
            {
                indicatorPosition = OutOfRangeIndicatorPositionB(indicatorPosition);
                TargetOutOfSight(true, indicatorPosition);
            }
            else
            {
                indicatorPosition *= -1f;

                indicatorPosition = OutOfRangeIndicatorPositionB(indicatorPosition);
                TargetOutOfSight(true, indicatorPosition);

            }

            rectTransform.position = indicatorPosition;

        }

        private Vector3 OutOfRangeIndicatorPositionB(Vector3 indicatorPosition)
        {
            indicatorPosition.z = 0f;

            Vector3 canvasCenter = new Vector3(canvasRect.rect.width / 2f, canvasRect.rect.height / 2f, 0f) 
                                   * canvasRect.localScale.x;
            
            indicatorPosition -= canvasCenter;

            float divX = (canvasRect.rect.width / 2f - outOfSightOffset) / Mathf.Abs(indicatorPosition.x);
            float divY = (canvasRect.rect.height / 2f - outOfSightOffset) / Mathf.Abs(indicatorPosition.y);

            if (divX < divY)
            {
                float angle = Vector3.SignedAngle(Vector3.right, indicatorPosition, Vector3.forward);
                indicatorPosition.x = Mathf.Sign(indicatorPosition.x) * (canvasRect.rect.width * 0.5f - outOfSightOffset) 
                                                                      * canvasRect.localScale.x;
                indicatorPosition.y = Mathf.Tan(Mathf.Deg2Rad * angle) * indicatorPosition.x;
            }

            else
            {
                float angle = Vector3.SignedAngle(Vector3.up, indicatorPosition, Vector3.forward);

                indicatorPosition.y = Mathf.Sign(indicatorPosition.y) * (canvasRect.rect.height / 2f - outOfSightOffset) 
                                                                      * canvasRect.localScale.y;
                indicatorPosition.x = -Mathf.Tan(Mathf.Deg2Rad * angle) * indicatorPosition.y;
            }

            indicatorPosition += canvasCenter;
            return indicatorPosition;
        }
        

        private void TargetOutOfSight(bool oos, Vector3 indicatorPosition)
        {
            if (oos)
            {
                _img.sprite = offScreenTargetIndicator;
                rectTransform.rotation = Quaternion.Euler(RotationOutOfSightTargetIndicator(indicatorPosition));
            }
            else
            {
                _img.sprite = targetIndicatorImage;
            }
        }

        private Vector3 RotationOutOfSightTargetIndicator(Vector3 indicatorPosition)
        {
            //Calculate the canvasCenter
            Vector3 canvasCenter = new Vector3(canvasRect.rect.width / 2f, 
                canvasRect.rect.height / 2f, 0f) * canvasRect.localScale.x;

            //Calculate the signedAngle between the position of the indicator and the Direction up.
            float angle = Vector3.SignedAngle(Vector3.up, indicatorPosition - canvasCenter, Vector3.forward);

            //return the angle as a rotation Vector
            return new Vector3(0f, 0f, angle);
        }
    }
}