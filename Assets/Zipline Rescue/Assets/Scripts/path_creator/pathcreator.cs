using UnityEngine;

public class pathcreator : MonoBehaviour
{
    [HideInInspector] public path Path;

    public void createpath()
    {
        Path = new path(transform.position);
    }
}