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
    public int ElectronCountInner = 0;
    public int ElectronCountMiddle = 0;
    public int ElectronCountOuter = 0;
    public int ElectronCount = 0;
    public int NeutronCount = 0;
    public int ProtonCount = 0;
    public bool IsRadioActive = false;
    public float RadioDecayRate = 0f;
    public bool IsGas = false;
    public bool IsLiquid = false;
    public bool IsSolid = false;
    public float IsotopeMass = 0;
    public int MaxDistance = 5;
    public bool IsRandom = false;
    public bool IsPartCompound = false;
    public string CompoundName = "";

    // realtime vars
    int TargetXLoc;
    int TargetYLoc;
    int TargetZLoc;
    bool DesinationXReached = false;
    bool DesinationYReached = false;
    bool DesinationZReached = false;
    GameObject UniverseManager;

    // Start is called before the first frame update
    void Start()
    {
        UniverseManager = GameObject.Find("Universe Controller");

        if (UniverseManager.gameObject.GetComponent<UniverseController>().ShowTrails)
        {
            gameObject.GetComponent<TrailRenderer>().enabled = true;
        }

        if (IsRandom)
        {
            // set the colour randomly, will be influenced by its properties in the future
            gameObject.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
            gameObject.GetComponent<TrailRenderer>().material.color = gameObject.GetComponent<MeshRenderer>().material.color;

            // set the properties
            ElectronCountInner = Random.Range(1, 2);
            ElectronCountMiddle = Random.Range(1, 8);
            ElectronCountOuter = Random.Range(1, 8);
            NeutronCount = Random.Range(0, 8);
            ProtonCount = Random.Range(0, 8);
            IsotopeMass = Random.Range(0f, 2f);
            ElectronCount = ElectronCountInner + ElectronCountMiddle + ElectronCountOuter;
        }
        SetNewBounds();
    }
    void SetNewBounds()
    {
        TargetXLoc = Random.Range(0, MaxDistance);
        TargetYLoc = Random.Range(0, MaxDistance);
        TargetZLoc = Random.Range(0, MaxDistance);
    }
    private void OnCollisionEnter(Collision collision)
    {
        // only increments when 2 atoms collider with one another
        if (collision.gameObject.tag == "atom")
        {   
            // fires if the electron count of the dominant particle is less than 8, and would be able to take all the electrons from the non-dominant particle
            if (collision.gameObject.GetComponent<AtomController>().ElectronCountOuter < 8 && gameObject.GetComponent<AtomController>().ElectronCountOuter + collision.gameObject.GetComponent<AtomController>().ElectronCountOuter !> 8)
            {
                UniverseManager.gameObject.GetComponent<UniverseController>().CollissionCounter++;

                // bond the atoms (compound)

                // atom has less than 8 atoms, considered unstable and seeking to gain electrons on its outer ring (compound)
                FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                joint.anchor = collision.contacts[0].point;
                joint.connectedBody = collision.contacts[0].otherCollider.transform.GetComponentInParent<Rigidbody>();
                joint.enableCollision = false;

                // set the destination of the child atom to that of the parent atom
                gameObject.GetComponent<AtomController>().TargetXLoc = collision.gameObject.GetComponent<AtomController>().TargetXLoc;
                gameObject.GetComponent<AtomController>().TargetYLoc = collision.gameObject.GetComponent<AtomController>().TargetYLoc;
                gameObject.GetComponent<AtomController>().TargetZLoc = collision.gameObject.GetComponent<AtomController>().TargetZLoc;

                // update the compound status boolean
                gameObject.GetComponent<AtomController>().IsPartCompound = true;
                collision.gameObject.GetComponent<AtomController>().IsPartCompound = true;

                // update the compund name string
                gameObject.GetComponent<AtomController>().CompoundName = collision.gameObject.GetComponent<AtomController>().Symbol + gameObject.GetComponent<AtomController>();
                collision.gameObject.GetComponent<AtomController>().CompoundName = collision.gameObject.GetComponent<AtomController>().Symbol + gameObject.GetComponent<AtomController>();
            }
            else if (collision.gameObject.GetComponent<AtomController>().ElectronCountOuter < 8 && gameObject.GetComponent<AtomController>().ElectronCountOuter < 8)
            {
                // explosion, due to 2 unstable atoms colliding
                // force, position, radius, speed
                collision.rigidbody.AddExplosionForce(2, collision.gameObject.transform.position, 2, 1.0f);
                UniverseManager.gameObject.GetComponent<UniverseController>().CollissionExplosionCounter++;
            }
            else
            {
                // atoms has 8 electrons, no need to form a compound
                // make them smash off each other
                gameObject.GetComponent<Rigidbody>().AddForce(collision.contacts[0].normal * IsotopeMass / 10);
            }
            
        }
        else
        {
            // hit container, redirect
            gameObject.GetComponent<Rigidbody>().AddForce(collision.contacts[0].normal * IsotopeMass / 10);
        }
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
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - MovementSpeed * Time.deltaTime, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else
            {
                // positive X movement
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + MovementSpeed * Time.deltaTime, gameObject.transform.position.y, gameObject.transform.position.z);
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
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - MovementSpeed * Time.deltaTime, gameObject.transform.position.z);
            }
            else
            {
                // positive Y
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + MovementSpeed * Time.deltaTime, gameObject.transform.position.z);
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
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - MovementSpeed * Time.deltaTime);
            }
            else
            {
                // positive z
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + MovementSpeed * Time.deltaTime);
            }     
        }
        else
        {
            DesinationZReached = true;
        }
    }
}
