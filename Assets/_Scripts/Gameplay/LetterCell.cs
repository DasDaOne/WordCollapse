using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class LetterCell : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public Vector2Int index;
    
    public string StoredLetter { get; private set; }

    private const string RussianCharacters = "абвгдежзийклмнопрстуфхцчшщыэюя";

    private void Start()
    {
        StoredLetter = RussianCharacters[Random.Range(0, RussianCharacters.Length)].ToString();

        text.text = StoredLetter.ToUpper();
    }

    public void OnSelectionTrigger()
    {
        StopAllCoroutines();
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        text.color = Color.blue;

        yield return new WaitForSeconds(0.125f);

        text.color = Color.white;
    }
}
