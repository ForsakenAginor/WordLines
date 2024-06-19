using UnityEngine;
namespace LeaderboardSystem
{
    internal class LeaderboardElement : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _nameField;
        [SerializeField] private TMPro.TextMeshProUGUI _rankField;
        [SerializeField] private TMPro.TextMeshProUGUI _scoreField;

        internal void Init(string name, int rank, int score)
        {
            const string AnonymousName = "Anonymous";
            const string AnonymousNameRu = "Аноним";

            _scoreField.text = score.ToString();
            _rankField.text = rank.ToString();
            
            if (name != AnonymousName)
            {
                _nameField.text = name;
                return;
            }

            _nameField.text = AnonymousNameRu;
        }
    }
}
