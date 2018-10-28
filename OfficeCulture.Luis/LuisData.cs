using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OfficeCulture.Luis
{
    public class LuisData
    {
        public string Query { get; set; }
        public IntentData TopScoringIntent { get; set; }
        public IList<EntityData> Entities { get; set; }
        public SentimentAnalysis SentimentAnalysis { get; set; }
    }

    public class IntentData
    {
        public string Intent { get; set; }
        public double Score { get; set; }
    }

    public class EntityData
    {
        public string Entity { get; set; }
        public string Type { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public double Score { get; set; }
    }

    public class SentimentAnalysis
    {
        public string Label { get; set; }
        public double Score { get; set; }
    }
}
 