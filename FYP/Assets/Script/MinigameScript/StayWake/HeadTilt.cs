using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTilt : MonoBehaviour
{

    public float forceAmount = 10f;
    public float fallAmount = 10f;
    private Rigidbody2D rb;

    public float fallThreshold = 15f;
    public float maxFallingTorque;
    public float minFallingTorque;

    public float rotationZ;
    public float fallTorque;

    public float maxLeanAngle;

    public int startTorque;
    int[] dir;

    [Header("Sprites")]
    //HeadReference
    [SerializeField] Sprite Eyeclosed;
    [SerializeField] Sprite EyeOpen;
    bool awake;
    [SerializeField] float awakeTime;
    float _awakeTime;

    SpriteRenderer sr;

    void Start()
    {
        _awakeTime = awakeTime;
        
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = Eyeclosed;

        dir = new int[] {-startTorque, startTorque };
        rb = GetComponent<Rigidbody2D>();
        int i = Random.Range(0, dir.Length);
        rb.AddTorque(dir[i]);
    }

    void Update()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            rb.AddTorque(forceAmount * CheckMouseSide());
            awakeTime = _awakeTime;
            awake = true;
            Debug.Log("Clicked");
        }
        if (awakeTime > 0 && awake)
        {
            sr.sprite = EyeOpen;
            awakeTime -= Time.deltaTime;
        }
        else
        {
            awake = false;
            sr.sprite = Eyeclosed;
        }

        //rb.AddTorque(-Mathf.Sign(transform.rotation.eulerAngles.z) * fallAmount*0.05f);
    }

    void ApplyFallingBehavior(float rotationZ)
    {
        // Calculate the falling torque based on the rotation angle
        fallTorque = Mathf.Lerp(minFallingTorque, maxFallingTorque, (Mathf.Abs(rotationZ) - fallThreshold) / (maxLeanAngle - fallThreshold));
        rb.AddTorque(Mathf.Sign(rotationZ) * fallTorque);
    }

    private void FixedUpdate()
    {
        rotationZ = transform.rotation.eulerAngles.z;
        if (rotationZ > 180)
            rotationZ -= 360;

        if (Mathf.Abs(rotationZ) > fallThreshold)
        {
            ApplyFallingBehavior(rotationZ);
        }

        //Clamp rotation
        //rotationZ = Mathf.Clamp(rotationZ, -maxLeanAngle, maxLeanAngle);
        //transform.rotation = Quaternion.Euler(0, 0, rotationZ);

    }


    float CheckMouseSide()
    {
        
        if(Input.mousePosition.x > Screen.width/2)
        {
            Debug.Log("Right");
            return 1;
        }
        else
        {
            Debug.Log("Left");
            return -1;
        }
    }
}
