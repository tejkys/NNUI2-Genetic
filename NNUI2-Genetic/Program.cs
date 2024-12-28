using NNUI2_Genetic;

var excel = new ExcelLoader();
var solver = new GeneticAlgorithm(excel.ReadPubs(@"Pubs.xlsx"));
Console.WriteLine($"Result: {solver.FindShortestPath()}m");