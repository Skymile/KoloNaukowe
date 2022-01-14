using System.Collections.Generic;
using System.Linq;

namespace KeystrokeDynamics
{
	public class Classifiers
	{
		public static int KNN(SampleSet current, List<SampleSet> training, int k, Distance distance) => (
			from j in (
				from i in training
				let dist = distance(current.Dwells, i.Dwells)
				orderby dist
				select (Distance: dist, Id: i.Id)
			).Take(k)
			group j by j.Id into p
			orderby p.Count() descending
			select p.First()
		).First().Id;
	}
}

//    o
//  zz  y  y    y

// 3NN
//z:2
//y:1

// 5NN
//y:3
//z:2
