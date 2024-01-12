using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public UI ui;
    private Rigidbody rb;
    private Collider collide;
    public GameObject player;
    private float velocityd;
    private float timer = 0.5f;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        collide = transform.GetComponent<Collider>();

    }

    void Update()
    {
        velocityd = rb.velocity.magnitude;
        if (velocityd == 0 && ui.player.ballflying)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                // respawns player
                timer = 0.5f;
                Debug.Log("stopped");
                ui.player.privatespecialpunch = false;
                ui.player.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 5);
                ui.player.ballflying = false;
                ui.player.ui.unactiveShot();
                ui.player.swipesBlocked = false;
                ui.player.targetdot.SetParent(ui.player.objectToRotate.transform);
                ui.player.targetdot.localPosition = new Vector3(0, 0, 3);
                ui.arrows.SetActive(true);
                ui.player.skill.SetActive(false);
                if (!ui.result && ui.currentshots >= ui.shots)
                {
                    ui.lose();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Circle")
        {
            ui.howmanycircles -= 1;
            other.gameObject.GetComponent<CircleController>().enabled = true;
        }
        if (other.gameObject.tag == "Hole" && ui.howmanycircles == 0)
        {
            rb.velocity = Vector3.zero;
            collide.isTrigger = true;
            ui.win();
        }
    }
}
