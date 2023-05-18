using System.Drawing;

namespace OpenVolleyScout.Parser
{
    public class AttackDefinition
    {
        public AttackDefinition(string code, int zone, TypeOfHitEnum typeOfHit, string name, Point startPoint)
        {
            Code=code;
            Zone=zone;
            TypeOfHit=typeOfHit;
            Name=name;
            StartPoint = startPoint;
        }
        public Point StartPoint { get; set; }
        public string Code { get; set; }
        public int Zone { get; set; }
        public TypeOfHitEnum TypeOfHit { get; set; }
        public string Name { get; set; }
    }
}