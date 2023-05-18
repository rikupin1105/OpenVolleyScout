using OpenVolleyScout.Parser;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;

namespace OpenVolleyScout.Parser
{
    public class DataVolleyParser
    {
        public static List<string> commands = new List<string>();
        public static List<string> AttackCommands = new()
        {
            //文字数が多い順に記述すること

            "AH",
            "AM",
            "AQ",
            "AT",
            "AU",
            "AN",
            "AO",
            "A"
        };
        public static List<string> ServeCommands = new()
        {
            "SH",
            "SM",
            "SQ",
            "S"
        };
        public static List<string> ReceptionCommands = new()
        {
            "RH",
            "RM",
            "RQ",
            "R"
        };
        public static List<string> BlockCommands = new()
        {
            "BH",
            "BM",
            "BQ",
            "BT",
            "BU",
            "BN",
            "BO",
            "B"
        };
        public static List<string> SetterCallCommands = new()
        {
            "KA",
            "KB",
            "KC",
            "KD",
            "KL",
            "K2",
            "K4",
            "K7",
            "K8",
            "K9"
        };
        public static List<AttackDefinition> CombinationCommands = new()
        {
            new("P1",4,TypeOfHitEnum.H,"レフトオープン",new Point(850,50)),
            new("P2",4,TypeOfHitEnum.H,"レフトショートオープン",new Point(850,200) ),
            new("P3",3,TypeOfHitEnum.H,"センターオープン", new Point(850,450)),
            new("P4",2,TypeOfHitEnum.H,"ライトショートオープン",new Point( 850,650)),
            new("P5",2,TypeOfHitEnum.H,"ライトオープン",new Point(850,880)),
            new("P6",2,TypeOfHitEnum.U,"レフトバック", new Point(580,50)),
            new("P7",7,TypeOfHitEnum.U,"パイプ", new Point(580,450)),
            new("P8",8,TypeOfHitEnum.U,"ライトバック", new Point(580,880)),
            new("P9",9,TypeOfHitEnum.U,"Rショートバック", new Point(580,750)),
            new("PG",9,TypeOfHitEnum.U,"前パイプ", new Point(580,400)),
            new("PH",8,TypeOfHitEnum.U,"後パイプ", new Point(580,700)),
            new("PA",3,TypeOfHitEnum.Q,"Aクイック",new Point( 850,450)),

            new("PB",4,TypeOfHitEnum.Q,"Bクイック", new Point(850,280)),
            new("PC",2,TypeOfHitEnum.Q,"Cクイック", new Point(850,620)),
            new("PD",4,TypeOfHitEnum.Q,"Dクイック", new Point(850,750)),
            new("PV",4,TypeOfHitEnum.M,"レフト平行",new Point(850,50)),
            new("PW",3,TypeOfHitEnum.M,"ダブル",new Point(850,200)),
            new("PX",2,TypeOfHitEnum.M,"セミ", new Point( 850,320)),
            new("PY",2,TypeOfHitEnum.M,"バックセミ", new Point( 850,620)),
            new("PZ",2,TypeOfHitEnum.M,"ライト平行" , new Point( 850,880)),
            new("PT",3,TypeOfHitEnum.T,"1人時間差", new Point( 850,450)),
            new("PO",2,TypeOfHitEnum.N,"Cワイド" , new Point( 850,620)),
            new("PR",2,TypeOfHitEnum.N,"ワイド", new Point( 850,620)),
            new("PL",2,TypeOfHitEnum.N,"エル",new Point( 850,640)),
            new("PS",3,TypeOfHitEnum.O,"セッターのツー", new Point( 850,550))
        };
        public static List<string> FreeBallCommands = new List<string>()
        {
            "F"
        };
        public static List<string> SetCommands = new List<string>()
        {
            "E"
        };
        public static List<string> DigCommands = new List<string>()
        {
            "D"
        };
        public DataVolleyParser()
        {
            commands.AddRange(ServeCommands);
            commands.AddRange(ReceptionCommands);
            commands.AddRange(AttackCommands);
            commands.AddRange(SetterCallCommands);
            commands.AddRange(CombinationCommands.Select(x => x.Code));
            commands.AddRange(BlockCommands);
            commands.AddRange(FreeBallCommands);
            commands.AddRange(SetCommands);
            commands.AddRange(DigCommands);

            commands.Add("a");
            commands.Add("*");

            commands.Add("=");
            commands.Add("/");
            commands.Add("-");
            commands.Add("+");
            commands.Add("#");

            commands.Add(".");
        }

