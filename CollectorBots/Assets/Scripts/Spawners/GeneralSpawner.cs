using System.Collections;
using UnityEngine;

public class GeneralSpawner : MonoBehaviour
{
    protected int MinTimeCreate = 3;
    protected int MaxTimeCreate = 5;

    protected IEnumerator LaunchCreate()
    {
        int timeCreate = Random.Range(MinTimeCreate, MaxTimeCreate);
        yield return new WaitForSeconds(timeCreate);
    }
}