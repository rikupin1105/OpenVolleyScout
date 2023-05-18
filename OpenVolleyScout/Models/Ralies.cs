using OpenVolleyScout;
using OpenVolleyScout.Parser;
using System.Collections.Generic;

namespace OpenVolleyScout.Models
{
    public class Ralies
    {
        public Ralies(string? input, List<Skill>? rallies, string rallyId)
        {
            Input = input;
            Rallies = rallies;
            RallyId = rallyId;
        }
        public string RallyId { get; set; }
        public string? Input { get; set; }
        /// <summary>
        /// セット
        /// </summary>
        public int Set { get; set; }
        /// <summary>
        /// ホームチームの得点
        /// </summary>
        public int HomePoint { get; set; }

        /// <summary>
        /// アウェイチームの得点
        /// </summary>
        public int AwayPoint { get; set; }
        public List<Skill>? Rallies { get; set; } = new();
    }
}

