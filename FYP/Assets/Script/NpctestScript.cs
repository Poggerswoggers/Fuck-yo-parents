using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpctestScript : MonoBehaviour
{
    // Properties
    private float x;
    private float y;
    public float speed;
    private float direction;
    private Vector2 vel;
    private bool isWalking;
    private bool loop;

    // Update is called once per frame
    private void Start()
    {
        StartWalking();
    }

    void Update()
    {
        UpdateMovement();

    }

    void UpdateMovement()
    {
        // Calculate walking velocity
        float vx, vy;
        if (isWalking)
        {
            vx = Mathf.Cos(direction) * speed;
            vy = Mathf.Sin(direction) * speed;
        }
        else
        {
            vx = vy = 0; // stand still
        }

        // Move
        vel = Vector2.Lerp(vel, new Vector2(vx, vy), 0.1f);
        x += vel.x * Time.deltaTime;
        y += vel.y * Time.deltaTime;

        // Wrap around screen
        if (loop)
        {
            float margin = 50;
            if (x < -margin) x = Screen.width + margin;
            if (x > Screen.width + margin) x = -margin;
            if (y < 0) y = Screen.height + margin * 2;
            if (y > Screen.height + margin * 2) y = 0;
        }

        // Update object position
        transform.position = new Vector3(x, y, transform.position.z);
    }

    public void StartWalking()
    {
        isWalking = true;
        speed = 1 + Random.value * 0.5f;
        direction = Random.value * Mathf.PI * 2;
    }

    public void StopWalking(bool halt)
    {
        if (halt)
        {
            vel = Vector2.zero; // Stop completely
        }
        isWalking = false;
    }
}

