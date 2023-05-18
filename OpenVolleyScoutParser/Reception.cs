namespace OpenVolleyScout.Parser
{
    public class Reception : Skill
    {
        public Reception(int playerNumber, TypeOfHitEnum? typeOfHit = null, EvaluationEnum? evaluation = null) : base(playerNumber, SkillType.Reception)
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