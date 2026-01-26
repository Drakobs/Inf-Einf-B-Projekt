using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Grid tilemapGrid;
    [SerializeField] private float heightUnits;
    [SerializeField] private float visibleUnits;

    [Header("Section Management")]
    [SerializeField] Transform sectionsContainer;
    [SerializeField] List<Section> sectionPrefabs;
    [SerializeField] Transform anchorSpawn;
    [SerializeField] Transform anchorDespawn;

    [Space(10)]
    [SerializeField] StartSection startSection;

    [Header("Background")]
    [SerializeField] SpriteRenderer fogRenderer;

    private List<Section> sections;

    [SerializeField] private float movementSpeed;

    private bool isPaused;

    void Start()
    {
        // register self on game manager
        GameManager.Instance.Map = this;

        // subscribe to GameManager events
        GameManager.Instance.LevelStarted += OnLevelStart;
        GameManager.Instance.LevelEnded += OnLevelEnd;

        // add start section to active sections list
        sections = new List<Section>() { startSection };

        //position camera
        CameraController.Instance.PositionCamera(startSection.AnchorCamera, tilemapGrid, visibleUnits);

        //set background fog
 
        
        //set initial paused state
        isPaused = GameManager.Instance.CurrentState != GameManager.GameState.Level;

    }

    // Update is called once per frame
    void Update()
    {
        // stop execution if game is currently not in level state
        if (isPaused) return;
        
        List<Section> sectionsToDespawn = new List<Section>();
        // move vector for sections
        Vector3 moveVector = new Vector3(movementSpeed * Time.deltaTime, 0f, 0f);
        foreach (Section section in sections)
        {
            // move section to the left
            section.transform.position -= moveVector;

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

    private void OnDestroy()
    {
        GameManager.Instance.LevelStarted -= OnLevelStart;
        GameManager.Instance.LevelEnded -= OnLevelEnd;
    }

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
        Debug.Log("Section spawned");
    }

    /// <summary>
    /// Executed when the level starts
    /// </summary>
    public void OnLevelStart()
    {
        isPaused = false;
    }

    /// <summary>
    /// Executed when the level ends
    /// </summary>
    public void OnLevelEnd()
    {
        isPaused = true;
    }
    #endregion
}
