using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DifficultyManager : MonoBehaviour
{
    public const string PlayerPref_LastSessionAverage = "LastSessionAverage";
    public static DifficultyManager instance;

    [Header("Dynamic Difficulty Options")]
    [SerializeField]
    private HeadsetManager headset;
    [SerializeField]
    [Range(5, 25)]
    private float deltaUpdateTime = 15;
    [SerializeField]
    [Range(10, 30)]
    private int maxDifficultyRaise = 15;
    [SerializeField]
    [Range(0.5f, 0.9f)]
    private float difficultyPercentile = 0.6f;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Multiple Difficulty Managers found. First on " + instance.name + ", second on " + name);
            throw new Exception();
        }
        else
        {
            instance = this;
            dynamicDataset = new List<int>();
            generatedDifficulties = new List<int>();
            //Line added
            ActivityStart();
        }
    }

    private static List<int> dynamicDataset;
    private static List<int> generatedDifficulties;
    private Coroutine DynamicUpdateCor;
    private DifficultyMode mode;
    private string userPrefsKey;
    private int difficulty;
    //private int SessionAverage;
    private bool isPaused;
    private int timerCounter;
    #region Properties
    /// <summary>
    /// The current difficulty.
    /// </summary>
    public static int Difficulty { get { return instance.difficulty; } }
    /// <summary>
    /// The average attention of the user in their last completed session.
    /// </summary>
    public static int SessionAverage { get; set; }
    /// <summary>
    /// The current mode of the script.
    /// </summary> 
    /// <example>
    /// <code>
    /// Static - Used in training programs only. Sets difficulty to predetermined value.
    /// Manual - Allows the user to select a difficulty. Difficulty does not change.
    /// Dynamic - Periodically changes the difficulty while activity plays.
    /// </code>
    /// </example>
    public static DifficultyMode Mode { get; set; }
    #endregion
    
    /// <summary>
    /// Sets the difficulty to specific value.
    /// </summary>
    /// <param name="value"></param>
    public static void SetDifficulty(int value)
    { 
        instance.difficulty = value; 
    }

    #region Dynamic Handlers

    /// <summary>
    /// Prepares the manager for dynamically updating the difficulty level
    /// </summary>
    private void ActivityStart()
    {
        // TODO: This line should be changed when we add the option to select Difficulty
        Mode = DifficultyMode.Dynamic;
        if (Mode == DifficultyMode.Dynamic)
        {
            StartDynamicUpdates();
        }
    }
    /// <summary>
    /// Stops the manager from tracking and setting difficulty
    /// </summary>
    private void ActivityEnded()
    {
        StopDynamicUpdates();
    }
    //Prepates the manager to record data and use it to calculate new difficulty.
    private void StartDynamicUpdates()
    {
        difficulty = 30;
        timerCounter = 0;
        dynamicDataset.Clear();
        generatedDifficulties.Clear();
        generatedDifficulties.Add(difficulty);
        HeadsetManager.UpdateAttentionEvent += AddTodynamicDataset;

        //DynamicUpdateCor = StartCoroutine(CountToDifficultyGeneration());
    }

    //Pauses data recording while activity is paused to prevent unnecessary data.
    private void PauseDynamicUpdates(bool isPaused)
    {
        this.isPaused = isPaused;
        if (isPaused)
        {
            HeadsetManager.UpdateAttentionEvent -= AddTodynamicDataset;
        }
        else
        {
            HeadsetManager.UpdateAttentionEvent += AddTodynamicDataset;
        }
    }

    //Stops data recording and dynamic update coroutine.
    private void StopDynamicUpdates()
    {
        HeadsetManager.UpdateAttentionEvent -= AddTodynamicDataset;
        //if (DynamicUpdateCor != null)
        //{
        //    StopCoroutine(DynamicUpdateCor);
        //    DynamicUpdateCor = null;
        //}
    }

    //Adds the data from headset to the dataset to be calculated for dynamic difficulty.
    private void AddTodynamicDataset(int value)
    {
        dynamicDataset.Add(value);
    }

    //Once every 15 seconds, calls dynamic difficulty generation.
    private IEnumerator CountToDifficultyRegeneration()
    {
        float timer = 0;
        while (true)
        {
            if (isPaused || !HeadsetManager.IsConnected)
            {
                yield return null;
                continue;
            }
            timer += Time.deltaTime;
            yield return null;
            if (timer >= deltaUpdateTime)
            {
                timer = 0;
                GenerateStandardDifficulty();
            }
        }
    }

    private void CountToDifficultyGeneration()
    {
        timerCounter++;
        if (timerCounter >= deltaUpdateTime)
        {
            timerCounter = 0;
            GenerateStandardDifficulty();
        }
    }

    public static float AverageGeneratedDifficulty()
    {
        float sum = 0;
        foreach (int sample in generatedDifficulties)
        {
            sum += sample;
        }
        return sum / generatedDifficulties.Count;
    }
    private void GenerateStandardDifficulty()
    {
        //sort all recorded data from lowest to highest
        dynamicDataset.Sort(new CompareInteger());

        //calculates the index of value by percentile rank
        int percentileRank = (int)Mathf.Round(difficultyPercentile * (dynamicDataset.Count + 1));

        //gets the value using percentile rank as index (low rank value)
        float percentileScoreAtRank = dynamicDataset[percentileRank];
        //gets the value using percentile rank as index + 1 (high rank value)
        float percentileScoreAboveRank = dynamicDataset[percentileRank + 1];

        //subtracts low rank value from high rank value, then multiplies by percentile as weight.
        float scoreWeight = difficultyPercentile * (percentileScoreAboveRank - percentileScoreAtRank);

        //Adds the weight to the low rank value, resulting in a weighted average value
        int newDifficulty = (int)Mathf.Round(scoreWeight + percentileScoreAtRank);

        //Prevents the dynamic difficulty from spiking up too sharply if the values fluctuate too much
        if (newDifficulty > difficulty + maxDifficultyRaise)
            difficulty += maxDifficultyRaise;
        else
            difficulty = newDifficulty;

        //Clamps the value to prevent too high/low difficulties from occuring.
        difficulty = Mathf.Clamp(difficulty, 30, 90);
        generatedDifficulties.Add(difficulty);
        dynamicDataset.Clear();
        Debug.Log("Updating Dynamic Difficulty: " + difficulty);
    }
    #endregion
    class CompareInteger : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (x > y)
                return 1;
            if (x < y)
                return -1;
            return 0;
        }
    }
}
