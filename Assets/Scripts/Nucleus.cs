using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Nucleus : MonoBehaviour
{
    public Transform nucleusCenter;
    public float attachRadius = 0.5f;
    public float attachForce = 10f;
    public float snapDistance = 0.5f;
    public TextMeshProUGUI elementNameText;
    public TextMeshProUGUI elementDescriptionText;

    [System.Serializable]
    public class ElectronShell
    {
        public int maxElectrons;   
        public float shellRadius;  
    }

    public List<ElectronShell> electronShells = new List<ElectronShell>
    {
        new ElectronShell { maxElectrons = 2, shellRadius = 1f },   // K shell
        new ElectronShell { maxElectrons = 8, shellRadius = 2f },   // L shell
        new ElectronShell { maxElectrons = 18, shellRadius = 3f },  // M shell
        new ElectronShell { maxElectrons = 32, shellRadius = 4f }   // N shell
    };

    public float electronOrbitSpeed = 1f;
    public float containmentRadius = 0.3f;
    public float nucleusForce = 10f;

    public List<GameObject> attachedElectrons = new List<GameObject>();
    public List<GameObject> attachedProtons = new List<GameObject>();
    public List<GameObject> attachedNeutrons = new List<GameObject>();
    private List<float> electronOrbitAngles = new List<float>();
    private List<int> electronShellIndices = new List<int>();

    // Periodic Table Data
    private static readonly string[] ElementNames = {
        "Hydrogen", "Helium", "Lithium", "Beryllium", "Boron", "Carbon", "Nitrogen", "Oxygen",
        "Fluorine", "Neon", "Sodium", "Magnesium", "Aluminum", "Silicon", "Phosphorus", "Sulfur",
        "Chlorine", "Argon", "Potassium", "Calcium"
    };

    private void Update()
    {
        // Find nearby protons and neutrons
        Collider[] nearbyParticles = Physics.OverlapSphere(
            nucleusCenter.position,
            attachRadius,
            LayerMask.GetMask("Proton", "Neutron", "Electron")
        );

        HashSet<GameObject> nearbyParticleSet = new HashSet<GameObject>(
            System.Array.ConvertAll(nearbyParticles, p => p.gameObject)
        );

        for (int i = attachedProtons.Count - 1; i >= 0; i--)
        {
            if (!nearbyParticleSet.Contains(attachedProtons[i]))
            {
                attachedProtons.RemoveAt(i);
            }
        }

        for (int i = attachedNeutrons.Count - 1; i >= 0; i--)
        {
            if (!nearbyParticleSet.Contains(attachedNeutrons[i]))
            {
                attachedNeutrons.RemoveAt(i);
            }
        }

        foreach (Collider particle in nearbyParticles)
        {
            if (particle.CompareTag("Electron"))
            {
                AttemptToAddElectronToBohrShell(particle.gameObject);
            }
            else if (particle.CompareTag("Proton"))
            {
                AttachToNucleus(particle.gameObject, attachedProtons);
            }
            else if (particle.CompareTag("Neutron"))
            {
                AttachToNucleus(particle.gameObject, attachedNeutrons);
            }
        }

        // Update UI with element information
        UpdateElementInfo();
    }

    void AttachToNucleus(GameObject particle, List<GameObject> particleList)
    {
        Rigidbody particleRb = particle.GetComponent<Rigidbody>();
        if (particleRb == null) return;

        Vector3 directionToNucleus = nucleusCenter.position - particle.transform.position;
        float distanceToNucleus = directionToNucleus.magnitude;

        if (distanceToNucleus <= snapDistance)
        {
            Vector3 localPosition = nucleusCenter.InverseTransformPoint(particle.transform.position);

            if (localPosition.magnitude > containmentRadius)
            {
                Vector3 containmentForce = -localPosition.normalized * nucleusForce;
                particleRb.AddForce(nucleusCenter.TransformDirection(containmentForce));
            }

            particleRb.velocity *= 0.5f;

            if (!particleList.Contains(particle))
            {
                particleList.Add(particle);
            }
        }
        else
        {
            particleRb.AddForce(directionToNucleus.normalized * nucleusForce);
        }
    }

    void AttemptToAddElectronToBohrShell(GameObject electron)
    {
        if (attachedElectrons.Contains(electron)) return;

        // Find the first shell that isn't full
        for (int shellIndex = 0; shellIndex < electronShells.Count; shellIndex++)
        {
            int electronsInThisShell = CountElectronsInShell(shellIndex);

            if (electronsInThisShell < electronShells[shellIndex].maxElectrons)
            {
                Rigidbody electronRb = electron.GetComponent<Rigidbody>();
                if (electronRb == null) return;

                electronRb.isKinematic = true;
                electronRb.velocity = Vector3.zero;
                attachedElectrons.Add(electron);
                electronShellIndices.Add(shellIndex);
                electronOrbitAngles.Add(Random.Range(0f, Mathf.PI * 2));

                break;
            }
        }
    }

    int CountElectronsInShell(int shellIndex)
    {
        int count = 0;
        for (int i = 0; i < electronShellIndices.Count; i++)
        {
            if (electronShellIndices[i] == shellIndex)
            {
                count++;
            }
        }
        return count;
    }

    private void FixedUpdate()
    {
        for (int i = attachedElectrons.Count - 1; i >= 0; i--)
        {
            if (attachedElectrons[i] == null)
            {
                attachedElectrons.RemoveAt(i);
                electronOrbitAngles.RemoveAt(i);
                electronShellIndices.RemoveAt(i);
                continue;
            }

            int shellIndex = electronShellIndices[i];
            float shellRadius = electronShells[shellIndex].shellRadius;

            electronOrbitAngles[i] += electronOrbitSpeed * Time.fixedDeltaTime;

            Vector3 orbitPosition = new Vector3(
                Mathf.Cos(electronOrbitAngles[i]) * shellRadius,
                Mathf.Sin(electronOrbitAngles[i]) * shellRadius,
                Mathf.Cos(electronOrbitAngles[i] + Mathf.PI / 4) * shellRadius
            );

            attachedElectrons[i].transform.position = nucleusCenter.position + orbitPosition;
        }
    }

    void UpdateElementInfo()
    {
        int protonCount = attachedProtons.Count;
        int neutronCount = attachedNeutrons.Count;
        int electronCount = attachedElectrons.Count;

        string elementName = protonCount > 0 && protonCount <= ElementNames.Length
            ? ElementNames[protonCount - 1]
            : "Unknown Element";

        if (elementNameText != null)
        {
            elementNameText.text = elementName;
        }

        if (elementDescriptionText != null)
        {
            elementDescriptionText.text = $"Atomic Number: {protonCount}\n" +
                                          $"Mass Number: {protonCount + neutronCount}\n" +
                                          $"Protons: {protonCount}\n" +
                                          $"Neutrons: {neutronCount}\n" +
                                          $"Electrons: {electronCount}";
        }
    }

    // Visualize the attachment radius and electron shells in the scene view
    private void OnDrawGizmosSelected()
    {
        if (nucleusCenter == null) return;

        // Draw attachment radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(nucleusCenter.position, attachRadius);

        // Draw electron shells
        Gizmos.color = Color.cyan;
        foreach (var shell in electronShells)
        {
            Gizmos.DrawWireSphere(nucleusCenter.position, shell.shellRadius);
        }
    }
}