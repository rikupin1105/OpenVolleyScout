namespace OpenVolleyScout.Parser
{
    public class Skill
    {
        public override string ToString()
        {
            var ret = string.Empty;
            if (IsHomeTeam)
                ret += "*";
            else
                ret += "a";

            ret += PlayerNumber.ToString("00");


            switch (SkillType)
            {
                case SkillType.Serve:
                    ret += "S";
                    break;
                case SkillType.Reception:
                    ret += "R";
                    break;
                case SkillType.Attack:
                    ret += "A";
                    break;
                case SkillType.Block:
                    ret += "B";
                    break;
                case SkillType.Dig:
                    ret += "D";
                    break;
                case SkillType.SET:
                    ret += "E";
                    break;
                case SkillType.FreeBall:
                    ret += "F";
                    break;
            };

            ret += TypeOfHit;

            switch (Evaluation)
            {
                case EvaluationEnum.Sharp:
                    ret += "#";
                    break;
                case EvaluationEnum.Plus:
                    ret += "+";
                    break;
                case EvaluationEnum.Exclamation:
                    ret += "!";
                    break;
                case EvaluationEnum.Slash:
                    ret += "/";
                    break;
                case EvaluationEnum.Minus:
                    ret += "-";
                    break;
                case EvaluationEnum.Equal:
                    ret += "=";
                    break;
                default:
                    ret += "+";
                    break;
            }

            return ret;
        }
        public Skill(int playerNumber, SkillType skillType)
        {
            PlayerNumber = playerNumber;
            SkillType = skillType;
        }
        public bool IsHomeTeam { get; set; }
        public int PlayerNumber { get; set; }
        public SkillType SkillType { get; set; }
        public TypeOfHitEnum? TypeOfHit { get; set; }
        public EvaluationEnum? Evaluation { get; set; }
        public AttackDefinition? AttackDifintion { get; set; }

        public int? StartZone { get; set; }
        public int? EndZone { get; set; }
        public char? EndZonePlus { get; set; }

    }
}