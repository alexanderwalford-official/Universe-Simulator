using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseController : MonoBehaviour
{
    // container references
    public GameObject UniverseContainerSmall;
    public GameObject UniverseContainerMedium;
    public GameObject UniverseContainerLarge;

    // public simulation settings
    public string SimulationSize = "medium";
    public float SimulationSpeed = 1.0f;
    public int MaxAtoms = 999999999;
    public float AtomSpawnRate = 0.1f;
    public string[] AtomsList;
    public int AtomCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (SimulationSize == "small")
        {
            UniverseContainerSmall.SetActive(true);
        }
        else if (SimulationSize == "medium")
        {
            UniverseContainerMedium.SetActive(true);
        }
        else if (SimulationSize == "large")
        {
            UniverseContainerLarge.SetActive(true);
        }
        else
        {
            // assume infinate
        }

        StartCoroutine(AtomSpawner());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator AtomSpawner()
    {
        // spawn a random atom every X defined seconds
        yield return new WaitForSeconds(AtomSpawnRate);

        // randomly select an atom
        string id = AtomsList[Random.Range(0, AtomsList.Length)];

        // load the gameobject
        GameObject Atom = Resources.Load<GameObject>("Atoms/" + id);

        // instantiate the object
        var obj = Instantiate(Atom, new Vector3(0, 0, 0), Quaternion.identity);
        obj.transform.parent = gameObject.transform;

        // + 1 atoms
        AtomCount++;

        // loop
        StartCoroutine(AtomSpawner());
    }
}
