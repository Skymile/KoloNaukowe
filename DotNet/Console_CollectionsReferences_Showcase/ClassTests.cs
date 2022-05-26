using System.Linq;
// #include <iostream>

struct ArrayStruct
{
	public int Length;
}

class Array
{
	public override string ToString() =>
		string.Join(", ", this.values.Take(this.Length));
	//var sb = new StringBuilder();
	//sb.Append(this.values[0]);
	//	for (int i = 1; i< this.Length; i++)
	//	{
	//		sb.Append(", ");
	//		sb.Append(this.values[i]);
	//}
	//return sb.ToString();

	public void Add(int value)
	{
		if (this.Length >= this.values.Length)
		{
			var newValues = new int[this.values.Length * 2];
			for (int i = 0; i < this.Length; i++)
				newValues[i] = this.values[i];
			values = newValues;
		}

		values[this.Length++] = value;
	}

	public int Length { get; set; }

	private int[] values = new int[4];
}
