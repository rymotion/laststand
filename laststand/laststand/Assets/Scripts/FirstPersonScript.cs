﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonScript : MonoBehaviour { //this script should be attached to the perspective camera
    public float reloadTime = 1f;
    public bool reloaded = true;
    public UnityEngine.UI.Text AmmoVal;
    public UnityEngine.UI.Text Health;
    public float horizontalSpeed = 10.0F;
    public float verticalSpeed = 10.0F;
    public GameObject gameOverCanvas;
    public GameObject reticle;
    public GameObject reloadReticle;
    private int health = 100;

    private void Start()
    {
        Health.text = health.ToString();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            transform.Rotate(h * Vector3.up, Space.World);

            float v = verticalSpeed * Input.GetAxis("Mouse Y");
            transform.Rotate(-v * Vector3.right, Space.Self);
            if (Input.GetMouseButtonDown(0) && reloaded)
            {
                RaycastHit hitInfo;
                AmmoVal.text = "0"; //Since our gun only shoots once we can just directly set it to 0 when fired. We can easily modify this later if we want for bigger clips.
                StartCoroutine(Reload());
                if (Physics.Raycast(transform.position, transform.forward, out hitInfo))
                    if (hitInfo.transform.CompareTag("Enemy"))
                        hitInfo.transform.GetComponent<EnemyScript>().TakeDamage(20);
            }
        }
    }

    IEnumerator Reload()
    {
        reloaded = false;
        reloadReticle.SetActive(true);
        reticle.SetActive(false);
        yield return new WaitForSeconds(reloadTime);
        AmmoVal.text = "1";
        reloaded = true;
        reloadReticle.SetActive(false);
        reticle.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        if ((health -= damage) <= 0)
        {
            gameOverCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f;
            AudioListener.volume = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        Health.text = health.ToString();
    }
}
