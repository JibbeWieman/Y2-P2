using UnityEngine;

public class Drawer : MonoBehaviour
{
    private ConfigurableJoint joint;
    private float maxForwardLimit = 0.3f;

    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
    }

    void Update()
    {
        // Get the object's local position relative to the joint's connected body
        Vector3 localPosition = transform.localPosition;

        // Prevent backward movement by clamping the X position
        if (localPosition.x < 0)
        {
            localPosition.x = 0; // Stop backward movement
        }

        // Ensure it doesn't exceed the max forward limit
        localPosition.x = Mathf.Clamp(localPosition.x, 0, maxForwardLimit);

        // Set the target position of the joint to the clamped position
        JointDrive xDrive = joint.xDrive;
        xDrive.positionSpring = 500; // Adjust spring strength as needed
        xDrive.positionDamper = 10; // Adjust damper as needed
        joint.targetPosition = new Vector3(localPosition.x, 0, 0);
        joint.xDrive = xDrive;
    }
}
