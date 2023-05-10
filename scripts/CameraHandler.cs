using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SG {
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;

        public Transform targetToGazeAt;
        public Transform thisPlayerTransform;
        public Transform fistPlayerTransform;
        public Transform secondPlayerTransform;
        //public Transform playerToGazeAt;

        public float maxDistance = 10f;
        public float scalingFactor = 80f;

        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        private LayerMask ignoreLayers;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        //public static CameraHandler singleton;


        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        public float cameraShpereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;

        private void Awake() {
            //singleton = this;
            //thisPlayerTransform = this.transform.parent.GetComponent<Transform>();
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 9 | 1 << 10); // the tilde symbol is used in order to perform the bitwise not operation
                                                // this expression is needed in order to perform collisions between objects
                                                // and camera, with this expression we ignore layers 9 and 10
        }

        public void FollowTarget(float delta) {
            //interpolation between the position of this game object and the target transform
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, 
                                                        ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition;

            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, 
            float mouseYInput, bool cameraLockFlag) {

            if (!cameraLockFlag)
            {
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else {
                try 
                {
                    thisPlayerTransform = this.transform.parent.transform.Find("Player");
                    fistPlayerTransform = GameObject.Find("player_holder").transform.Find("Player");
                    secondPlayerTransform = GameObject.Find("player_holder(Clone)").transform.Find("Player");

                    if (thisPlayerTransform.Equals(fistPlayerTransform))
                    {
                        targetToGazeAt = secondPlayerTransform;
                 
                    } else {
                        targetToGazeAt = fistPlayerTransform;
                    }
                }
                catch (Exception e) {
                    e = null;
                }
                Vector3 camToTarget = targetToGazeAt.position - myTransform.position;
                camToTarget.y = 0;

                Quaternion rotationToPerform = Quaternion.LookRotation(camToTarget, Vector3.up);
                myTransform.rotation = rotationToPerform;

                float targetsDistance = Vector3.Distance(targetToGazeAt.position, targetTransform.position);

                pivotAngle = (1 / (targetsDistance + float.Epsilon)) * scalingFactor;
                
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);
                Vector3 rotation = Vector3.zero;
                rotation.x = pivotAngle;

                Quaternion pivotRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = pivotRotation;

            }
        }

        public void HandleCameraCollision(float delta) {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            bool spherecastValue = Physics.SphereCast(cameraPivotTransform.position, cameraShpereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers);

            

            if (spherecastValue) {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset) {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }
    }
}

