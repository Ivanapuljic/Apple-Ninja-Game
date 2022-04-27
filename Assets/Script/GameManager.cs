using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }

    private const float REQUIRED_SLICE_FORCE = 400.0f;
    private List<Apple> apples = new List<Apple>();
    public GameObject applePrefab;

    private float lastSpawn;
    private float lastSpawn1;

    private float deltaSpawn = 1.0f;

    public Transform trail;

    private bool isPaused;

    private Vector3 lastMousePos;
    private Collider2D[] appleCols;

    //UI part of the game
    private int score;
    private int highscore;
    private int lifepoint;
    public Text scoreText;
    public Text highscoreText;
    public Image[] lifePoints;
    public GameObject pauseMenu;
    public GameObject deathMenu;

    //bomb
    private List<Bomb> bombs = new List<Bomb>();
    public GameObject bombPrefab;
    private Collider2D[] bombCals;


    private Bomb GetBomb()
    {
        Bomb b = bombs.Find(x => !x.IsActive);
        if(b== null)
        {
            b = Instantiate(bombPrefab).GetComponent<Bomb>();
            bombs.Add(b);
        }
        return b;
    }

    //

    private void Awake()
    {
        Instance = this;
    }



    private void Start()
    {
        appleCols = new Collider2D[0];
        bombCals = new Collider2D[0];
        NewGame();

    }
    public void NewGame()     
    {
        
        score = 0;
        lifepoint = 3;
        pauseMenu.SetActive(false);
        scoreText.text = score.ToString();
        highscore = PlayerPrefs.GetInt("Score");
        highscoreText.text = "BEST:" + highscore.ToString();
        Time.timeScale = 1;
        isPaused = false;

        foreach (Image i in lifePoints)
            i.enabled = true;

        foreach (Apple a in apples)
            Destroy(a.gameObject);
            apples.Clear();

        foreach (Bomb b in bombs)
            Destroy(b.gameObject);
        bombs.Clear();



        deathMenu.SetActive(false);
        

    }




    private void Update()
    {
        if (isPaused)
            return;

        if(Time.time - lastSpawn > deltaSpawn)
        {
            Apple a = GetApple();
            float randomX = Random.Range(-1.65f, 1.65f);

            a.LaunchApple(Random.Range(1.85f, 2.75f), randomX, -randomX);
            lastSpawn = Time.time;
        }

        if(Time.time-lastSpawn1  > 2.0f)
        {
        
            Bomb b = GetBomb();
            b.LaunchBomb(Random.Range(1.85f, 2.75f), Random.Range(1.5f, 1.5f), -Random.Range(1.5f, 1.5f));
            lastSpawn1 = Time.time;
            
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 pos =Camera.main.ScreenToWorldPoint(Input.mousePosition);

            pos.z = -1;
            trail.position = pos;
            Collider2D[] thisFramesApple = Physics2D.OverlapPointAll(new Vector2(pos.x, pos.y), LayerMask.GetMask("Apple"));
            //bomb
            Collider2D[] thisFramesBomb = Physics2D.OverlapPointAll(new Vector2(pos.x, pos.y), LayerMask.GetMask("Bomb"));
            //
            //Debug.Log((Input.mousePosition - lastMousePos).sqrMagnitude);

            if ((Input.mousePosition - lastMousePos).sqrMagnitude > REQUIRED_SLICE_FORCE)
                //bomb
                foreach (Collider2D c2 in thisFramesBomb)
                {
                    for(int i=0; i<bombCals.Length; i++)
                    {
                        if(c2== bombCals[i])
                        {
                            c2.GetComponent<Bomb>().SliceBomb();
                        }
                    }
                }
            bombCals = thisFramesBomb;
             //
            foreach (Collider2D c2 in thisFramesApple)
            {
                for(int i = 0; i<appleCols.Length; i++)
                {
                    if(c2 == appleCols[i])
                    {
                            c2.GetComponent<Apple>().Slice();
                    }
                }
            }
            lastMousePos = Input.mousePosition;
            appleCols = thisFramesApple;
        }

    }

    private Apple GetApple()
    {
        Apple a = apples.Find(x => !x.IsActive);

        if (a== null)
        {
            a = Instantiate(applePrefab).GetComponent<Apple>();
            apples.Add(a);
        }
        return a;
    }

    public void IncrementScore(int scoreAmount)
    {
        score += scoreAmount;
        scoreText.text = score.ToString();

        if(score > highscore)
        {
            highscore = score;
            highscoreText.text = "BEST:" + highscore.ToString();
            PlayerPrefs.SetInt("Score", highscore);
        }
    }

    //Lose life
    public void LoseLP()
    {
        if (lifepoint == 0)
            return;

        lifepoint--;
        lifePoints[lifepoint].enabled = false;
        if(lifepoint ==0)
             Death();
        
    }

    //Lose all lives
    public void Death()
    {
        isPaused = true;
        deathMenu.SetActive(true);
        SoundManager.Instance.PlaySound(2);
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;

        //
    }

    public void PauseGame()
    {
        //sound
        SoundManager.Instance.PlaySound(1);
        //
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        isPaused = pauseMenu.activeSelf;
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
    }
    public void ToMenu()
    {
        
            SceneManager.LoadScene("Menu");
        
    }
}
