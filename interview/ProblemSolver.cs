namespace interview
{
    class ExtendedResult
    {
        public KeyValuePair<int, int> Result { get; set; }
        public KeyValuePair<int, List<char>> Vehicles { get; set; }
        public KeyValuePair<int, char> Jobs { get; set; }
    }

    internal class ProblemSolver
    {
      
       public static Dictionary<int, int> FindJobForVehicles( Dictionary<int, List<char>> vehicles, Dictionary<int, char> jobs)
        {
            Dictionary<int, int> result = new();
            //  simple finder
            foreach (KeyValuePair<int, char> job in jobs)
            {
                if (vehicles.Any(v => v.Value.Contains(job.Value) && result.ContainsKey(v.Key) == false))
                {
                    KeyValuePair<int, List<char>> vehicle = vehicles.First(v => v.Value.Contains(job.Value) && result.ContainsKey(v.Key) == false);
                    result.Add(vehicle.Key, job.Key);
                }
            }
            FindJobForFreeVehicles(result, vehicles, jobs);

            return result;
        }

        static void FindJobForFreeVehicles(Dictionary<int, int> result, Dictionary<int, List<char>> vehicles, Dictionary<int, char> jobs)
        {
            List<ExtendedResult> extendedResult = result.Join(vehicles, r => r.Key, v => v.Key, (r, v) => new { result = r, veicheles = v })
                                       .Join(jobs, r => r.result.Value, j => j.Key, (r, j) => new ExtendedResult() { Result = r.result, Vehicles = r.veicheles, Jobs = j }).ToList();

            IEnumerable<KeyValuePair<int, char>> freeJobs = jobs.Where(j => result.ContainsValue(j.Key) == false);

            foreach (KeyValuePair<int, char> freeJob in freeJobs)
            {
                IEnumerable<ExtendedResult> resultRecordsWhatCanBeGoodForSwap = extendedResult.Where(r => r.Jobs.Value != freeJob.Value && r.Vehicles.Value.Contains(freeJob.Value));
                foreach (var resultRecordWhatCanBeGoodForSwap in resultRecordsWhatCanBeGoodForSwap)
                {
                    IEnumerable<KeyValuePair<int, List<char>>> vehiclesWithoutJob = vehicles.Where(v => result.ContainsKey(v.Key) == false);
                    foreach (var vehicleWithoutJob in vehiclesWithoutJob)
                    {
                        if (vehicleWithoutJob.Value.Contains(resultRecordWhatCanBeGoodForSwap.Jobs.Value))
                        {
                            //SWAP
                            SimpleSwap(result, vehicleWithoutJob.Key, resultRecordWhatCanBeGoodForSwap.Result.Key, freeJob.Key);
                            FindJobForFreeVehicles(result, vehicles, jobs);
                            return;
                        }
                    }
                    int rsKey = resultRecordWhatCanBeGoodForSwap.Result.Key;
                    // Search new vehicle for job
                    if (FindVehicleToSeacherJob(resultRecordWhatCanBeGoodForSwap.Jobs, extendedResult.Where(er => er.Result.Key != rsKey).ToList(), vehiclesWithoutJob, result))
                    {
                        result[rsKey] = freeJob.Key;
                        FindJobForFreeVehicles(result, vehicles, jobs);
                        return;
                    }

                }
            }
        }

        // Recursive depth finder
        static bool FindVehicleToSeacherJob(KeyValuePair<int, char> searcherjob, List<ExtendedResult> extendedResult, IEnumerable<KeyValuePair<int, List<char>>> vehiclesWithoutJob, Dictionary<int, int> result)
        {
            var resultRecordWhatCanBeGoodForSwaps = extendedResult.Where(r => r.Jobs.Value != searcherjob.Value && r.Vehicles.Value.Contains(searcherjob.Value));
            foreach (var resultRecordWhatCanBeGoodForSwap in resultRecordWhatCanBeGoodForSwaps)
            {
                if (vehiclesWithoutJob.Any(v => v.Value.Contains(resultRecordWhatCanBeGoodForSwap.Jobs.Value)))
                {
                    KeyValuePair<int, List<char>> goodVehicle = vehiclesWithoutJob.FirstOrDefault(v => v.Value.Contains(resultRecordWhatCanBeGoodForSwap.Jobs.Value));

                    SimpleSwap(result, goodVehicle.Key, resultRecordWhatCanBeGoodForSwap.Result.Key, searcherjob.Key);
                    return true;
                }

                int rsKey = resultRecordWhatCanBeGoodForSwap.Result.Key;
                if (FindVehicleToSeacherJob(resultRecordWhatCanBeGoodForSwap.Jobs, extendedResult.Where(er => er.Result.Key != rsKey).ToList(), vehiclesWithoutJob, result))
                {
                    result[rsKey] = searcherjob.Key;
                    return true;
                }

            }
            return false;
        }

        private static void SimpleSwap(Dictionary<int, int> result, int freeVehicleId, int vehicleIdToBeSwapped, int newJobId) 
        {
            var pairedJobId = result[vehicleIdToBeSwapped];

            result[vehicleIdToBeSwapped] = newJobId;
            result.Add(freeVehicleId, pairedJobId);
        }
    }
}
