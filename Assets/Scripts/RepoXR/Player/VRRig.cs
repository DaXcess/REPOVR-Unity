using UnityEngine;

namespace RepoXR.Player
{
    public class VRRig : MonoBehaviour
    {
        public MeshRenderer[] meshes;

        [Header("Tracking")]
        public Transform head;
        public Transform leftArm;
        public Transform rightArm;
        public Transform leftArmTarget;
        public Transform rightArmTarget;
        public Transform leftHandAnchor;
        public Transform rightHandAnchor;
        public Transform leftHandTip;
        public Transform rightHandTip;

        [Header("Stuff idk think of something")]
        public Transform headAnchor;
        public Transform planeOffsetTransform;
        public RectTransform infoHud;
        public Transform inventory;
        public Transform map;
        public Transform headLamp;

        public Collider leftHandCollider;
        public Collider rightHandCollider;
        public Collider mapPickupCollider;
        public Collider lampTriggerCollider;

        public VRInventory inventoryController;
        
        [Header("Offsets")] 
        public Vector3 headOffset;

        [Space(10)]
        public Vector3 mapRightPosition;
        public Vector3 mapLeftPosition;

        [Space(10)]
        public Vector3 normalPlaneOffset;
        public Vector3 gazePlaneOffset;
        
        private Transform leftArmMesh;
        private Transform rightArmMesh;

        private void Awake()
        {
            leftArmMesh = leftArm.GetComponentInChildren<MeshRenderer>().transform;
            rightArmMesh = rightArm.GetComponentInChildren<MeshRenderer>().transform;
        }

        private void LateUpdate()
        {
            transform.position = head.position + headOffset;
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.Euler(transform.eulerAngles.x, head.eulerAngles.y, transform.eulerAngles.z),
                10f * Time.deltaTime);

            headAnchor.position = head.position;
            
            UpdateArms();
        }

        private void UpdateArms()
        {
            leftArm.localPosition = new Vector3(leftArm.localPosition.x, leftArm.localPosition.y, 0);
            rightArm.localPosition = new Vector3(rightArm.localPosition.x, rightArm.localPosition.y, 0);

            leftArm.LookAt(leftArmTarget.position);
            rightArm.LookAt(rightArmTarget.position);

            var maxDistanceLeft = leftHandAnchor.localPosition.z;
            var maxDistanceRight = rightHandAnchor.localPosition.z;

            if (Vector3.Distance(leftArm.position, leftArmTarget.position) is var leftDistance &&
                leftDistance < maxDistanceLeft)
            {
                leftArm.localPosition += Vector3.back * (maxDistanceLeft - leftDistance);
                leftArm.LookAt(leftArmTarget.position);
            }

            if (Vector3.Distance(rightArm.position, rightArmTarget.position) is var rightDistance &&
                rightDistance < maxDistanceRight)
            {
                rightArm.localPosition += Vector3.back * (maxDistanceRight - rightDistance);
                rightArm.LookAt(rightArmTarget.position);
            }

            leftHandTip.rotation = leftArmTarget.rotation;
            rightHandTip.rotation = rightArmTarget.rotation;

            // Debug
            Debug.DrawRay(leftHandTip.position, leftHandTip.forward * 5, Color.blue);
            Debug.DrawRay(rightHandTip.position, rightHandTip.forward * 5, Color.blue);
        }
    }
}