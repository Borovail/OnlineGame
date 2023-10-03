using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollowScript : NetworkBehaviour
{
   
    private Transform playerTarget;

    [SerializeField]
    private float bound_X = 0.3f, bound_Y = 0.15f,border_xLeft=-2, border_xRight=43;



    private Vector3 deltaPos;

    private float delta_X, delta_Y;

    void Awake()
    {
        //if(IsLocalPlayer)
            playerTarget = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void LateUpdate()
    {
        if (playerTarget == null)
        {
            playerTarget = GameObject.FindGameObjectWithTag("Player")?.transform;
            return;
        }

        deltaPos = Vector3.zero;

        if (transform.position.x<border_xRight && transform.position.x > border_xLeft)
        {

            delta_X = playerTarget.position.x - transform.position.x;

            if (delta_X > bound_X || delta_X < -bound_X)
            {
                if (transform.position.x < playerTarget.position.x)
                    deltaPos.x = delta_X - bound_X;
                else
                    deltaPos.x = delta_X + bound_X;
            }
        }
    
        delta_Y = playerTarget.position.y - transform.position.y;

        if (delta_Y > bound_Y || delta_Y < -bound_Y)
        {
            if (transform.position.y < playerTarget.position.y)
                deltaPos.y = delta_Y - bound_Y;
            else
                deltaPos.y = delta_Y + bound_Y;
        }

        deltaPos.z = 0f;

        transform.position += deltaPos;

    }
}
