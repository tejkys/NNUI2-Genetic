using System;
using System.Text;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;

namespace NNUI2_Genetic;

public class GeneticAlgorithm
{
    private const int POPULATION_SIZE = 750;
    private const int ITERATIONS_AMOUNT = 1000;

    private List<Pub> _pubs;
    private List<PubsPath> _population;
    private Random _random = new Random();

    public int Generation { get; private set; } = 0;
    public GeneticAlgorithm(List<Pub> pubs)
    {
        this._pubs = pubs;
        this._population = new();
    }

    public double FindShortestPath()
    {
        FillPopulation();

        for (int i = 0; i < ITERATIONS_AMOUNT; i++)
        {
            Generation += 1;
            Console.WriteLine($"Processing {Generation} / {ITERATIONS_AMOUNT} generation (Distance: {_population.First().GetTotalDistance()})");

            //selection
            var alfaMales = SelectParents();
            //mating
            var kids = Mate(alfaMales);
            //mutate
            Mutate(kids);

            alfaMales.AddRange(kids);

            _population = alfaMales;
        }

        var best = _population.OrderBy(x => x.GetTotalDistance()).First();
        PrintPubs(best.Path);
        return best.GetTotalDistance();
    }

    private void FillPopulation()
    {
        _population.Clear();
        for (int i = 0; i < POPULATION_SIZE; i++)
        {
            _population.Add(new PubsPath() { Path = GetRandomPath() });
        }
    }

    private List<Pub> GetRandomPath()
    {
	    return _pubs.OrderBy(x => Guid.NewGuid()).ToList();
    }
    
    private List<PubsPath> SelectParents()
    {
        //Tournament
        List<PubsPath> result = new();

        for (int i = 0; i < _population.Count - 1; i+=2)
        {
            result.Add(_population[i].GetTotalDistance() < _population[i + 1].GetTotalDistance()
                ? _population[i]
                : _population[i + 1]);
        }
	    return result;
    }
    


    private List<PubsPath> Mate(List<PubsPath> parents)
    {
        List<PubsPath> result = new();
        for (int i = 0; i < parents.Count / 2; i++)
        {
            int separationIndexA = _random.Next(1, (int)(parents[i].Path.Count * 0.5));
            int separationIndexB = _random.Next((int)(parents[i].Path.Count * 0.5) , parents[i].Path.Count);

            var child1 = new PubsPath() { Path = new List<Pub>(new Pub[parents[i].Path.Count]) };
            var child2 = new PubsPath(){Path = new List<Pub>(new Pub[parents[i].Path.Count])};
            //copy genome core
            for (int j = separationIndexA; j < separationIndexB; j++)
            {
                child1.Path[j] = parents[i].Path[j];
                child2.Path[j] = parents[i + parents.Count/2].Path[j];
            }
            //fill rest
            for (int j = 0; j < child1.Path.Count; j++)
            {
                if (child1.Path[j] is null)
                {
                    for (int k = 0; k < parents[i + parents.Count / 2].Path.Count; k++)
                    {
                        if (!child1.Path.Contains(parents[i + parents.Count / 2].Path[k]))
                        {
                            child1.Path[j] = parents[i + parents.Count / 2].Path[k];
                        }
                    }
                }
                if (child2.Path[j] is null)
                {
                    for (int k = 0; k < parents[i].Path.Count; k++)
                    {
                        if (!child2.Path.Contains(parents[i].Path[k]))
                        {
                            child2.Path[j] = parents[i].Path[k];
                        }
                    }
                }
            }
            result.Add(child1);
            result.Add(child2);
        }

        return result;
    }

    private void Mutate(List<PubsPath> paths)
    {
        foreach (var path in paths)
        {
            if (_random.Next(0, 100) < 5)
            {
                int randomIndexA = _random.Next(0, path.Path.Count);
                int randomIndexB = _random.Next(0, path.Path.Count);

                var tmp = path.Path[randomIndexA];
                path.Path[randomIndexA] = path.Path[randomIndexB];
                path.Path[randomIndexB] = tmp;

            }
        }

    }

    private void PrintPubs(List<Pub> pubs)
    {
	    var result = new StringBuilder("#################################\n");
	    foreach (var pub in pubs)
	    {
		    result.AppendLine(pub.ToString());
	    }

	    result.AppendLine("#################################");
        Console.WriteLine(result.ToString());
    }
}