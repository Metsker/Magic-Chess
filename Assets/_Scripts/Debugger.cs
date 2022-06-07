using UnityEngine;

public class Debugger : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool isDebug;
    public static bool IsDebug { get; private set; }

    private void Start()
    {
        IsDebug = isDebug;
    }
}
