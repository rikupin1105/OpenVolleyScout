using System.Drawing;

namespace OpenVolleyScout.Parser
{
    public class Attack : Skill
    {
        public Point? GetEndZoneCoordinates()
        {
            switch (EndZone)
            {
                case 1:
                    return new Point(1800 - 150, 0 + 150);
                case 6:
                    return new Point(1800-150, 450);
                case 5:
                    return new Point(1800-150, 900 - 150);

                case 9:
                    return new Point(1350, 0 + 150);
                case 8:
                    return new Point(1350, 450);
                case 7:
                    return new Point(1350, 900 - 150);

                case 2:
                    return new Point(900 + 150, 0 + 150);
                case 3:
                    return new Point(900 + 150, 450);
                case 4:
                    return new Point(900 + 150, 900 - 150);
            }
            return null;
        }
        public Point? GetStartZoneCoordinates()
        {
            if (Combination is not null)
            {
                return DataVolleyParser.CombinationCommands.First(x => x.Code == Combination).StartPoint;
            }
            else if (StartZone is not null)
            {
                switch (StartZone)
                {
                    case 5:
                        return new Point(0+150, 0+150);
                    case 6:
                        return new Point(0+150, 450);
                    case 1:
                        return new Point(0+150, 900 -150);

                    case 4:
                        return new Point(900 - 150, 0);
                    case 3:
                        return new Point(900 - 150, 450);
                    case 2:
                        return new Point(900 - 150, 900 - 150);

                    case 7:
                        return new Point(450, 0+150);
                    case 8:
                        return new Point(450, 450);
                    case 9:
                        return new Point(450, 900 - 150);
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        public string? Combination { get; set; }
        public Attack(int playerNumber, TypeOfHitEnum? typeOfHit = null, EvaluationEnum? evaluation = null, string? combination = null) : base(playerNumber, SkillType.Attack)
        {
            if (typeOfHit is null)
                TypeOfHit = TypeOfHitEnum.H;
            else
                TypeOfHit = typeOfHit;

            if (combination is not null)
            {
                if (DataVolleyParser.CombinationCommands.Select(x => x.Code).Contains(combination))
                {
                    Combination = combination;
                }
            }

            if (evaluation is not null)
            {
                Evaluation = evaluation;
            }
        }
    }
}