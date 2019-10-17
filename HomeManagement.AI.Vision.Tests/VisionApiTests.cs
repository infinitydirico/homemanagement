using HomeManagement.AI.Vision.Analysis;
using HomeManagement.AI.Vision.Analysis.Criterias;
using HomeManagement.AI.Vision.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HomeManagement.AI.Vision.Tests
{
    [TestClass]
    public class VisionApiTests
    {
        [TestMethod]
        public void GivenVisionApiMock_WhenSearchingForPlainText_GetPosiblePlainText()
        {
            var vision = ReadMock();

            var engine = new Engine
            {
                Criterias = new List<IMatch>()
                {
                    new TextLookUpCriteria()
                }
            };

            var text = engine.GetAllMatches(vision.RecognitionResult.First()).ToList();

            Assert.IsNotNull(text);
        }

        [TestMethod]
        public void GivenVisionApiMock_WhenSearchingForNumbers_GetPosibleMoney()
        {
            var vision = ReadMock();

            var engine = new Engine
            {
                Criterias = new List<IMatch>()
                {
                    //new NumberLookUpCriteria(),
                    new MoneyLookUpCriteria()
                }
            };

            var numbers = engine.GetAllMatches(vision.RecognitionResult.First()).ToList();

            Assert.IsNotNull(numbers);
        }

        [TestMethod]
        public void GivenVisionApiMock_WhenSearchingForDates_GetPosibleDates()
        {
            var vision = ReadMock();

            var engine = new Engine
            {
                Criterias = new List<IMatch>()
                {
                    new DateLookUpCriteria()
                }
            };

            var dates = engine.GetAllMatches(vision.RecognitionResult).ToList();

            Assert.IsNotNull(dates);
        }

        private VisionResponseV2 ReadMock()
        {
            var lines = File.ReadAllLines("mock1.txt");

            var sb = new StringBuilder();
            foreach (var line in lines)
            {
                sb.AppendLine(line);
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<VisionResponseV2>(sb.ToString());
        }
    }
}
