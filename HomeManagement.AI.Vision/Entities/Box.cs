using Newtonsoft.Json;
using System;

namespace HomeManagement.AI.Vision.Entities
{
    public class Box
    {
        [JsonProperty("boundingBox")]
        public string[] BoundingBox { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        public int TopLeftX => BoundingBox.Length > 0 ? int.Parse(BoundingBox[0]) : default(int);
        public int TopLeftY => BoundingBox.Length > 0 ? int.Parse(BoundingBox[1]) : default(int);
        public int TopRightX => BoundingBox.Length > 0 ? int.Parse(BoundingBox[2]) : default(int);
        public int TopRightY => BoundingBox.Length > 0 ? int.Parse(BoundingBox[3]) : default(int);
        public int BottomLeftX => BoundingBox.Length > 0 ? int.Parse(BoundingBox[4]) : default(int);
        public int BottomLeftY => BoundingBox.Length > 0 ? int.Parse(BoundingBox[5]) : default(int);
        public int BottomRightX => BoundingBox.Length > 0 ? int.Parse(BoundingBox[6]) : default(int);
        public int BottomRightY => BoundingBox.Length > 0 ? int.Parse(BoundingBox[7]) : default(int);

        public override bool Equals(object obj)
        {
            if (obj is Box)
            {
                var b = obj as Box;
                return b.TopLeftX.Equals(TopLeftX) &&
                    b.TopLeftY.Equals(TopLeftY) &&
                    b.TopRightX.Equals(TopRightX) &&
                    b.TopRightY.Equals(TopRightY) &&
                    b.BottomLeftX.Equals(BottomLeftX) &&
                    b.BottomLeftY.Equals(BottomLeftY) &&
                    b.BottomRightX.Equals(BottomRightX) &&
                    b.BottomRightY.Equals(BottomRightY);
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool IsOnSameColumn(Box box)
        {
            var topLeftXDiff = Math.Abs(TopLeftX - box.TopLeftX);
            var bottomLeftXDiff = Math.Abs(BottomLeftX - box.BottomLeftX);

            var topRightXDiff = Math.Abs(TopRightX - box.TopRightX);
            var bottomRightXDiff = Math.Abs(BottomRightX - box.BottomRightX);

            return (topLeftXDiff < 50) && (bottomLeftXDiff < 50) &&
                (topRightXDiff < 50) && (bottomRightXDiff < 50);
        }

        public bool IsOnSameRow(Box box)
        {
            var topLeftYDiff = Math.Abs(TopLeftY - box.TopLeftY);
            var bottomLeftYDiff = Math.Abs(BottomLeftY - box.BottomLeftY);

            var topRightYDiff = Math.Abs(TopRightY - box.TopRightY);
            var bottomRightYDiff = Math.Abs(BottomRightY - box.BottomRightY);

            return (topLeftYDiff < 50) && (bottomLeftYDiff < 50) &&
                (topRightYDiff < 50) && (bottomRightYDiff < 50);
        }
    }
}
