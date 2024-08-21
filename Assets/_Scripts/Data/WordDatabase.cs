using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordDataBase", menuName = "Data/Create WordDataBase")]
public class WordDatabase : ScriptableObject
{
	[SerializeField] private TextAsset wordsTextAsset;
	
	private Dictionary<string, int> words;
	
	public void Initialize()
	{
		words = new Dictionary<string, int>();
		
		fgCSVReader.LoadFromString(wordsTextAsset.text, WordReader);
	}

	private void WordReader(int lineIndex, List<string> line)
	{
		string word = line[0];

		if (words.ContainsKey(line[0]))
		{
			Debug.LogError($"Word {word} is already present in a dictionary, word lines were {words[word]} and {lineIndex}, word was: {word}");
			return;
		}
		
		words.Add(line[0], lineIndex);
	}

	public bool IsWordValid(string word)
	{
		return words.ContainsKey(word);
	}
}
