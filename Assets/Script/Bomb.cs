using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private const float GRAVITY = 2.0f;
    public bool IsActive { set; get; }
    public SpriteRenderer sRenderer;
    private float verticalVelocity;
    private float speed;

    private float rotationSpeed;


    //private void Start()
    //{
    //  LaunchBomb(2.0f, 1, -1);
    //}

    public void LaunchBomb(float verticalVelocity, float xSpeed, float xStart)
    {
        IsActive = true;
        speed = xSpeed;
        this.verticalVelocity = verticalVelocity;
        transform.position = new Vector3(xStart, 0, 0);
        rotationSpeed = Random.Range(-180, 180);


    }
    private void Update()
    {
        if (!IsActive)
            return;

        verticalVelocity -= GRAVITY * Time.deltaTime;
        transform.position += new Vector3(speed, verticalVelocity, 0) * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);


    }
    public void SliceBomb()
    {
        GameManager.Instance.Death();

    }
}