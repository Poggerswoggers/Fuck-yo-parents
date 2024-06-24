using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PmdController : MonoBehaviour
{
    private Vector3 MoveForce;
    public float moveSpeed;
    public float traction = 1;

    [SerializeField] float moveX;
    [SerializeField] float moveY;

    [SerializeField] float steeringAngle = 20f;
    [SerializeField] Vector3 rotateAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        Traction();
        ApplySteering();
    }

    public void Moving()
    {
        MoveForce += -transform.up * moveSpeed * moveY * Time.deltaTime;
        transform.position += MoveForce * Time.deltaTime;
    }

    public void Traction()
    {
        Debug.DrawRay(transform.position, MoveForce.normalized * 3f, Color.blue);
        Debug.DrawRay(transform.position, -transform.up * 3f, Color.black);
        MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, traction * Time.deltaTime) * MoveForce.magnitude;
    }

    public void ApplySteering()
    {
        Vector3 _rotateAngle = Vector3.forward * moveX * MoveForce.magnitude * steeringAngle; //turnAssist;
        rotateAngle = _rotateAngle * Time.deltaTime;
        transform.Rotate(rotateAngle);

    }

}
