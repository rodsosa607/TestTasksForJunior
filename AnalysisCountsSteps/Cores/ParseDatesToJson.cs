namespace AnalysisCountsSteps.Cores
{
    public class ParseDatesToJson
    {
        public string name { get; set; }
        public string status { get; set; }
        public int average { get; set; }
        public int best_res { get; set; }
        public int worst_res { get; set; }
        public int rank { get; set; }
        public ParseDatesToJson(string name, int average, int best_res, int worst_res, string status, int rank)
        {
            this.name = name;
            this.average = average;
            this.best_res = best_res;
            this.worst_res = worst_res;
            this.status = status;
            this.rank = rank;
        }      
    }
}
