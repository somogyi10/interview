namespace interview
{
    internal class Data
    {
        static readonly HttpClient client = new HttpClient();

        public Data()
        {
            Vehicles = new Dictionary<int, List<char>>();
            Jobs = new Dictionary<int, char>();

            Task.Run(() => this.FillUpData()).Wait();
        }

        public Dictionary<int, List<char>> Vehicles { get; set; }
        public Dictionary<int, char> Jobs { get; set; }
        //  Dictionary<int, List<char>> vehicles = new Dictionary<int, List<char>>();
        //  Dictionary<int, char> jobs = new Dictionary<int, char>();

        private async Task FillUpData()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://nexogenshares.blob.core.windows.net/recruitment/mediordev.txt");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();


                string[] lines = responseBody.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                // Vehicles
                int numberOfVehicles = Int32.Parse(lines[0]);
                for (int i = 1; i <= numberOfVehicles; i++)
                {
                    string[] items = lines[i].Split(" ");
                    List<char> jobTypeList = items[1..].Select(item => char.Parse(item)).ToList();
                    Vehicles.Add(Int32.Parse(items[0]), jobTypeList);
                }

                // Jobs
                int numberOfJobs = Int32.Parse(lines[numberOfVehicles + 1]);
                for (int i = numberOfVehicles +2; i <= numberOfJobs + numberOfVehicles +1 ; i++)
                {
                    string[] items = lines[i].Split(" ");
                    Jobs.Add(Int32.Parse(items[0]), char.Parse(items[1]));
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}
