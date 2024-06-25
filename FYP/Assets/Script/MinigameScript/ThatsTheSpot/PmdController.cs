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

    [SerializeField] PrecisionSlider slider;

    public Transform targets;
    bool arrived;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        SetInput();
        Moving();
        Traction();
        ApplySteering();

    }

    void SetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            moveY =1;
            moveX = TurnTowardsTarget();
        }
    }
    float TurnTowardsTarget()
    {
        Vector3 vectorToTarget = targets.position - transform.position;
        vectorToTarget.Normalize();

        float angleToTarget = Vector3.SignedAngle(-transform.up, vectorToTarget, Vector3.forward);

        if(Mathf.Abs(slider.GetSliderPoint()-0.5f) > 0.1f)
        {
            steeringAngle *= 0.5f;
        }

        float steerAmount = angleToTarget / 20f;
        steerAmount = Mathf.Clamp(steerAmount, -1f, 1f);
        return steerAmount;
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
        Vector3 _rotateAngle = Vector3.forward * moveX * steeringAngle; //turnAssist;
        rotateAngle = _rotateAngle * Time.deltaTime;
        transform.Rotate(rotateAngle);

    }
    

    private void OnTriggerStay2D(Collider2D other)
    {   
        if(other.tag == "Target")
        {
            moveX = (moveX > 0) ? moveX -= Time.deltaTime * 0.9f : 0;
            moveY = (moveY > 0) ? moveY -= Time.deltaTime * 0.5f : 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveY = 0f;
    }
}
