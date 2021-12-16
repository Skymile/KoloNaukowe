/*

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

List<int> list = new List<int>() { 1, 2, 3 };

// Write 1.
using (var writer = new StreamWriter("test.txt"))
	foreach (var i in list)
		writer.WriteLine(i);
// Write 2.
File.WriteAllLines("text.txt", list.Select(i => i.ToString()));

// Read 1.
List<string> rList = new List<string>();
using (var reader = new StreamReader("test.txt"))
	while (!reader.EndOfStream)
	{
		string line = reader.ReadLine();
		rList.Add(line);
	}
IEnumerable<string> enList = rList;

// Read 2.
IEnumerable<string> lines = File.ReadLines("test.txt");

 */