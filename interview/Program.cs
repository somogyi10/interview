using interview;

namespace interview
{
    class Program
    {
        static void Main(string[] args)
        {
            Data data = new();
            Dictionary<int, int> result = ProblemSolver.FindJobForVehicles( data.Vehicles, data.Jobs);

            PrintResult(result, data.Vehicles, data.Jobs);
            ErrorCheck(result, data.Vehicles, data.Jobs);
        }


        static void ErrorCheck(Dictionary<int, int> result, Dictionary<int, List<char>> veicheles, Dictionary<int, char> jobs)
        {
            if (result.Values.Count != result.Values.Distinct().Count())
                Console.WriteLine("Error: Job ids is not unique");

            foreach (KeyValuePair<int, int> resultItem in result)
            {
                if (veicheles.First(v => v.Key == resultItem.Key).Value.Contains(jobs.First(j => j.Key == resultItem.Value).Value) == false)
                { 
                    Console.WriteLine("ERROR: veichle not contains jobs tpye");
                    Console.WriteLine($"Error item {resultItem.Key} {resultItem.Value}");
                }
            }
        }

        static void PrintResult(Dictionary<int, int> result, Dictionary<int, List<char>> veicheles, Dictionary<int, char> jobs)
        {

            foreach (KeyValuePair<int, int> resultItem in result)
            {
                Console.WriteLine($"V:{resultItem.Key} J:{resultItem.Value}  " +
                    $"J:{ jobs.First(j => j.Key == resultItem.Value).Value } V:{ string.Join(" ", veicheles.First(v => v.Key == resultItem.Key).Value) }");

            }

            foreach (var missingVechile in veicheles.Where(v => result.ContainsKey(v.Key) == false))
                Console.WriteLine($"Missing V:{missingVechile.Key} { string.Join(" ", missingVechile.Value) }");
            foreach (var unpairedJobs in jobs.Where(j => result.ContainsValue(j.Key) == false))
                Console.WriteLine($"Missing J:{unpairedJobs.Key} { unpairedJobs.Value} ");
        }

        static void PrintMinimalResult(Dictionary<int, int> result, Dictionary<int, List<char>> veicheles, Dictionary<int, char> jobs)
        {

            foreach (KeyValuePair<int, int> resultItem in result)
            {
                Console.WriteLine($"{resultItem.Key} {resultItem.Value}");

            }
        }
    }
}



