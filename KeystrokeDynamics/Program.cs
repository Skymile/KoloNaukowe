using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

const string alphabet = "QWERTYUIOPASDFGHJKLZXCVBNM";
var random = new Random();

using (var writer = new StreamWriter("input.txt"))
{
	writer.Write("C");
	foreach (var i in Enumerable.Range(1, 30))
		writer.Write(i.ToString().PadLeft(7));
	writer.WriteLine();

	foreach (var i in alphabet)
	{
		writer.Write(i.ToString());
		foreach (var j in Enumerable.Range(0, 30))
		{
			int value = random.Next() % 120 + 10;
			string text = value.ToString().PadLeft(7);
			writer.Write(text);
		}
		writer.WriteLine();
	}
}

//File.WriteAllLines(
//	"input.txt", alphabet
//	.Select(i => string.Join(" ",
//		i.ToString(),
//		string.Join(" ", Enumerable
//			.Range(0, 30)
//			.Select(i => random.Next() % 120 + 10)
//			.Select(i => i.ToString().PadLeft(6))
//		)
//	))
//	.Prepend(string.Join(" ",
//		"C",
//		string.Join(" ", Enumerable
//			.Range(1, 30)
//			.Select(i => i.ToString().PadLeft(6))
//	)))
//);

var lines2 =
	from i in File.ReadLines("input.txt").Skip(1)
	let j = i.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1)
	select j into k
	from j in k
	select int.Parse(j);

  var lines = File
	.ReadLines("input.txt")
	.Skip(1)
	.Select(i => i
		.Split(" ", StringSplitOptions.RemoveEmptyEntries)
		.Skip(1)
		.Select(int.Parse)
		.ToArray())
	.ToArray();
int len = lines[0].Length;

var users = new Dictionary<int, int[]>(); // Id, {Q, W, E ...}
for (int i = 0; i < lines.Length; i++)
{
	for (int j = 0; j < lines[i].Length; j++)
	{
		if (!users.ContainsKey(j))
			users[j] = new int[len];
		users[j][i] = lines[i][j];
	}
}

;
