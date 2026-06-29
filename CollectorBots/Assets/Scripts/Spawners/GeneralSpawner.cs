using System.Collections;
using UnityEngine;

public class GeneralSpawner : MonoBehaviour
{
    [SerializeField] protected Transform SpawnContainer;

    protected int MinTimeCreate = 3;
    protected int MaxTimeCreate = 5;

    protected IEnumerator LaunchCreate(Vector3 position)
    {
        int timeCreate = Random.Range(MinTimeCreate, MaxTimeCreate);
        yield return new WaitForSeconds(timeCreate);
    }
}