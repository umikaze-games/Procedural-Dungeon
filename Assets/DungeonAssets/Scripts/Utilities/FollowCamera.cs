using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform target;

    private void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        if (target == null)
        {
            Debug.LogError("No target found. Assign the Player tag to your player avatar.");
            return;
        }
        transform.position = target.position;
    }
}
