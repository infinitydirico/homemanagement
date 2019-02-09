namespace HomeManagement.AI.Vision.Analysis
{
    public class WordLookUp
    {
        public string Template { get; set; }

        public LookUpCriteria Criteria { get; set; }

        public double GetMatch(string word)
        {
            double percentage = 0;

            int a = 0;
            for (int i = 0; i < word.Length; i++)
            {
                if (i >= Template.Length)
                {
                    a = 0;
                }

                char wordCharacter = word[i];
                char templateCharacter = Template[a];

                if (Criteria.Match(wordCharacter, templateCharacter))
                {
                    percentage += 100 / word.Length;
                }
                a++;
            }

            return percentage;
        }
    }
}
