using OpenVolleyScout;
using OpenVolleyScout.Parser;
using System.Collections.Generic;

namespace OpenVolleyScout.Models
{
    public class Player
    {
        public Player(bool isHomeTeam, int playerNum, string? playerName = null)
        {
            IsHomeTeam = isHomeTeam;
            PlayerNum = playerNum;
            PlayerName = playerName;
        }
        public void AddSkill(Skill skill)
        {
            Skills.Add(skill);
        }
        public void RemoveSkill(Skill skill)
        {
            var i = Skills.LastIndexOf(skill);
            Skills.RemoveAt(i);
        }

        public List<Skill> Skills { get; set; } = new();
        public bool IsHomeTeam { get; set; }
        public int PlayerNum { get; set; }
        public string? PlayerName { get; set; }
    }
    public static class PlayerExtension
    {
        public static FormBModel ConvertFormB(this Player player)
        {
            var fm = new FormBModel(player);
            foreach (var skill in player.Skills)
            {
                if (skill.SkillType == SkillType.Serve)
                {
                    fm.Serve.Count++;
                    switch (skill.Evaluation)
                    {
                        case EvaluationEnum.Sharp:
                            fm.Serve.Point++;
                            break;
                        case EvaluationEnum.Plus:
                            fm.Serve.Effect++;
                            break;
                        case EvaluationEnum.Exclamation:
                            break;
                        case EvaluationEnum.Slash:
                            fm.Serve.Effect++;
                            break;
                        case EvaluationEnum.Minus:
                            break;
                        case EvaluationEnum.Equal:
                            fm.Serve.Lost++;
                            break;
                        case null:
                            break;
                    }
                }
                else if (skill.SkillType == SkillType.Attack)
                {
                    fm.Attack.Count++;

                    switch (skill.Evaluation)
                    {
                        case EvaluationEnum.Sharp:
                            fm.Attack.Point++;
                            break;
                        case EvaluationEnum.Plus:
                            break;
                        case EvaluationEnum.Exclamation:
                            break;
                        case EvaluationEnum.Slash:
                            break;
                        case EvaluationEnum.Minus:
                            break;
                        case EvaluationEnum.Equal:
                            fm.Attack.Lost++;
                            break;
                        case null:
                            break;
                    }
                }
                else if (skill.SkillType==SkillType.Block)
                {
                    if (skill.Evaluation == EvaluationEnum.Sharp)
                    {
                        fm.Block.Point++;
                    }
                }
                else if (skill.SkillType==SkillType.Reception)
                {
                    fm.Reception.Count++;
                    switch (skill.Evaluation)
                    {
                        case EvaluationEnum.Sharp:
                            fm.Reception.Perfect++;
                            break;
                        case EvaluationEnum.Plus:
                            fm.Reception.Good++;
                            break;
                        case EvaluationEnum.Exclamation:
                            break;
                        case EvaluationEnum.Slash:
                            break;
                        case EvaluationEnum.Minus:
                            break;
                        case EvaluationEnum.Equal:
                            fm.Reception.Lost++;
                            break;
                        case null:
                            break;
                    }
                }
            }
            return fm;
        }
    }
}
