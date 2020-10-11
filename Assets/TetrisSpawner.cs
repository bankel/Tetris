using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    public GameObject[] tetrisPrefab;

    // Start is called before the first frame update
    void Start()
    {
        NewTetris();
    }

    public void NewTetris()
    {
        Instantiate(tetrisPrefab[Random.Range(0, tetrisPrefab.Length)], transform.position,
            Quaternion.identity);
    }
}