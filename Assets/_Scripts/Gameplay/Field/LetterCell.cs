using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class LetterCell : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public Vector2Int index;
    
    public string StoredLetter { get; private set; }

    private void Start()
    {
        StoredLetter = WeightedRandomLetterProvider.GetLetter();

        text.text = StoredLetter.ToUpper();
    }

    public void OnSelectionTrigger()
    {
    }
}
