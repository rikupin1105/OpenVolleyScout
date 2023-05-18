namespace OpenVolleyScout.Parser
{
    public class FreeBall : Skill
    {
        public FreeBall(int playerNumber, TypeOfHitEnum? typeOfHit = null, EvaluationEnum? evaluation = null) : base(playerNumber, SkillType.FreeBall)
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