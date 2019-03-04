using HomeManagement.AI.Vision.Analysis;
using HomeManagement.AI.Vision.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using HomeManagement.Core.Extensions;
using System.Linq;
using System.Text;
using System.IO;
using HomeManagement.AI.Vision.Analysis.Criterias;
using System.Collections.Generic;

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
            double realPrice = 0.0;
            var vision = ReadMock();

            var engine = new Engine
            {
                Criterias = new List<ILookUpCriteria>()
                {
                    new MoneyLookUpCriteria()
                }
            };

            var moneyCriteria = new MoneyLookUpCriteria();
            var numbers = engine.GetAllMatches(vision.RecognitionResult).ToList();

            var moneyLine = vision.RecognitionResult.Lines.FirstOrDefault(x => x.Text.Any(c => char.GetUnicodeCategory(c).Equals(System.Globalization.UnicodeCategory.CurrencySymbol)));

            foreach (var number in numbers)
            {
                var line = vision.RecognitionResult.Lines.FirstOrDefault(x => x.Text.Equals(number));

                if(moneyLine.IsOnSameColumn(line) || moneyLine.IsOnSameRow(line))
                {
                    realPrice = double.Parse(line.Text.RemoveEmptySpaces());
                }
            }

            Assert.IsNotNull(numbers);
        }

        [TestMethod]
        public void GivenVisionApiMock_WhenSearchingForDates_GetPosibleDates()
        {
            DateTime realDate;
            var vision = ReadMock();

            var engine = new Engine
            {
                Criterias = new List<ILookUpCriteria>()
                {
                    new DateLookUpCriteria()
                }
            };

            var dates = engine.GetAllMatches(vision.RecognitionResult).ToList();

            foreach (var date in dates)
            {
                TimeSpan result;
                if(!TimeSpan.TryParse(date, out result))
                {
                    realDate = DateTime.Parse(date);
                }
            }            

            Assert.IsNotNull(dates);
        }

        private VisionResponse ReadMock()
        {
            var lines = File.ReadAllLines("mock.txt");

            var sb = new StringBuilder();
            foreach (var line in lines)
            {
                sb.AppendLine(line);
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<VisionResponse>(sb.ToString());
        }
    }
}
