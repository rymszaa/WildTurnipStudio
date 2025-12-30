using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public Grapple grapple;

    private Quaternion desiredRotation;
    private float rotateSpeed = 5f;
    
    void Update()
    {
        if (!grapple.IsGrappling())
        {
            desiredRotation = transform.parent.rotation;
        }
        else
        {
            desiredRotation = Quaternion.LookRotation(grapple.GetGrapplePoint() - transform.position);
        }
        

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotateSpeed * Time.deltaTime);
    }
}
