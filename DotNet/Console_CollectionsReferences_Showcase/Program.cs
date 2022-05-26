using System;

var kk = new ArrayStruct() { Length = 8 };
var arr = new Array();
for (int i = 0; i < 20; i++)
{
	Console.WriteLine(arr.Length);
	arr.Add(i);
}

Console.WriteLine(arr);
Console.WriteLine(kk.Length);

TestArr(arr);
TestStruct(kk);

Console.WriteLine(arr.Length);
Console.WriteLine(kk.Length);

static void TestStruct(ArrayStruct i)
{
	i.Length = 9;
}

static void TestArr(Array array)
{
	array.Length = 50;
}

//int[] arr = new int[4];
//var linkedList = new LinkedList<int>();

// list = [4, 3, 0, 1, 2]

// set  = [4=>0, 3=>1, 0=>2, 1=>3, 2=>4]
// set    [0, 1, 0, 0, 0]
// set[1]
// 3
// => 1

/*
var list = new List<int>();
var dict = new Dictionary<int, int>();
var set = new HashSet<int>();

//var readonlyCol = new ReadOnlyCollection<int>(list);
//var col = new Collection<int>(dict);
//var stack = new Stack<int>();
//var queue = new Queue<int>();

var random = new Random();
for (int i = 0; i < 100_000; i++)
{
int n = random.Next();

list.Add(n);
dict[n] = n;
set.Add(n);
}

var sw = Stopwatch.StartNew();

int countList = 0;
int countDict = 0;
int countSet = 0;

for (int i = 0; i < 100_000; i++)
{
if (list.Contains(i))
	++countList;
}

sw.Stop();
Console.WriteLine("List: " +sw.ElapsedMilliseconds);
sw = Stopwatch.StartNew();

for (int i = 0; i < 100_000; i++)
{
if (dict.ContainsKey(i))
	++countDict;
}

sw.Stop();

Console.WriteLine("Dict: " + sw.ElapsedMilliseconds);
sw = Stopwatch.StartNew();

for (int i = 0; i < 100_000; i++)
{
if (set.Contains(i))
	++countSet;
}

sw.Stop();

Console.WriteLine("Set: " + sw.ElapsedMilliseconds);

Console.WriteLine("Hello World!");

//*/
