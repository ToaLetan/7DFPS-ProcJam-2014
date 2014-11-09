using UnityEngine;
using System.Collections;

public class BillboardSprite : MonoBehaviour 
{
    private Camera playerCamera = null;


	// Use this for initialization
	void Start () 
    {
        playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponent<Camera>(); ;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (playerCamera != null)
        {
            transform.LookAt(playerCamera.transform.position);
        }
	}
}
