using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Apple : MonoBehaviour
{
    private const float GRAVITY = 2.0f;
    public bool IsActive{ set; get; }
    public SpriteRenderer sRenderer;

    private float verticalVelocity; //gravity
    private float speed; //positive:right negative:left
    private bool isSliced = false;

    public Sprite[] sprites;
    private int spriteIndex;
    private float lastSpriteUpdate;
    private float spriteUpdateDelta = 0.125f;
    private float rotationSpeed;

   // private void Start()
   //{
   //    LaunchApple(2.0f, 1, -1);
   // sRenderer = GetComponent<SpriteRenderer>();

    //}
    public void LaunchApple(float verticalVelocity, float xSpeed, float xStart)
    {
        IsActive = true;
        speed = xSpeed;
        this.verticalVelocity = verticalVelocity;
        transform.position = new Vector3(xStart, 0, 0);
        rotationSpeed = Random.Range(-180,180);
        isSliced = false;
        spriteIndex = 0;
        sRenderer.sprite = sprites[spriteIndex];
    }
    private void Update()
    {
        if (!IsActive)
            return;

        verticalVelocity -= GRAVITY * Time.deltaTime;
        transform.position += new Vector3(speed, verticalVelocity, 0) * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);


        if (isSliced)
        {
            if(spriteIndex != sprites.Length-1 && Time.time - lastSpriteUpdate > spriteUpdateDelta)
            {
                lastSpriteUpdate = Time.time;
                spriteIndex++;
               sRenderer.sprite = sprites[spriteIndex];

            }
        }


        if(transform.position.y < -1) //we dont see apple
        {
            IsActive = false;
            if (!isSliced)
                GameManager.Instance.LoseLP();
        }
           
    }

    public void Slice()
    {
        if (isSliced)
            return;
        if (verticalVelocity < 0.5f)
            verticalVelocity = 0.5f;

        speed = speed * 0.5f;
        isSliced = true;
        //sound
        SoundManager.Instance.PlaySound(0);
        //
        GameManager.Instance.IncrementScore(1);
    }

  


}
