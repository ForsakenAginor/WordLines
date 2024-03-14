using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private BoardInitializer _board;
    [SerializeField] private Records _records;

    private void OnEnable()
    {
        _board.WordFound += OnWordFound;
    }

    private void OnDisable()
    {
        _board.WordFound -= OnWordFound;
    }

    private void OnWordFound(string word)
    {
        _records.AddRecord(word);
    }
}
