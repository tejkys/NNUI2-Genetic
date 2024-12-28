namespace NNUI2_Genetic;

public class PubsPath
{
    public List<Pub> Path { get; set; } = new();
    public double GetTotalDistance()
    {
        double result = 0;
        for (int i = 0; i < Path.Count - 1; i++)
        {
            result += Path[i].GetDistance(Path[i + 1]);
        }
        return result;
    }
}