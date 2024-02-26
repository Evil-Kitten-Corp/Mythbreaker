using System.Collections;
using UnityEngine;

namespace Characters
{
    public class GrapplingHook : MonoBehaviour
    {
        [SerializeField] Hook[] hooklist = new Hook[2];
        PlayerControllerI controller;
        Transform cameraTr;
        Vector3 playerToHitDir;

        public float radius = 5.0f;
        float timer;
        public float maxDistance = 100.0f;
        public float maxHookPower;
        float currentHookPower;

        RaycastHit rayhit;

        [SerializeField]
        GameObject leftHand;
        [SerializeField]
        GameObject rightHand;

        public AnimationCurve affectCurve;
        float ropeWidth = 0.05f;
        public int lineCount;
        public float waveHeight;
        public float waveCount;
        public float waveSpeed;
        public float ropeSpeed;
        public float damper;
        public float strength;

        bool startFlag = false;
        bool stopFlag = false;
        bool endFlag = false;
        bool IsForwardMove = false;


        int maxCount = 300;
        float maxSpeed = 100.0f;

        #region Event Functions

        void Start()
        {
            controller = GetComponent<PlayerControllerI>();
            cameraTr = Camera.main.transform;

            hooklist[0] = new Hook();
            hooklist[1] = new Hook();

            hooklist[0].handObj = leftHand;
            hooklist[1].handObj = rightHand;

            hooklist[0].lineRenderer = leftHand.GetComponent<LineRenderer>();
            hooklist[1].lineRenderer = rightHand.GetComponent<LineRenderer>();

            hooklist[0].lineRenderer.startWidth = ropeWidth;
            hooklist[0].lineRenderer.endWidth = ropeWidth;
            hooklist[1].lineRenderer.startWidth = ropeWidth;
            hooklist[1].lineRenderer.endWidth = ropeWidth;

            hooklist[0].spring = new Spring();
            hooklist[1].spring = new Spring();

            hooklist[0].spring.target = 0;
            hooklist[1].spring.target = 0;
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                HookRay();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                ClearHook();
            }

            if (Input.GetMouseButton(0))
            {
                stopFlag = false;
                if (!startFlag)
                {
                    startFlag = true;
                    StartCoroutine(HookPointMoveCoroutine());
                }

            }
            else if (Input.GetMouseButtonUp(0))
            {
                stopFlag = true;
            }

            if (Input.GetMouseButton(1))
            {
                stopFlag = false;
                if (!startFlag)
                {
                    startFlag = true;
                    StartCoroutine(SlerpMoveCoroutine());
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                stopFlag = true;
            }
        }
    
        private void LateUpdate()
        {
            DrawRope();
        }

        #endregion
    
        void SetHookList(RaycastHit hook_)
        {
            //Direction of the target
            Vector3 targetDir = (hook_.point - this.transform.position).normalized;

            // Cross Product -> Inner Product for accurate orientation and angle.
            Vector3 cross = Vector3.Cross(this.transform.forward, targetDir);
            float Angle = Vector3.Dot(Vector3.up, cross) * Mathf.Rad2Deg;

            if (Angle > 0)
            {
                hooklist[1].IsHook = false;
                hooklist[1].IsRope = false;
                hooklist[1].currentRopePosition = hooklist[1].handObj.transform.position;
                hooklist[1].lineRenderer.positionCount = 0;
                hooklist[1].spring.Reset();
                hooklist[1].hitRay = hook_;
                UpdateDir(hooklist[1]);
                SetIkHand(1);
                StartCoroutine(LineRendererSetCoroutine(1));
            }
            else
            {
                hooklist[0].IsHook = false;
                hooklist[0].IsRope = false;
                hooklist[0].currentRopePosition = hooklist[0].handObj.transform.position;
                hooklist[0].lineRenderer.positionCount = 0;
                hooklist[0].spring.Reset();
                hooklist[0].hitRay = hook_;
                UpdateDir(hooklist[0]);
                SetIkHand(0);
                StartCoroutine(LineRendererSetCoroutine(0));
            }
        
        }

