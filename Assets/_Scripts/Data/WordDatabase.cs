using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordDataBase", menuName = "Data/Create WordDataBase")]
public class WordDatabase : ScriptableObject
{
	[SerializeField] private TextAsset wordsTextAsset;
	
	private Dictionary<string, int> words;

	private void OnEnable()
	{
		words = new Dictionary<string, int>();
		
		fgCSVReader.LoadFromString(wordsTextAsset.text, WordReader);
	}

	private void WordReader(int lineIndex, List<string> line)
	{
		string word = line[0].ToLower().Replace("ё", "е");

		if (word.Length < GameConstants.MinWordLength || word.Length >= GameConstants.MaxWordLength || words.ContainsKey(word))
		{
			return;
		}
		
		words.Add(word, lineIndex);
	}

	public bool IsWordValid(string word)
	{
		return word != null && words.ContainsKey(word);
	}
}
