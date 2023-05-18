using OpenVolleyScout;
using OpenVolleyScout.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenVolleyScout.Models
{
    public class FormBModel
    {
        public FormBModel(Player player)
        {
            IsHomeTeam = player.IsHomeTeam;
            PlayerNum = player.PlayerNum;
            PlayerName = player.PlayerName;
        }
        public FormBModel(FormBModel model)
        {
            IsHomeTeam = model.IsHomeTeam;
            PlayerNum = model.PlayerNum;
            PlayerName = model.PlayerName;
            Attack = model.Attack;
            Block = model.Block;
            Serve = model.Serve;
            Reception = model.Reception;
        }

        public void Update()
        {
        }
        public void Update(Skill skill)
        {
            if (skill.SkillType == SkillType.Serve)
            {
                Serve.Count++;
                switch (skill.Evaluation)
                {
                    case EvaluationEnum.Sharp:
                        Serve.Point++;
                        break;
                    case EvaluationEnum.Plus:
                        Serve.Effect++;
                        break;
                    case EvaluationEnum.Exclamation:
                        break;
                    case EvaluationEnum.Slash:
                        Serve.Effect++;
                        break;
                    case EvaluationEnum.Minus:
                        break;
                    case EvaluationEnum.Equal:
                        Serve.Lost++;
                        break;
                    case null:
                        break;
                }
            }
            else if (skill.SkillType == SkillType.Attack)
            {
                Attack.Count++;

                switch (skill.Evaluation)
                {
                    case EvaluationEnum.Sharp:
                        Attack.Point++;
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
                        Attack.Lost++;
                        break;
                    case null:
                        break;
                }
            }
            else if (skill.SkillType==SkillType.Block)
            {
                if (skill.Evaluation == EvaluationEnum.Sharp)
                {
                    Block.Point++;
                }
            }
            else if (skill.SkillType==SkillType.Reception)
            {
                Reception.Count++;
                switch (skill.Evaluation)
                {
                    case EvaluationEnum.Sharp:
                        Reception.Perfect++;
                        break;
                    case EvaluationEnum.Plus:
                        Reception.Good++;
                        break;
                    case EvaluationEnum.Exclamation:
                        break;
                    case EvaluationEnum.Slash:
                        break;
                    case EvaluationEnum.Minus:
                        break;
                    case EvaluationEnum.Equal:
                        Reception.Lost++;
                        break;
                    case null:
                        break;
                }
            }
        }
        public void Delete(Skill skill)
        {
            if (skill.SkillType == SkillType.Serve)
            {
                Serve.Count--;
                switch (skill.Evaluation)
                {
                    case EvaluationEnum.Sharp:
                        Serve.Point--;
                        break;
                    case EvaluationEnum.Plus:
                        Serve.Effect--;
                        break;
                    case EvaluationEnum.Exclamation:
                        break;
                    case EvaluationEnum.Slash:
                        Serve.Effect--;
                        break;
                    case EvaluationEnum.Minus:
                        break;
                    case EvaluationEnum.Equal:
                        Serve.Lost--;
                        break;
                    case null:
                        break;
                }
            }
            else if (skill.SkillType == SkillType.Attack)
            {
                Attack.Count--;

                switch (skill.Evaluation)
                {
                    case EvaluationEnum.Sharp:
                        Attack.Point--;
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
                        Attack.Lost--;
                        break;
                    case null:
                        break;
                }
            }
            else if (skill.SkillType==SkillType.Block)
            {
                if (skill.Evaluation == EvaluationEnum.Sharp)
                {
                    Block.Point--;
                }
            }
            else if (skill.SkillType==SkillType.Reception)
            {
                Reception.Count--;
                switch (skill.Evaluation)
                {
                    case EvaluationEnum.Sharp:
                        Reception.Perfect--;
                        break;
                    case EvaluationEnum.Plus:
                        Reception.Good--;
                        break;
                    case EvaluationEnum.Exclamation:
                        break;
                    case EvaluationEnum.Slash:
                        break;
                    case EvaluationEnum.Minus:
                        break;
                    case EvaluationEnum.Equal:
                        Reception.Lost--;
                        break;
                    case null:
                        break;
                }
            }
        }

        public bool IsHomeTeam { get; set; }
        public int? PlayerNum { get; set; }
        public string? PlayerName { get; set; }

        public AttackModel Attack { get; set; } = new();
        public BlockModel Block { get; set; } = new();
        public ServeModel Serve { get; set; } = new();
        public ReceptionModel Reception { get; set; } = new();

        public class AttackModel
        {
            public int Count { get; set; }
            public int Point { get; set; }
            public int Lost { get; set; }
            public double Rate
            {
                get => (double)Point / (double)Count * 100;
            }
        }
        public class BlockModel
        {
            public int Point { get; set; }
        }
        public class ServeModel
        {
            public int Count { get; set; }
            public int Point { get; set; }
            public int Lost { get; set; }
            public int Effect { get; set; }
            public double Rate
            {
                get => (((double)Point * 100) + ((double)Effect * 25) - ((double)Lost * 25))/(double)Count;
            }
        }
        public class ReceptionModel
        {
            public int Count { get; set; }
            public int Perfect { get; set; }
            public int Good { get; set; }
            public int Lost { get; set; }
            public double Rate
            {
                get => (((double)Perfect * 100) + ((double)Good * 25))/(double)Count;
            }
        }
    }
}
