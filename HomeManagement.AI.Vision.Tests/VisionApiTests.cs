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
                Criterias = new List<ILookUpCriteria>()
                {
                    new TextLookUpCriteria()
                }
            };

            var text = engine.GetAllMatches(vision.RecognitionResult).ToList();

            Assert.IsNotNull(text);
        }

        [TestMethod]
        public void GivenVisionApiMock_WhenSearchingForNumbers_GetPosibleMoney()
        {
            var vision = ReadMock();

            var engine = new Engine
            {
                Criterias = new List<ILookUpCriteria>()
                {
                    new NumberLookUpCriteria(),
                    new MoneyLookUpCriteria { SearchNearRows = true}
                }
            };

            var numbers = engine.GetAllMatches(vision.RecognitionResult).ToList();

            Assert.IsNotNull(numbers);
        }

        [TestMethod]
        public void GivenVisionApiMock_WhenSearchingForDates_GetPosibleDates()
        {
            var vision = ReadMock();

            var engine = new Engine
            {
                Criterias = new List<ILookUpCriteria>()
                {
                    new DateLookUpCriteria()
                }
            };

            var dates = engine.GetAllMatches(vision.RecognitionResult).ToList();

            Assert.IsNotNull(dates);
        }

        private VisionResponse ReadMock()
        {
            var lines = File.ReadAllLines("litoral_gas.txt");

            var sb = new StringBuilder();
            foreach (var line in lines)
            {
                sb.AppendLine(line);
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<VisionResponse>(sb.ToString());
        }
    }
}
