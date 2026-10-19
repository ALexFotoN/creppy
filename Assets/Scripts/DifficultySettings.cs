
using UnityEngine;

public class DifficultySettings : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public static DifficultySettings Instance { get; private set; }

    [SerializeField] private Difficulty difficulty = Difficulty.Medium;

    public float EnemyDamageMultiplier
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.Easy: return 0.5f;
                case Difficulty.Hard: return 1.5f;
                default: return 1f;
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }
}