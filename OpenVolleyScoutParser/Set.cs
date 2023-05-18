namespace OpenVolleyScout.Parser
{
    public class Set : Skill
    {
        public string Call { get; set; }
        public Set(int playerNumber, TypeOfHitEnum? typeOfHit = null, EvaluationEnum? evaluation = null) : base(playerNumber, SkillType.SET)
        {
            if (typeOfHit is null)
                TypeOfHit = TypeOfHitEnum.H;
            else
                TypeOfHit = typeOfHit;

            if (evaluation is not null)
            {
                Evaluation = evaluation;
            }
        }
    }
}