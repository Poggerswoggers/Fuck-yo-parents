using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PmdController : MonoBehaviour
{
    bool canDrive = true;

    private Vector3 MoveForce;
    [SerializeField] float _moveSpeed;
    private float moveSpeed;
    public float traction = 1;

    [SerializeField] float moveX;
    [SerializeField] float moveY;

    [SerializeField] float steeringAngle = 20f;
    [SerializeField] Vector3 rotateAngle;

    [SerializeField] Transform targetArea;

    //origin
    Vector3 originPos;


    //Reference
    [SerializeField] PrecisionSlider slider;
    [SerializeField] ThatsTheSpot minigameRef;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = _moveSpeed;
        originPos = transform.position;
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
        if (Input.GetMouseButtonDown(0) && canDrive)
        {
            canDrive = false;
            moveY =1;
            moveX = TurnTowardsTarget();
        }
    }
    float TurnTowardsTarget()
    {
        Vector3 vectorToTarget = targetArea.position - transform.position;
        vectorToTarget.Normalize();

        float angleToTarget = Vector3.SignedAngle(-transform.up, vectorToTarget, Vector3.forward);

        if(Mathf.Abs(slider.GetSliderPoint()-0.5f) > 0.3f)
        {
            steeringAngle *= 0.55f;
        }
        else if(Mathf.Abs(slider.GetSliderPoint() - 0.5f) > 0.15f)
        {
            steeringAngle *= 0.7f;
        }
        else if (Mathf.Abs(slider.GetSliderPoint() - 0.5f) > 0.08f)
        {
            steeringAngle *= 0.85f;
        }
        Debug.Log(Mathf.Abs(slider.GetSliderPoint()));

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

            if(moveY>0.01f && moveY < 0.8f)
            {
                minigameRef.GetArea(targetArea.transform, this.transform);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveX = 0;
        moveY = 0;

        minigameRef.GetArea(targetArea.transform, this.transform);
    }

    public void ResetAttempt()
    {
        canDrive = true;
        steeringAngle = 45;
        transform.position = originPos;
        transform.rotation = Quaternion.identity;
        moveSpeed = _moveSpeed;
        slider.ResetSlider();
    }
}
