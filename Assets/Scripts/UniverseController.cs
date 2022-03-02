using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniverseController : MonoBehaviour
{
    // container references
    public GameObject UniverseContainerSmall;
    public GameObject UniverseContainerMedium;
    public GameObject UniverseContainerLarge;
    public Camera MainCam;

    public Text InformationText;

    // public simulation settings
    public string SimulationSize = "medium";
    public float SimulationSpeed = 1.0f;
    public int MaxAtoms = 999999999;
    public float AtomSpawnRate = 0.1f;
    public string[] AtomsList;
    public int AtomCount = 0;
    public int CollissionCounter = 0;
    public int TimeElapsed = 0;

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
        StartCoroutine(TimeManager());
    }

    // Update is called once per frame
    void Update()
    {
        InformationText.text = "================\nSIMULATION DATA\n================ \nAtom Count: " + AtomCount.ToString() + "\nCollision Count (bonds): " + CollissionCounter.ToString() + "\nTime Elapsed (sec): " + TimeElapsed.ToString() + "\nSimulation Speed: " + SimulationSpeed + "\nSimulation Size: " + SimulationSize;
    }

    IEnumerator TimeManager()
    {
        if (TimeElapsed.ToString().EndsWith("0"))
        {
            MainCam.fieldOfView = 0.5f;
        }
        else if (TimeElapsed.ToString().EndsWith("5"))
        {
            MainCam.fieldOfView = 20f;
        }
        else
        {
            MainCam.fieldOfView = 93f;
        }
        yield return new WaitForSeconds(1);
        TimeElapsed++;
        StartCoroutine(TimeManager());
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
        var obj = Instantiate(Atom, new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2)), Quaternion.identity);
        obj.transform.parent = gameObject.transform;

        // + 1 atoms
        AtomCount++;

        // loop
        StartCoroutine(AtomSpawner());
    }
}
