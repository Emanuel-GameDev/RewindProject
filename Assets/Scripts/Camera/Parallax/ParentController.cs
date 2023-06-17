using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentController : MonoBehaviour
{
    private void Start()
    {
        if (transform.parent != null)
            transform.parent = null;

        float playerPosX = GameManager.Instance.cameraManager.mainCam.Follow.transform.position.x;
        transform.position = new Vector3(playerPosX, 0, 0);
    }
}
