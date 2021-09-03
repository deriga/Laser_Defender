using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float SpinSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, SpinSpeed * Time.deltaTime);
    }
}