        IEnumerator LineRendererSetCoroutine(int i)
        {
            if (i == 0)
            {
                while (true)
                {
                    yield return null;
                }
            }
            else if (i == 1)
            {
                while (true)
                {
                    yield return null;
                }
            }

            hooklist[i].IsHook = true;
            hooklist[i].lineRenderer.positionCount = lineCount + 1;
            hooklist[i].spring.velocity =waveSpeed;
            hooklist[i].spring.damper= damper;
            hooklist[i].spring.strength =strength;

            while (true)
            {
                if (hooklist[i].lineRenderer.positionCount == 0)
                    yield break;

                if (Vector3.Distance(hooklist[i].hitRay.point, hooklist[i].lineRenderer.GetPosition(lineCount)) < 0.2f)
                {
                    hooklist[i].IsRope = true;
                    break;
                }
                yield return null;
            }
        }
        void UpdateDir(Hook hook)
        {
            hook.cameraToHitDir = hook.hitRay.point - cameraTr.transform.position;
            hook.handToHitDir = hook.hitRay.point - hook.handObj.transform.position;
        }

        void HookRay()
        {
            Hook _hook = new Hook();

            if (Physics.Raycast(cameraTr.position, cameraTr.forward, out _hook.hitRay, maxDistance, 
                    LayerMask.NameToLayer("Player")))
            {
            }
            else
            {
                if (Physics.SphereCast(cameraTr.position, radius, cameraTr.forward, out _hook.hitRay, 
                        maxDistance, LayerMask.NameToLayer("Player")))
                {
                }
            }

            if (_hook.hitRay.collider)
            {
                SetHookList(_hook.hitRay);
            }
        }

        void SetIkHand()
        {
        }

        bool SetIkHand(int i)
        {
            return true;
        }

        void DrawRope()
        {
            for (int i = 0; i < hooklist.Length; i++)
            {
                if (hooklist[i].IsHook)
                {
                    UpdateDir(hooklist[i]);
                    Debug.DrawRay(cameraTr.position, hooklist[i].cameraToHitDir, Color.red);
                    Debug.DrawRay(hooklist[i].handObj.transform.position, hooklist[i].handToHitDir, Color.blue);
                    if (SetIkHand(i))
                    {
                        hooklist[i].spring.Update(Time.deltaTime);
                        Vector3 up = Quaternion.LookRotation(hooklist[i].handToHitDir.normalized) * Vector3.up;
                        hooklist[i].currentRopePosition = Vector3.Lerp(hooklist[i].currentRopePosition, 
                            hooklist[i].hitRay.point, Time.deltaTime * ropeSpeed);

                        for (int j = 0; j < lineCount + 1; j++)
                        {
                            float delta = j / (float)lineCount;
                            Vector3 offset = up * (waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * 
                                                   hooklist[i].spring.value * affectCurve.Evaluate(delta));

                            hooklist[i].lineRenderer.SetPosition(j, Vector3.Lerp(
                                hooklist[i].handObj.transform.position, hooklist[i].currentRopePosition, 
                                delta) + offset);
                        }
                    }

                }
            }
        }

        void ClearHook()
        {
            for (int i = 0; i < hooklist.Length; i++)
            {
                hooklist[i].lineRenderer.positionCount = 0;
                hooklist[i].IsHook = false;
                hooklist[i].IsRope = false;

                hooklist[i].spring.Reset();
                hooklist[i].currentRopePosition = hooklist[i].handObj.transform.position;
            }

        }

        void ClearEndHook()
        {
            for (int i = 0; i < hooklist.Length; i++)
            {
                if (hooklist[i].IsRope)
                {
                    hooklist[i].lineRenderer.positionCount = 0;
                    hooklist[i].IsHook = false;
                    hooklist[i].IsRope = false;

                    hooklist[i].spring.Reset();
                    hooklist[i].currentRopePosition = hooklist[i].handObj.transform.position;

                }
            }
        
            if (controller.animator.GetCurrentAnimatorStateInfo(0).IsName("RopeSwing"))
            {
                controller.animator.SetTrigger("GrapplinghookEnd");
            }
        }

        void HookMoveSetting()
        {
            SetIkHand();

            //(Gravity deletion)
            controller.player_rigidbody.useGravity = false;
        }

