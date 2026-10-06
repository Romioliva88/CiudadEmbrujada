using UnityEngine;

public class AlinearCiudad : MonoBehaviour
{
    void Awake()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        Debug.Log("CIUDAD ALINEADA: " + transform.position);
    }
}