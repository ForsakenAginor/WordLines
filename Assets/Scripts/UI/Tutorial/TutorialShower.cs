using UnityEngine;

public class TutorialShower : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialPanel;

    private void Start()
    {
        TutorialData tutorialData = new ();

        if (tutorialData.IsTutorialCompleted == false)
            _tutorialPanel.SetActive(true);
    }
}
