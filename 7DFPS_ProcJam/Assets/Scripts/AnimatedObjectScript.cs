using UnityEngine;
using System.Collections;

public class AnimatedObjectScript : MonoBehaviour 
{
    public delegate void AnimationFinishedEvent();
    public event AnimationFinishedEvent Animation_Complete;

    public bool DestroyOnComplete = false;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnAnimationComplete()
    {
        if (DestroyOnComplete == true)
        {
            Destroy(gameObject);
        }
    }
}