        public List<Skill> Parse(string input)
        {
            var ret = new List<Skill>();
            foreach (var item in input.Split(' '))
            {
                var skill = Parsed(item);
                ret.AddRange(skill);
            }
            return ret;
        }
        private static List<string> GetSnipets(string command)
        {
            var snipets = new List<string>();

            var last_index = 0;
            for (int i = 0; i < command.Length; i++)
            {
                for (int j = command.Length - i; j > 0; j--)
                {
                    var sub = command.Substring(i, j);

                    if (commands.Contains(sub))
                    {
                        if (last_index < i)
                        {
                            snipets.Add(command.Substring(last_index, i-last_index));
                        }
                        last_index = j + i;

                        snipets.Add(sub);

                        i += sub.Length - 1;
                        break;
                    }
                }
            }

            if (last_index < command.Length)
            {
                snipets.Add(command.Substring(last_index, command.Length - last_index));
            }

            //Console.WriteLine(string.Join("\n", snipets));
            return snipets;
        }
        private static List<Skill> Parsed(string command)
        {
            var ret = new List<Skill>();

            var bridge = command.Contains(".");
            var bridged = command.Split('.');

            var playerNum = getPlayerNum(command);
            var snipets = GetSnipets(command);
            var skillSnipets = GetSkill(snipets);
            var skill = GetSkill(skillSnipets);
            var typeofhit = GetTypeOfHit(skillSnipets);

            var isHomeTeam = command[0] != 'a';

            if (skill == SkillType.Serve)
            {
                ret.Add(new Serve(playerNum, typeofhit)
                {
                    IsHomeTeam = isHomeTeam,
                    Evaluation = GetEvaluation(bridged[0])
                });

                if (bridge)
                {
                    //ブリッジ入力分
                    var bridge_playerNum = getPlayerNum(bridged[1]);
                    var bridged_snipets = GetSnipets(bridged[1]);
                    var bridged_skillSnipets = GetSkill(bridged_snipets);
                    var eva = GetEvaluation(bridged[1]);

                    var direction = GetDirectionWithBridge(snipets, skillSnipets, playerNum, SkillType.Serve, bridged_skillSnipets, bridge_playerNum, SkillType.Reception);
                    ret[0].StartZone = direction.Item1;
                    ret[0].EndZone = direction.Item2;

                    if (eva is not null && ret[0].Evaluation is null)
                    {
                        switch (eva)
                        {
                            case EvaluationEnum.Sharp:
                                ret[0].Evaluation = EvaluationEnum.Plus;
                                break;
                            case EvaluationEnum.Plus:
                                ret[0].Evaluation = EvaluationEnum.Exclamation;
                                break;
                            case EvaluationEnum.Exclamation:
                                ret[0].Evaluation = EvaluationEnum.Plus;
                                break;
                            case EvaluationEnum.Slash:
                                ret[0].Evaluation = EvaluationEnum.Slash;
                                break;
                            case EvaluationEnum.Equal:
                                ret[0].Evaluation = EvaluationEnum.Sharp;
                                break;
                        }
                    }

                    ret.Add(new Reception(bridge_playerNum, typeofhit, eva)
                    {
                        IsHomeTeam = !isHomeTeam,
                        StartZone = direction.Item1,
                        EndZone = direction.Item2
                    });
                }
                else
                {
                    //ブリッジ入力なし
                    var direction = GetDirection(snipets, skillSnipets, playerNum, SkillType.Serve);
                    ret[0].StartZone = direction.Item1;
                    ret[0].EndZone = direction.Item2;
                }
            }
            else if (skill == SkillType.Attack)
            {
                if (typeofhit == null)
                {
                    var c = CombinationCommands.FirstOrDefault(x => x.Code == skillSnipets);
                    if (c != null)
                    {
                        typeofhit = c.TypeOfHit;
                    }
                }
                ret.Add(new Attack(playerNum, typeofhit, combination: skillSnipets)
                {
                    IsHomeTeam = isHomeTeam,
                    Evaluation = GetEvaluation(bridged[0])
                });

                if (bridge)
                {
                    var bridged_snipets = GetSnipets(bridged[1]);
                    var bridged_skillSnipets = GetSkill(bridged_snipets);
                    var bridged_skill = GetSkill(bridged_skillSnipets);

                    var bridge_playerNum = getPlayerNum(bridged[1]);
                    var eva = GetEvaluation(bridged[1]);


                    if (bridged_skill is null || bridged_skill == SkillType.Block)
                    {
                        //ブリッジ入力分
                        var direction = GetDirectionWithBridge(snipets, skillSnipets, playerNum, SkillType.Attack, bridged_skillSnipets, bridge_playerNum, SkillType.Block);
                        ret[0].StartZone = direction.Item1;
                        ret[0].EndZone = direction.Item2;

                        int? endzone;
                        switch (ret[0].StartZone)
                        {
                            case 1:
                            case 2:
                            case 9:
                                endzone = 4;
                                break;
                            case 6:
                            case 3:
                            case 8:
                                endzone = 3;
                                break;
                            case 4:
                            case 5:
                            case 7:
                                endzone = 2;
                                break;
                            default:
                                endzone = null;
                                break;
                        }
                        if (direction.Item2 is null)
                        {
                            ret[0].EndZone = endzone;
                        }


                        if (eva is not null && ret[0].Evaluation is null)
                        {
                            switch (eva)
                            {
                                case EvaluationEnum.Sharp:
                                    ret[0].Evaluation = EvaluationEnum.Slash;
                                    break;
                                case EvaluationEnum.Plus:
                                    ret[0].Evaluation = EvaluationEnum.Minus;
                                    break;
                                case EvaluationEnum.Exclamation:
                                    ret[0].Evaluation = EvaluationEnum.Exclamation;
                                    break;
                                case EvaluationEnum.Slash:
                                    ret[0].Evaluation = EvaluationEnum.Plus;
                                    break;
                                case EvaluationEnum.Minus:
                                    ret[0].Evaluation = EvaluationEnum.Plus;
                                    break;
                                case EvaluationEnum.Equal:
                                    ret[0].Evaluation = EvaluationEnum.Sharp;
                                    break;
                            }
                        }

                        ret.Add(new Block(bridge_playerNum, typeofhit, eva)
                        {
                            IsHomeTeam = !isHomeTeam,
                            EndZone = endzone
                        });
                    }
                    else if (bridged_skill == SkillType.Dig || bridged_skill == SkillType.Serve)
                    {
                        var direction = GetDirectionWithBridge(snipets, skillSnipets, playerNum, SkillType.Attack, bridged_skillSnipets, bridge_playerNum, SkillType.Dig);
                        ret[0].StartZone = direction.Item1;
                        ret[0].EndZone = direction.Item2;

                        if (eva is not null && ret[0].Evaluation is null)
                        {
                            switch (eva)
                            {
                                case EvaluationEnum.Sharp:
                                    ret[0].Evaluation = EvaluationEnum.Minus;
                                    break;
                                case EvaluationEnum.Plus:
                                    ret[0].Evaluation = EvaluationEnum.Minus;
                                    break;
                                case EvaluationEnum.Exclamation:
                                    ret[0].Evaluation = EvaluationEnum.Plus;
                                    break;
                                case EvaluationEnum.Slash:
                                    ret[0].Evaluation = EvaluationEnum.Plus;
                                    break;
                                case EvaluationEnum.Minus:
                                    ret[0].Evaluation = EvaluationEnum.Plus;
                                    break;
                                case EvaluationEnum.Equal:
                                    ret[0].Evaluation = EvaluationEnum.Sharp;
                                    break;
                            }
                        }
                        ret.Add(new Dig(bridge_playerNum, typeofhit, eva)
                        {
                            IsHomeTeam = !isHomeTeam,
                            StartZone = direction.Item1,
                            EndZone = direction.Item2
                        });
                    }
                    else if (bridged_skill == SkillType.Attack)
                    {
                        var direction = GetDirectionWithBridge(snipets, skillSnipets, playerNum, SkillType.Attack, bridged_skillSnipets, bridge_playerNum, SkillType.Dig);
                        ret[0].StartZone = direction.Item1;
                        ret[0].EndZone = direction.Item2;
                        ret.Add(new Attack(bridge_playerNum, typeofhit, eva, bridged_skillSnipets)
                        {
                            IsHomeTeam = !isHomeTeam,
                            StartZone = direction.Item1,
                            EndZone = direction.Item2
                        });
                    }
                    else if (bridged_skill == SkillType.SET)
                    {
                        var direction = GetDirectionWithBridge(snipets, skillSnipets, playerNum, SkillType.Attack, bridged_skillSnipets, bridge_playerNum, SkillType.Dig);
                        ret[0].StartZone = direction.Item1;
                        ret[0].EndZone = direction.Item2;
                        ret.Add(new Set(bridge_playerNum, typeofhit, eva)
                        {
                            IsHomeTeam = !isHomeTeam,
                            EndZone = direction.Item1
                        });
                    }
                    else if (bridged_skill == SkillType.Reception)
                    {
                        var direction = GetDirectionWithBridge(snipets, skillSnipets, playerNum, SkillType.Attack, bridged_skillSnipets, bridge_playerNum, SkillType.Dig);
                        ret[0].StartZone = direction.Item1;
                        ret[0].EndZone = direction.Item2;
                        ret.Add(new Reception(bridge_playerNum, typeofhit, eva)
                        {
                            IsHomeTeam = !isHomeTeam,
                            StartZone = direction.Item1,
                            EndZone = direction.Item2
                        });
                    }
                    else if (bridged_skill == SkillType.FreeBall)
                    {
                        var direction = GetDirectionWithBridge(snipets, skillSnipets, playerNum, SkillType.Attack, bridged_skillSnipets, bridge_playerNum, SkillType.Dig);
                        ret[0].StartZone = direction.Item1;
                        ret[0].EndZone = direction.Item2;
                        ret.Add(new FreeBall(bridge_playerNum, typeofhit, eva)
                        {
                            IsHomeTeam = !isHomeTeam,
                            StartZone = direction.Item1,
                            EndZone = direction.Item2
                        });
                    }
                }
                else
                {
                    //ブリッジ入力なし
                    var direction = GetDirection(snipets, skillSnipets, playerNum, SkillType.Attack);
                    ret[0].StartZone = direction.Item1;
                    ret[0].EndZone = direction.Item2;
                }
            }
            else if (skill == SkillType.Block)
            {
                ret.Add(new Block(playerNum, typeofhit)
                {
                    IsHomeTeam = isHomeTeam,
                    Evaluation = GetEvaluation(bridged[0])
                });
                if (bridge)
                {
                    //ブリッジ入力分
                    var bridged_snipets = GetSnipets(bridged[1]);
                    var bridged_skillSnipets = GetSkill(bridged_snipets);
                    var bridged_skill = GetSkill(bridged_skillSnipets);

                    var bridge_playerNum = getPlayerNum(bridged[1]);
                    var eva = GetEvaluation(bridged[1]);

                    if (bridged_skill is null || bridged_skill == SkillType.Attack)
                    {
                        switch (eva)
                        {
                            case EvaluationEnum.Sharp:
                                ret[0].Evaluation = EvaluationEnum.Equal;
                                break;
                            case EvaluationEnum.Plus:
                                ret[0].Evaluation = EvaluationEnum.Minus;
                                break;
                            case EvaluationEnum.Exclamation:
                                ret[0].Evaluation = EvaluationEnum.Exclamation;
                                break;
                            case EvaluationEnum.Slash:
                                ret[0].Evaluation = EvaluationEnum.Sharp;
                                break;
                            case EvaluationEnum.Minus:
                                ret[0].Evaluation = EvaluationEnum.Plus;
                                break;
                            case EvaluationEnum.Equal:
                                ret[0].Evaluation = EvaluationEnum.Plus;
                                break;
                        }
                        ret.Insert(0, new Attack(bridge_playerNum, typeofhit, eva, bridged_skillSnipets)
                        {
                            IsHomeTeam = !isHomeTeam
                        });
                    }
                    if (eva is not null && ret[0].Evaluation is null)
                    {
                        switch (eva)
                        {
                            case EvaluationEnum.Sharp:
                                ret[0].Evaluation = EvaluationEnum.Plus;
                                break;
                            case EvaluationEnum.Plus:
                                ret[0].Evaluation = EvaluationEnum.Exclamation;
                                break;
                            case EvaluationEnum.Exclamation:
                                ret[0].Evaluation = EvaluationEnum.Plus;
                                break;
                            case EvaluationEnum.Slash:
                                ret[0].Evaluation = EvaluationEnum.Slash;
                                break;
                            case EvaluationEnum.Equal:
                                ret[0].Evaluation = EvaluationEnum.Sharp;
                                break;
                        }
                    }

                }
                else
                {
                    //ブリッジ入力なし
                    var direction = GetDirection(snipets, skillSnipets, playerNum, SkillType.Block);
                    ret[0].EndZone = direction.Item1;
                }
            }
            else if (skill == SkillType.Dig)
            {
                var direction = GetDirection(snipets, skillSnipets, playerNum, SkillType.Dig);

                ret.Add(new Dig(playerNum, typeofhit)
                {
                    IsHomeTeam = isHomeTeam,
                    Evaluation = GetEvaluation(bridged[0]),
                    StartZone = direction.Item1,
                    EndZone = direction.Item2,
                });
            }
            else if (skill == SkillType.Reception)
            {
                var direction = GetDirection(snipets, skillSnipets, playerNum, SkillType.Reception);

                ret.Add(new Reception(playerNum, typeofhit)
                {
                    IsHomeTeam = isHomeTeam,
                    Evaluation = GetEvaluation(bridged[0]),
                    StartZone = direction.Item1,
                    EndZone = direction.Item2
                });
            }
            else if (skill == SkillType.SET)
            {
                var direction = GetDirection(snipets, skillSnipets, playerNum, SkillType.SET);

                ret.Add(new Set(playerNum, typeofhit)
                {
                    IsHomeTeam = isHomeTeam,
                    Evaluation = GetEvaluation(bridged[0]),
                    StartZone = direction.Item1,
                    EndZone = direction.Item2
                });
            }

            return ret;
        }
        public static int getPlayerNum(string command)
        {
            return int.Parse(Regex.Match(command, @"\d{1,2}").Value);
        }
        public static string GetSkill(List<string> snipets)
        {
            var i = 1;
            if (snipets[0] == "a")
            {
                i++;
            }
            if (snipets.Count==1) return snipets[0];
            return snipets[i];
        }
        public static SkillType? GetSkill(string skillSnipets)
        {
            if (ServeCommands.Contains(skillSnipets))
            {
                return SkillType.Serve;
            }
            else if (ReceptionCommands.Contains(skillSnipets))
            {
                return SkillType.Reception;
            }
            else if (BlockCommands.Contains(skillSnipets))
            {
                return SkillType.Block;
            }
            else if (AttackCommands.Contains(skillSnipets))
            {
                return SkillType.Attack;
            }
            else if (CombinationCommands.Select(x => x.Code).Contains(skillSnipets))
            {
                return SkillType.Attack;
            }
            else if (SetCommands.Contains(skillSnipets))
            {
                return SkillType.SET;
            }
            else if (DigCommands.Contains(skillSnipets))
            {
                return SkillType.Dig;
            }
            else if (FreeBallCommands.Contains(skillSnipets))
            {
                return SkillType.FreeBall;
            }
            else
            {
                return null;
            }
        }
        public static TypeOfHitEnum? GetTypeOfHit(string skillSnipets)
        {
            if (skillSnipets.Contains("H")) return TypeOfHitEnum.H;
            if (skillSnipets.Contains("M")) return TypeOfHitEnum.M;
            if (skillSnipets.Contains("Q")) return TypeOfHitEnum.Q;
            if (skillSnipets.Contains("T")) return TypeOfHitEnum.T;
            if (skillSnipets.Contains("U")) return TypeOfHitEnum.U;
            if (skillSnipets.Contains("N")) return TypeOfHitEnum.N;
            if (skillSnipets.Contains("O")) return TypeOfHitEnum.O;
            else return null;
        }
        public static EvaluationEnum? GetEvaluation(string skillSnipets)
        {
            if (skillSnipets.Contains("#")) return EvaluationEnum.Sharp;
            if (skillSnipets.Contains("+")) return EvaluationEnum.Plus;
            if (skillSnipets.Contains("!")) return EvaluationEnum.Exclamation;
            if (skillSnipets.Contains("/")) return EvaluationEnum.Slash;
            if (skillSnipets.Contains("-")) return EvaluationEnum.Minus;
            if (skillSnipets.Contains("=")) return EvaluationEnum.Equal;
            else return null;
        }
        private static (int?, int?) GetDirection(List<string> snipets, string skillSnipets, int playerNum, SkillType skillType)
        {
            try
            {
                if (snipets[0] == "a") snipets.RemoveAt(0);
                if (snipets[0] == playerNum.ToString()) snipets.RemoveAt(0);
                snipets.Remove(skillSnipets);
                snipets.Remove("#");
                snipets.Remove("+");
                snipets.Remove("-");
                snipets.Remove("/");
                snipets.Remove("!");
                snipets.Remove("=");

                int? startZone = null;
                int? endZone = null;
                if (CombinationCommands.Select(x => x.Code).Contains(skillSnipets))
                {
                    var c = CombinationCommands.First(x => x.Code == skillSnipets);
                    startZone = c.Zone;

                    if (snipets.Count > 0)
                    {
                        var regex = Regex.Match(snipets[0], @"\d{1,1}").Value;
                        endZone = int.Parse(regex);
                    }
                }
                else
                {
                    var regex = Regex.Match(snipets[0], @"\d{1,2}").Value;

                    if (regex.Length == 1)
                    {
                        if (skillType == SkillType.Attack)
                        {
                            startZone = int.Parse(regex);
                        }
                        else
                        {
                            endZone = int.Parse(regex);
                        }
                    }
                    else if (regex.Length == 2)
                    {
                        if (skillType == SkillType.SET)
                        {
                            endZone = int.Parse(regex[0].ToString());
                        }
                        else
                        {
                            startZone = int.Parse(regex[0].ToString());
                            endZone = int.Parse(regex[1].ToString());
                        }
                    }
                    else if (regex.Length == 3)
                    {
                        startZone = int.Parse(regex[0].ToString());
                        endZone = int.Parse(regex[1].ToString());
                    }
                }
                return (startZone, endZone);
            }
            catch (Exception)
            {
                return (null, null);
            }
        }
        private static (int?, int?) GetDirectionWithBridge(List<string> snipets, string skillSnipets, int playerNum, SkillType skillType, string bridge_skillSnipets, int bridge_playerNum, SkillType bridge_skillType)
        {
            try
            {
                if (snipets[0] == "a") snipets.RemoveAt(0);
                if (snipets[0] == playerNum.ToString()) snipets.RemoveAt(0);
                snipets.Remove(skillSnipets);
                var dotIndex = snipets.IndexOf(".");
                if (snipets[dotIndex+1] == bridge_playerNum.ToString())
                {
                    snipets.RemoveAt(dotIndex+1);
                }
                snipets.Remove(".");
                snipets.Remove("#");
                snipets.Remove("+");
                snipets.Remove("-");
                snipets.Remove("/");
                snipets.Remove("!");
                snipets.Remove("=");
                snipets.Remove(bridge_skillSnipets);

                int? startZone = null;
                int? endZone = null;
                if (CombinationCommands.Select(x => x.Code).Contains(skillSnipets))
                {
                    var c = CombinationCommands.First(x => x.Code == skillSnipets);
                    startZone = c.Zone;

                    if (snipets.Count > 0)
                    {
                        var regex = Regex.Match(snipets[0], @"\d{1,1}").Value;
                        endZone = int.Parse(regex);
                    }
                }
                else
                {
                    var regex = Regex.Match(string.Join("", snipets), @"[1-9]{1,2}").Value;

                    if (regex.Length == 1)
                    {
                        if (skillType == SkillType.Attack)
                        {
                            startZone = int.Parse(regex);
                        }
                        else
                        {
                            endZone = int.Parse(regex);
                        }
                    }
                    else if (regex.Length == 2)
                    {
                        if (skillType == SkillType.SET)
                        {
                            endZone = int.Parse(regex[0].ToString());
                        }
                        else
                        {
                            startZone = int.Parse(regex[0].ToString());
                            endZone = int.Parse(regex[1].ToString());
                        }
                    }
                    else if (regex.Length == 3)
                    {
                        startZone = int.Parse(regex[0].ToString());
                        endZone = int.Parse(regex[1].ToString());
                    }
                }
                return (startZone, endZone);
            }
            catch (Exception)
            {
                return (null, null);
            }
        }
    }
}