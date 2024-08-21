using System.Collections.Generic;

public static class WeightedRandomLetterProvider
{
	private static readonly WeightedRandom<string> WeightedRandomLetters = new (new ()
	{
		{"a", 8.0f},
		{"б", 1.5f},
		{"в", 4.5f},
		{"г", 1.7f},
		{"д", 3.0f},
		{"е", 8.5f},
		{"ж", 1.0f},
		{"з", 1.6f},
		{"и", 7.3f},
		{"й", 1.2f},
		{"к", 3.5f},
		{"л", 4.4f},
		{"м", 3.2f},
		{"н", 6.7f},
		{"о", 10.9f},
		{"п", 2.8f},
		{"р", 4.7f},
		{"с", 5.4f},
		{"т", 6.2f},
		{"у", 2.6f},
		{"ф", 0.3f},
		{"х", 1.0f},
		{"ц", 0.5f},
		{"ч", 1.4f},
		{"ш", 0.7f},
		{"ы", 1.9f},
		{"э", 0.3f},
		{"ю", 0.6f},
		{"я", 2.0f}
	});

	public static string GetLetter() => WeightedRandomLetters.GetValue();
}
