namespace PoemGenerator.BusinessLogic
{
    public class RuleParsingEngine : IRuleParsingEngine
    {
        /// <summary>
        /// The variable rules is a dictionary which drives the poem generation logic
        /// </summary>
        private readonly Dictionary<string, List<List<string>>> rules = new();

        private readonly Random rnd = new();
        private string? poem;
        public string[]? FileLines { get; set; }

        public RuleParsingEngine()
        {
        }

        /// <summary>
        /// This method reads the rule containing file and creates the dictionary which will be used to construct the poem afterwards
        /// </summary>
        /// <param name="FileLines"></param>
        public void PrepareCollections(string[] FileLines)
        {
            string[] RuleKeyValuePairs;
            string[] SecondPart;
            string[] RuleOrCommands;

            foreach (string FileLine in FileLines)
            {
                List<List<string>>? actualRule = new();
                RuleKeyValuePairs = FileLine.Split(new string[] { ": " }, StringSplitOptions.TrimEntries);
                SecondPart = RuleKeyValuePairs[1].Split(' ');
                foreach (string Secondpart in SecondPart)
                {
                    RuleOrCommands = Secondpart.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    var values = new List<string>(RuleOrCommands);
                    actualRule.Add(values);
                }
                rules.Add(RuleKeyValuePairs[0], actualRule);
            }
        }

        /// <summary>
        /// Method that returns a poem based on the rule definition text file
        /// The poem isn't accurate grammatically
        /// </summary>
        /// <returns>a string containing the actual poem</returns>
        public string GetPoem()
        {
            try
            {
                int LineCount = 0;
                List<List<string>>? parsedValue = new();
                //Get the line count. This would be 5 in out test file's case
                if (rules.TryGetValue("POEM", out parsedValue))
                {
                    LineCount = parsedValue.Count;
                }
                //form each line
                for (int i = 0; i < LineCount; i++)
                {
                    EvaluateLineComponent("LINE");
                }
                return poem ?? "";
            }
            catch (Exception e)
            {
                return "Something went wrong while parsing the rule file: " + e.Message;
            }
        }

        /// <summary>
        /// This recursive function parses a word and applies rules contained within the variable "rules"
        /// </summary>
        /// <param name="word"></param>
        public void EvaluateLineComponent(string word)
        {
            List<List<string>>? parsedValue = new();
            if (rules.TryGetValue(word, out parsedValue))
            {
                foreach (List<string> value in parsedValue)
                {
                    int seed = rnd.Next(value.Count);
                    if (value[seed][0] == '$')
                    {
                        //if you find a $, execute the associated command;
                        if (value[seed].Split('$')[1] == "LINEBREAK")
                        {
                            poem += " " + "\r\n";
                        }
                        if (value[seed].Split('$')[1] == "END")
                        {
                            poem += " " + "\r\n";
                            return;
                        }
                    }
                    else if (value[seed][0] == '<')
                    {
                        EvaluateLineComponent(value[seed].Split(new char[] { '<', '>' })[1]);
                    }
                    else
                    {
                        poem += " " + value[seed];
                    }
                }
            }
            return;
        }
    }
}