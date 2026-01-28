using System.Collections.Generic;
using UnityEngine;

public class MovementLayer : MonoBehaviour
{
    [Header("Section Management")]
    [SerializeField] Transform sectionsContainer;
    [SerializeField] List<Section> sectionPrefabs;
    [SerializeField] Transform anchorSpawn;
    [SerializeField] Transform anchorDespawn;

    [Space(10)]
    [SerializeField] private Section startSection;
    [SerializeField] protected Map map;

    #region Attributes
    private List<Section> sections;
    private bool move;
    #endregion


    protected virtual void Start()
    {
        sections = new List<Section>() { startSection };

        map.StartMovement += OnStartMovement;
        map.StopMovement += OnStopMovement;
    }

    protected virtual void Update()
    {
        if (!move) return; 

        Move();
    }

    protected virtual void OnDestroy()
    {
        map.StartMovement -= OnStartMovement;
        map.StopMovement -= OnStopMovement;
    }

    #region Movement
    protected virtual void Move()
    {
        var movementVector = new Vector3(map.MovementSpeed * Time.deltaTime, 0f, 0f);
        Move(movementVector);
    }

    protected void Move(Vector3 movementVector)
    {
        List<Section> sectionsToDespawn = new List<Section>();

        foreach (Section section in sections)
        {
            // move section to the left
            section.transform.position -= movementVector;

            if (section.AnchorEnd.position.x < anchorDespawn.transform.position.x)
            {
                // mark section for despawning
                sectionsToDespawn.Add(section);
            }
        }

        // despawn marked sections
        foreach (Section section in sectionsToDespawn)
        {
            DespawnSection(section);
        }

        // check whether a new sections must be spawned
        if (sections[sections.Count - 1].AnchorEnd.transform.position.x < anchorSpawn.transform.position.x)
        {
            SpawnRandomSection();
        }
    }
    #endregion

    #region Spawning/Despawing Sections
    /// <summary>
    /// Despawns the given section
    /// </summary>
    /// <param name="section">section to despawn</param>
    public void DespawnSection(Section section)
    {
        sections.Remove(section);
        Destroy(section.gameObject);
    }

    /// <summary>
    /// Spawns a randomly chosen section
    /// </summary>
    public void SpawnRandomSection()
    {
        Section randomSection = sectionPrefabs[Random.Range(0, sectionPrefabs.Count)];
        SpawnSection(randomSection);
    }

    /// <summary>
    /// Spawns the given section
    /// </summary>
    /// <param name="section"></param>
    public void SpawnSection(Section section)
    {
        // instantiate given section
        Section spawnedSection = Instantiate(section, sectionsContainer);

        // position the instantiated section
        Section lastSection = sections[sections.Count - 1];
        Vector3 offset = spawnedSection.AnchorStart.transform.localPosition;
        spawnedSection.transform.position = lastSection.AnchorEnd.transform.position - offset;

        // add instantiated section to active sections
        sections.Add(spawnedSection);
    }
    #endregion

    #region Event Methods
    private void OnStartMovement()
    {
        move = true;
    }

    private void OnStopMovement()
    {
        move = false;
    }
    #endregion
}