        void SettingReset()
        {
            stopFlag = false;
            startFlag = false;
            controller.player_rigidbody.useGravity = true;
        }
        private IEnumerator DashCoroutine(Vector3 direction, float movePower, float targetTime)
        {
            float DashTimer = 0;

            ClearEndHook();
            controller.animator.SetBool("Grounded", false);
            while (DashTimer < targetTime)
            {
                DashTimer += Time.deltaTime;

                controller.player_rigidbody.MovePosition(this.transform.position + direction * movePower / 2 * Time.deltaTime);

                yield return null;
            }
            SettingReset();
            StartCoroutine(ForwardMoveCoroutine(movePower));
        }

        IEnumerator ForwardMoveCoroutine(float movePower)
        {
            if (IsForwardMove) yield break;
            else IsForwardMove = true;

            while (!controller.isGround)
            {
                if (Physics.Raycast(this.transform.position + transform.up * 1f, this.transform.forward, 
                        0.3f, LayerMask.NameToLayer("Player")))
                {
                    break;
                }

                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
                controller.player_rigidbody.MovePosition(this.transform.position + this.transform.forward * 
                    movePower / 5 * Time.deltaTime);
                yield return null;
            }
            IsForwardMove = false;

        }

        IEnumerator HookPointMoveCoroutine()
        {
            Vector3 startpoint = transform.position;
            while (true)
            {
                Vector3 movePoint;

                if (hooklist[0].IsRope && hooklist[1].IsRope)
                {
                    movePoint = Vector3.Lerp(hooklist[0].hitRay.point, hooklist[1].hitRay.point, 0.5f);
                }
                else
                {
                    if (hooklist[0].IsRope)
                    {
                        movePoint = hooklist[0].hitRay.point;
                    }
                    else if (hooklist[1].IsRope)
                    {
                        movePoint = hooklist[1].hitRay.point;
                    }
                    else
                    {
                        SettingReset();
                        break;
                    }
                }

                HookMoveSetting();

                if (!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("RopeSwing"))
                {
                    controller.animator.SetTrigger("GrapplinghookStart");
                }

                controller.isGround = false;


                //(Calibration point)
                Vector3 correctionPoint = movePoint;

                correctionPoint.y = movePoint.y + controller.player_collider.height * 1.25f;

                if (Physics.CheckBox(correctionPoint, new Vector3(1, 1, 1), Quaternion.identity, 
                        LayerMask.NameToLayer("Player")))
                {
                    correctionPoint.y = movePoint.y - controller.player_collider.height;
                }
                else if (controller.player_collider.height < movePoint.y)
                {
                    correctionPoint.y = movePoint.y + controller.player_collider.height * 0.6f;
                    correctionPoint += (correctionPoint - startpoint).normalized * 0.9f;
                }
                else
                {
                    correctionPoint = movePoint;
                }

                float distance = 0.0f;

                // (Speed increase part)
                timer += Time.deltaTime;
                distance = Vector3.Distance(correctionPoint, this.transform.position);

                if (timer >= 0.3f)
                {
                    timer = 0;
                    if (currentHookPower < maxHookPower)
                    {
                        currentHookPower += 10;
                    }
                }
                if (distance < 2.0f)
                {
                    if (!hooklist[0].IsRope && !hooklist[1].IsRope)
                    {
                        if (currentHookPower > 1.0f)
                            currentHookPower = distance * distance * 5;
                    }
                }
                // (Find direction of movement)
                Vector3 correctionDir = correctionPoint - this.transform.position;
                playerToHitDir = Vector3.Lerp(this.transform.position, correctionPoint, 0.5f);
                Vector3 lerpDir = playerToHitDir - this.transform.position;

                // (Rotation and movement)
                this.transform.rotation = Quaternion.LookRotation(new Vector3(correctionDir.x, 0, correctionDir.z));
                controller.player_rigidbody.AddForce(lerpDir.normalized * (Time.deltaTime * currentHookPower) , ForceMode.Impulse);

                // (If there is a wall in front of the player while moving.)
                if (Physics.Raycast(this.transform.position + transform.up * 0.5f, this.transform.forward, 
                        out rayhit, 2.5f, LayerMask.NameToLayer("Player")))
                {
                    /*
                 
                    if (Input.GetKey(KeyCode.Space))
                    {
                    this.transform.rotation = Quaternion.LookRotation(-rayhit.normal);

                    controller.player_rigidbody.MovePosition(this.transform.position  -(rayhit.point - transform.position).normalized * 3.0f);
                    controller.moveState.ChangeState(MoveState.ClimbingMovement);

                        if (controller.isAnimator)
                        {
                            controller.animator.SetTrigger(controller.aniHashClimbingHook);
                            controller.animator.ResetTrigger(controller.aniHashClimbingEndJump);
                        }
                    endFlag = true;
                    break;
                    }

                 */

                    if (Vector3.Distance(this.transform.position, rayhit.point) < 0.6f)
                    {
                        endFlag = true;
                        break;
                    }

                }
                // Compared to the current location and the location to be moved.
                else if (distance < 1.0f || Vector3.Distance(this.transform.position, movePoint) < 0.75f)
                {
                    if (hooklist[0].IsRope && hooklist[1].IsRope)
                    {
                        Debug.Log(currentHookPower);
                        StartCoroutine(DashCoroutine(correctionDir, currentHookPower, 0.5f));
                        yield break;
                    }
                    else
                    {
                        StartCoroutine(DashCoroutine(correctionDir, 10f, 0.1f));
                    }

                    endFlag = true;
                    break;
                }

                if (stopFlag)
                {
                    endFlag = true;
                    break;
                }

                yield return null;
            }

            if (endFlag)
            {
                //done moving.
                ClearEndHook();
                SettingReset();
                controller.player_rigidbody.velocity = Vector3.zero;
                currentHookPower = 0;
                timer = 0;
            }

        }

