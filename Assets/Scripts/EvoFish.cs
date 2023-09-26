using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvoFish : MonoBehaviour
{
    public Animator animator;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Food")
        {
            animator.Play("Bite");
        }
    }
}
