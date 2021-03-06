﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // UI tingz
    public Slider healthSlider;
    public Slider flowSlider;
    public Text flowText;

    public Texture2D cursor;


    public static string era = "pre";

    public float cd = 0.7f;
    int onCD = 0;
    float time = 0;
    float timeint = 0;
    public int damage = 5;
    public static GameObject player;
    public static Vector2 target;
    public static EnemyController attacking;
    private Vector3 position;
    public static GameObject tower;
    public static GameObject basebuilding;
    public static int moving;
    private bool superHealing;
    public static int health;
    public static int flow;
    public static string mode = "Slashy";
    public static float speed;
    public static int flowregen;

    Animator animator;
    bool isLookingRight = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void Start()
    {
        player = gameObject;
        speed = 2.0f;
        time = Time.time;
        moving = 0;
        superHealing = false;

        flowregen = 1;
        health = 100;
        flow = 25;

        StartCoroutine(regenhp());
        StartCoroutine(regenflow());
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        flowSlider = GameObject.Find("FlowSlider").GetComponent<Slider>();
        flowText = GameObject.Find("FlowText").GetComponent<Text>();

        healthSlider.value = health;
        flowSlider.value = flow;
    }

    void Update()
    {
        if (health <= 0)
        {
            //UIController.LoseUI();
        }

        // AWSD CONTROLS
        Vector2 move = Vector2.zero;

        if (Input.GetKey("a"))
        {
            move.x -= 1f;
        }
        if (Input.GetKey("d"))
        {
            move.x += 1f;
        }
        if (Input.GetKey("w"))
        {
            move.y += 1f;
        }
        if (Input.GetKey("s"))
        {
            move.y -= 1f;
        }

        if (moving == 0)
        {
            this.transform.Translate(move * speed * Time.deltaTime);
        }
        else if (moving == 1)
        {
            target += move * speed;
        }


        if (era == "pre")
        {
            if (Input.GetKeyDown("e"))
            {
                if (flow >= 10)
                {
                    mode = "Build";
                    tower = (GameObject)Resources.Load("Drum");
                }

            }
            if (Input.GetKeyDown("q"))
            {
                if (flow >= 20)
                {
                    mode = "Build";
                    tower = (GameObject)Resources.Load("Flute");
                }

            }
        }

        else if (era == "classical")
        {
            if (Input.GetKeyDown("e"))
            {
                if (flow >= 15)
                {
                    mode = "Build";
                    tower = (GameObject)Resources.Load("Harp");
                }

            }
            if (Input.GetKeyDown("q"))
            {
                if (flow >= 20)
                {
                    mode = "Build";
                    tower = (GameObject)Resources.Load("Trumpet");
                }

            }
            if (Input.GetKeyDown("r"))
            {
                if (flow >= 30)
                {
                    mode = "Build";
                    tower = (GameObject)Resources.Load("Violin");
                }
            }
            if (Input.GetKeyDown("f"))
            {
                if (flow >= 50)
                {
                    mode = "Build";
                    tower = (GameObject)Resources.Load("Piano");
                }
            }
        }


        else if (era == "modern")
        {
            if (Input.GetKeyDown("e"))
            {
                if (flow >= 10)
                {
                    mode = "Build";
                    tower = (GameObject)Resources.Load("Drumset");
                }

            }
            if (Input.GetKeyDown("q"))
            {
                if (flow >= 20)
                {
                    mode = "Build";
                    tower = (GameObject)Resources.Load("Rapper");
                }

            }
            if (Input.GetKeyDown("r"))
            {
                if (flow >= 30)
                {
                    mode = "Build";
                    tower = (GameObject)Resources.Load("Guitar");
                }
            }
            if (Input.GetKeyDown("f"))
            {
                if (flow >= 50)
                {
                    mode = "Build";
                    tower = (GameObject)Resources.Load("Synthesizer");
                }
            }
            if (Input.GetKeyDown("t"))
            {
                if (flow >= 40)
                {
                    mode = "Build";
                    basebuilding = (GameObject)Resources.Load("Gramophone");
                }
            }

        }

        if (moving == 2)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        }

        if (moving == 1)
        {
            if (attacking != null)
            {
                target = attacking.GetComponent<Transform>().position;
            }
            Vector2 pos = Camera.main.ScreenToWorldPoint(target);
            if (attacking != null)
            {
                pos = attacking.GetComponent<Transform>().position;
            }
            if (Vector2.Distance(this.transform.position, pos) < 1f && attacking != null)
            {

                timeint = Time.time - time;
                if (onCD == 0)
                {
                    if (isLookingRight)
                    {
                        //look right
                        animator.SetInteger("AnimState", 0);
                    }
                    else
                    {
                        //look left
                        animator.SetInteger("AnimState", 2);
                    }
                }
                if (onCD == 1)
                {
                    moving = 0;
                    if (timeint > cd)
                    {
                        onCD = 0;
                    }
                }
                else
                {
                    //attack right
                    if (isLookingRight)
                    {
                        animator.SetTrigger("Attack");
                    }
                    //attack left
                    else
                    {
                        animator.SetTrigger("Attack");
                    }
                    moving = 0;
                    attacking.hp -= damage;
                    PlayerController.flow += 1;
                    onCD = 1;
                    time = Time.time;
                }
            }
            else
            {
                if (transform.position.x > pos.x)
                {
                    //look left
                    animator.SetInteger("AnimState", 2);
                    isLookingRight = false;
                }
                if (transform.position.x < pos.x)
                {
                    //look right
                    animator.SetInteger("AnimState", 0);
                    isLookingRight = true;
                }
                this.transform.position = Vector2.MoveTowards(this.transform.position, pos, speed * Time.deltaTime);
            }

        }
        if (mode == "Slashy")
        {

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                moving = 1;
                if (attacking != null)
                {
                    if (Vector2.Distance(attacking.GetComponent<Transform>().position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) > 2f)
                    {
                        attacking = null;
                    }
                }
                target = Input.mousePosition;

            }
        }

        bool baseHealRange = Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Base").transform.position) < 4f;
        if (baseHealRange && !superHealing)
        {
            superHealing = true;
            StartCoroutine("regenhpBase");
        }
        if (!baseHealRange)
        {
            StopCoroutine("regenhpBase");
            superHealing = false;
        }

        else if (mode == "Build" || mode == "Ability")
        {
            if (Input.GetMouseButton(0))
            { }

            else if (Input.GetMouseButton(1))
            {
                mode = "Slashy";
                moving = 1;
                target = Input.mousePosition;
            }
        }
        healthSlider.value = health;
        flowSlider.value = flow;
        flowText.text = flow.ToString();
    }

    public bool isClose()
    {
        if (Vector2.Distance(this.transform.position, target) < 6f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator regenhp()
    {
        while (health >= 0)
        {
            yield return new WaitForSeconds(2);
            if (health < 100)
            {
                health++;
            }
        }
    }

    IEnumerator regenhpBase()
    {
        while (health >= 0)
        {
            yield return new WaitForSeconds(1);
            if (health < 99)
            {
                print("SUPAHEAL");
                health += 2;
            }
        }
    }

    IEnumerator regenflow()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            if (flow < 100)
            {
                flow+=flowregen;
            }
        }
    }


    public void drumbutton()
    {
        if (flow >= 10)
        {
            mode = "Build";
            tower = (GameObject)Resources.Load("Drum");
        }
    }

    public void flutebutton()
    {
        if (flow >= 20)
        {
            mode = "Build";
            tower = (GameObject)Resources.Load("Flute");
        }
    }



    public void harpbutton()
    {
        if (flow >= 15)
        {
            mode = "Build";
            tower = (GameObject)Resources.Load("Harp");
        }
    }

    public void trumpetbutton()
    {
        if (flow >= 20)
        {
            mode = "Build";
            tower = (GameObject)Resources.Load("Trumpet");
        }
    }

    public void violinbutton()
    {
        if (flow >= 30)
        {
            mode = "Build";
            tower = (GameObject)Resources.Load("Violin");
        }
    }

    public void pianobutton()
    {
        if (flow >= 50)
        {
            mode = "Build";
            tower = (GameObject)Resources.Load("Piano");
        }
    }

    public void rapperbutton()
    {
        if (flow >= 20)
        {
            mode = "Build";
            tower = (GameObject)Resources.Load("Rapper");
        }
    }

    public void drumsetbutton()
    {
        if (flow >= 10)
        {
            mode = "Build";
            tower = (GameObject)Resources.Load("Drumset");
        }
    }

    public void guitarbutton()
    {
        if (flow >= 30)
        {
            mode = "Build";
            tower = (GameObject)Resources.Load("Guitar");
        }
    }

    public void synthesizerbutton()
    {
        if (flow >= 50)
        {
            mode = "Build";
            tower = (GameObject)Resources.Load("Synthesizer");
        }
    }

    public void gramophonebutton()
    {
        if (flow >= 40)
        {
            mode = "Build";
            basebuilding = (GameObject)Resources.Load("Gramophone");
        }
    }
}
