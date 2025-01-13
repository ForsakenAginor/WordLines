using Lean.Localization;
using UnityEngine;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu(LeanLocalization.ComponentPathPrefix + "Localized GameObject")]
public class LeanLocalizationGO : LeanLocalizedBehaviour
{
    private LocalizedTransform _gameObject;

    public override void UpdateTranslation(LeanTranslation translation)
    {
        if (translation != null && translation.Data is GameObject translationObject 
            && translationObject.TryGetComponent(out LocalizedTransform localizedTransform))
        {
            if(_gameObject != null)
                Destroy(_gameObject.gameObject);

            _gameObject = Instantiate(localizedTransform, transform);
        }
    }
}
