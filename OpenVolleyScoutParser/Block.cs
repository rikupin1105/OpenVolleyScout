namespace OpenVolleyScout.Parser
{
    public class Block : Skill
    {
        public Block(int playerNumber, TypeOfHitEnum? typeOfHit = null, EvaluationEnum? evaluation = null) : base(playerNumber, SkillType.Block)
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