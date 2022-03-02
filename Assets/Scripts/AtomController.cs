using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomController : MonoBehaviour
{
    // atom properties
    public string ElementName = "";
    public bool IsElectricallyConductive = false;
    public float ElectricalConductivityRate = 0f;
    public bool IsThermallyConductive = false;
    public float ThermalConductivityRate = 0f;
    public bool IsMagnetic = false;
    public float MagnativityRate = 0f;
    public float MovementSpeed = 1.0f; // * simulation speed
    public string Symbol = "";
    public int ElectronCount = 0;
    public int NeutronCount = 0;
    public int ProtonCount = 0;
    public bool IsRadioActive = false;
    public float RadioDecayRate = 0f;
    public bool IsGas = false;
    public bool IsLiquid = false;
    public bool IsSolid = false;
    public float IsotopeMass = 0;
    public int MaxDistance = 100;
    public bool IsRandom = false;

    // realtime vars
    int TargetXLoc;
    int TargetYLoc;
    int TargetZLoc;
    bool DesinationXReached = false;
    bool DesinationYReached = false;
    bool DesinationZReached = false;

    // Start is called before the first frame update
    void Start()
    {
        if (IsRandom)
        {
            // set the colour randomly, will be influenced by its properties in the future
            gameObject.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();

            // set the properties
            ElectronCount = Random.Range(0, 9999999);
            NeutronCount = Random.Range(0, 9999999);
            ProtonCount = Random.Range(0, 9999999);
            IsotopeMass = Random.Range(0f, 2f);
        }
        SetNewBounds();
    }

    void SetNewBounds()
    {
        TargetXLoc = Random.Range(0, MaxDistance);
        TargetYLoc = Random.Range(0, MaxDistance);
        TargetZLoc = Random.Range(0, MaxDistance);
    }

    // Update is called once per frame
    void Update()
    {
        // move the atom to a random destination

        if (DesinationXReached && DesinationYReached && DesinationZReached)
        {
            SetNewBounds();
            DesinationXReached = false;
            DesinationYReached = false;
            DesinationZReached = false;
        }
        if (gameObject.transform.position.x != TargetXLoc)
        {
            if (gameObject.transform.position.x > TargetXLoc)
            {
                // negative X movement
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - MovementSpeed + IsotopeMass * 2 * Time.deltaTime, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else
            {
                // positive X movement
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + MovementSpeed - IsotopeMass / 2 * Time.deltaTime, gameObject.transform.position.y, gameObject.transform.position.z);
            }   
        }
        else
        {
            DesinationXReached = true;
        }
        if (gameObject.transform.position.y != TargetYLoc)
        {
            if (gameObject.transform.position.y > TargetYLoc)
            {
                // negative Y
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - MovementSpeed + IsotopeMass / 2 * Time.deltaTime, gameObject.transform.position.z);
            }
            else
            {
                // positive Y
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + MovementSpeed - IsotopeMass / 2 * Time.deltaTime, gameObject.transform.position.z);
            }
        }
        else
        {
            DesinationYReached = true;
        }
        if (gameObject.transform.position.z != TargetZLoc)
        {
            if (gameObject.transform.position.z > TargetZLoc)
            {
                // negative z
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - MovementSpeed + IsotopeMass / 2 * Time.deltaTime);
            }
            else
            {
                // positive z
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + MovementSpeed - IsotopeMass / 2 * Time.deltaTime);
            }     
        }
        else
        {
            DesinationZReached = true;
        }
    }
}