        IEnumerator SlerpMoveCoroutine()
        {

            if (hooklist[0].IsRope && hooklist[1].IsRope)
            {
                SettingReset();
                yield break;
            }
            else if (hooklist[0].IsRope || hooklist[1].IsRope)
            {
                Hook hook;

                if (hooklist[0].IsRope)
                {
                    hook = hooklist[0];
                }
                else if (hooklist[1].IsRope)
                {
                    hook = hooklist[1];
                }
                else
                {
                    SettingReset();
                    yield break;
                }


                Vector3 startPos = this.transform.position + (hook.hitRay.point - transform.position) * 0.2f;
                Vector3 endPos = hook.hitRay.point + (hook.hitRay.point - transform.position) * 0.65f;
                controller.fallTime = 0.0f;

                Vector3 center = (startPos + endPos) * 0.5f;

                startPos = startPos - center;
                endPos = endPos - center;

                float speed = 0.0f;
                int count = 8;

                if (!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("RopeSwing"))
                {
                    controller.animator.SetTrigger("GrapplinghookStart");
                }

                controller.isGround = false;
                HookMoveSetting();

                while (true)
                {
                    Vector3 temp;

                    temp = Vector3.Slerp(startPos, endPos, count / maxCount);
                    temp += center;

                    Vector3 dir = temp - transform.position;

                    if (count > 20)
                        controller.GroundedCheck();

                    if (Physics.Raycast(this.transform.position + transform.up * 0.5f, 
                            this.transform.forward, out rayhit, 1.0f, LayerMask.NameToLayer("Player")) 
                        || controller.isGround)
                    {
                        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
                        endFlag = true;
                        break;
                    }

                    if (Vector3.Distance(temp, transform.position) < 3.0f)
                    {
                        if (count >= maxCount)
                        {
                            StartCoroutine(DashCoroutine(dir, speed, 0.15f));
                            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
                            yield break;
                        }
                        else
                        {
                            count+=5;
                        }
                    }
                    else if (speed < maxSpeed)
                    {
                        speed += 0.85f;
                    }

                    this.transform.rotation = Quaternion.LookRotation(dir);
                    controller.player_rigidbody.MovePosition(this.transform.position + dir.normalized * Time.deltaTime * speed);

                    if (stopFlag)
                    {
                        StartCoroutine(DashCoroutine(dir, speed, 0.15f));
                        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
                        endFlag = true;
                        break;
                    }

                    yield return null;
                }

                if (endFlag)
                {
                    ClearEndHook();
                    SettingReset();
                    controller.player_rigidbody.velocity = Vector3.zero;
                    currentHookPower = 0;
                    timer = 0;
                }
                yield return null;
            }
            else
            {
                startFlag = false;
            }
            yield return null;

        }
    }
}