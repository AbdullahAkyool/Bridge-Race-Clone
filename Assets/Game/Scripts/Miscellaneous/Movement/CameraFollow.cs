using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Movement
{
    enum CameraFollowPlane
    {
        XZ,
        XY,
        YZ
    }

    public class CameraFollow : MonoBehaviour
    {

        [SerializeField] private Transform targetTransform;

        [SerializeField] private CameraFollowPlane followPlane;

        private Vector3 starterOffset;

        Transform cameraTransform;
        Vector3 planeVector;

        private void Awake()
        {
            InitCamera();
            CalculateStartOffset();
            planeVector = GetPlaneVector();
        }

        private void InitCamera()
        {
            cameraTransform = Camera.main.transform;
        }

        private Vector3 GetPlaneVector()
        {
            if (followPlane.Equals(CameraFollowPlane.XZ))
            {
                return new Vector3(1, 0, 1);
            }
            else if (followPlane.Equals(CameraFollowPlane.XY))
            {
                return new Vector3(1, 1, 0); ;
            }
            else if (followPlane.Equals(CameraFollowPlane.YZ))
            {
                return new Vector3(0, 1, 1);
            }
            else return Vector3.zero;
        }

        private void CalculateStartOffset()
        {
            starterOffset = targetTransform.position - cameraTransform.position;
        }

        private void LateUpdate()
        {
            Vector3 offsettedPos = targetTransform.position - starterOffset;
            offsettedPos = new Vector3(-starterOffset.x + targetTransform.position.x * planeVector.x, -starterOffset.y + targetTransform.position.y * planeVector.y, -starterOffset.z + targetTransform.position.z * planeVector.z);
            cameraTransform.position = offsettedPos;
        }

    }
}


