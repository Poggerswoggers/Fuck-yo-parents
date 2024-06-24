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

    public List<Transform> targets;
    int index;
    Transform currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        currentTarget = targets[0];
    }

    // Update is called once per frame
    void Update()
    {
        SetInput();
        ChangeTarget();
        Moving();
        Traction();
        ApplySteering();

        moveX = TurnTowardsTarget();
    }

    void SetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TurnTowardsTarget();
        }
    }
    float TurnTowardsTarget()
    {
        Vector3 vectorToTarget = currentTarget.position - transform.position;
        vectorToTarget.Normalize();

        float angleToTarget = Vector3.SignedAngle(-transform.up, vectorToTarget, Vector3.forward);


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
        Vector3 _rotateAngle = Vector3.forward * moveX * MoveForce.magnitude * steeringAngle; //turnAssist;
        rotateAngle = _rotateAngle * Time.deltaTime;
        transform.Rotate(rotateAngle);

    }
    void ChangeTarget()
    {
        if(Vector2.Distance(transform.position, currentTarget.position)< 2f)
        {
            if (index<targets.Count-1)
            {
                index++;
                currentTarget = targets[index];
            }
            else
            {
                moveX = 0f;
                moveY = 0f;
            }
        }
    }
    

}
