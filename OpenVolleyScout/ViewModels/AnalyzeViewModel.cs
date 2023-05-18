using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenVolleyScout.Models;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using OpenVolleyScout;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Windows.Media.Effects;
using System.Reactive.Linq;
using OpenVolleyScout.Parser;

namespace OpenVolleyScout.ViewModels
{
    public class AnalyzeViewModel : BindableBase
    {
        public ReactiveProperty<string> SelectedTeam { get; set; } = new("ホームチーム");
        public ObservableCollection<string> Team { get; set; } = new()
        {
            "ホームチーム",
            "アウェイチーム",
            "両方"
        };

        public ReactiveProperty<string> SelectedSkill { get; set; } = new("アタック");
        public ObservableCollection<string> Skill { get; set; } = new()
        {
            "アタック",
            "サーブ"
        };

        public ObservableCollection<string> Players { get; set; } = new();
        public ObservableCollection<Path> Paths { get; set; } = new();
        public ObservableCollection<ISeries> Series { get; set; } = new();

        public Axis[] XAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    MaxLimit=1000,
                    MinLimit =-100,
                    MinStep=300,
                    ForceStepToMin=true,

                    SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
                    {
                        StrokeThickness = 2,
                        PathEffect = new DashEffect(new float[] { 3, 3 })
                    }
                }
            };
        public Axis[] YAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    MaxLimit=1900,
                    MinLimit =-100,
                    MinStep=300,
                    ForceStepToMin=true,

                    SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
                    {
                        StrokeThickness = 2,
                        PathEffect = new DashEffect(new float[] { 3, 3 })
                    }
                }
            };


        private readonly Scouting _scouting;
        public AnalyzeViewModel(Scouting scouting)
        {
            _scouting = scouting;

            var players = _scouting.Players.ToReadOnlyReactiveCollection();
            players.ObserveAddChanged().Subscribe(_ =>
            {
                PlayersComboBoxUpdate(players);
                RePlot(SelectedSkill.Value, players);
            });
            players.ObserveReplaceChanged().Subscribe(_ =>
            {
                RePlot(SelectedSkill.Value, players);
            });

            SelectedTeam.Subscribe(x =>
            {
                PlayersComboBoxUpdate(players);
                RePlot(SelectedSkill.Value, players);
            });

            SelectedSkill.Subscribe(selectedskill =>
            {
                RePlot(selectedskill, players);
            });
        }

        public void RePlot(string selectedskill, ReadOnlyReactiveCollection<Player> players)
        {
            Series.Clear();
            Series.Add(new LineSeries<Point>
            {
                GeometrySize = 0,
                Fill = null,
                Mapping = (x, y) =>
                {
                    y.PrimaryValue = x.X;
                    y.SecondaryValue = x.Y;
                },
                Values = new Point[]
                {
                    new Point(0,0),
                    new Point(0,900)
                },
                Stroke = new SolidColorPaint
                {
                    Color = SKColors.White,
                    StrokeThickness = 2
                },
            });
            Series.Add(new LineSeries<Point>
            {
                GeometrySize = 0,
                Fill = null,
                Mapping = (x, y) =>
                {
                    y.PrimaryValue = x.X;
                    y.SecondaryValue = x.Y;
                },
                Values = new Point[]
                {
                    new Point(0,900),
                    new Point(1800,900)
                },
                Stroke = new SolidColorPaint
                {
                    Color = SKColors.White,
                    StrokeThickness = 2
                },
            });
            Series.Add(new LineSeries<Point>
            {
                GeometrySize = 0,
                Fill = null,
                Mapping = (x, y) =>
                {
                    y.PrimaryValue = x.X;
                    y.SecondaryValue = x.Y;
                },
                Values = new Point[]
                {
                    new Point(0,0),
                    new Point(1800,0)
                },
                Stroke = new SolidColorPaint
                {
                    Color = SKColors.White,
                    StrokeThickness = 2
                },
            });
            Series.Add(new LineSeries<Point>
            {
                GeometrySize = 0,
                Fill = null,
                Mapping = (x, y) =>
                {
                    y.PrimaryValue = x.X;
                    y.SecondaryValue = x.Y;
                },
                Values = new Point[]
                {
                    new Point(1800,0),
                    new Point(1800,900)
                },
                Stroke = new SolidColorPaint
                {
                    Color = SKColors.White,
                    StrokeThickness = 2
                },
            });
            Series.Add(new LineSeries<Point>
            {
                GeometrySize = 0,
                Fill = null,
                Mapping = (x, y) =>
                {
                    y.PrimaryValue = x.X;
                    y.SecondaryValue = x.Y;
                },
                Values = new Point[]
                {
                    new Point(900,0),
                    new Point(900,900)
                },
                Stroke = new SolidColorPaint
                {
                    Color = SKColors.White,
                    StrokeThickness = 2
                },
            });

            IEnumerable<Player> p;
            if (SelectedTeam.Value == "ホームチーム")
                p = players.Where(x => x.IsHomeTeam==true);
            else if (SelectedTeam.Value=="アウェイチーム")
                p = players.Where(x => x.IsHomeTeam==false);
            else
                p = players;

            var pp = p.Select(x => x.Skills);

            if (selectedskill == "アタック")
            {
                var i = pp.SelectMany(item => item
                    .Where(x => x.SkillType==SkillType.Attack)
                    .Where(item => item.StartZone is not null && item.EndZone is not null));

                PlotAttack(i.ToList());
            }
            else if (selectedskill =="サーブ")
            {
                var i = pp.SelectMany(item => item
                    .Where(x => x.SkillType==SkillType.Serve)
                    .Where(item => item.StartZone is not null && item.EndZone is not null));

                PlotServe(i.ToList());
            }
        }

        private void PlotAttack(List<Skill> skills)
        {
            var zoneCount = new List<(int, char?)>();
            var zones = new List<(int, char?)>
            {
                new(1, null),
                new(2, null),
                new(3, null),
                new(4, null),
                new(5, null),
                new(6, null),
                new(7, null),
                new(8, null),
                new(9, null)
            };

            foreach (var zone in zones)
            {
                var skill1 = skills.Where(x => x.EndZone==zone.Item1 && x.EndZonePlus==zone.Item2);
                var i = 0;
                foreach (var skill in skill1)
                {
                    var color = SKColors.CornflowerBlue;
                    if (skill.Evaluation == EvaluationEnum.Sharp)
                    {
                        color = SKColors.GreenYellow;
                    }
                    else if (skill.Evaluation == EvaluationEnum.Equal)
                    {
                        color = SKColors.Red;
                    }

                    var aaa = 100.0 / skill1.Count();

                    double offset = 0;
                    if (i%2==0)
                        offset = aaa * i;
                    else
                        offset = aaa*-1 * i;

                    var c = zoneCount.Count(x => x.Item1 == skill.EndZone && x.Item2 == skill.EndZone);


                    Series.Add(new LineSeries<Point>
                    {
                        GeometrySize = 0,
                        Fill = null,
                        Mapping = (x, y) =>
                        {
                            y.PrimaryValue = x.X;
                            y.SecondaryValue = x.Y;
                        },
                        Values = new Point[]
                        {
                            new Point(
                                ((Attack)skill).GetStartZoneCoordinates()!.Value.X,
                                ((Attack)skill).GetStartZoneCoordinates()!.Value.Y),

                            new Point(
                                ((Attack)skill).GetEndZoneCoordinates()!.Value.X,
                                ((Attack)skill).GetEndZoneCoordinates()!.Value.Y + offset)
                        },
                        Tag = null,
                        Stroke = new SolidColorPaint
                        {
                            Color = color,
                            StrokeThickness = 1
                        },

                    });
                    i++;
                }
            }
        }
        private void PlotServe(List<Skill> skills)
        {
            var zoneCount = new List<(int, char?)>();
            var zones = new List<(int, char?)>
            {
                new(4, null),
                new(7, null),
                new(5, null),
                new(3, null),
                new(8, null),
                new(6, null),
                new(2, null),
                new(9, null),
                new(1, null)
            };

            foreach (var zone in zones)
            {
                var skill1 = skills.Where(x => x.EndZone==zone.Item1 && x.EndZonePlus==zone.Item2);
                var i = 0;
                var aaa = 200.0 / skill1.Count();
                foreach (var skill in skill1.OrderBy(x => x.StartZone == 1)
                    .ThenBy(x => x.StartZone == 9)
                    .ThenBy(x => x.StartZone == 6)
                    .ThenBy(x => x.StartZone == 7)
                    .ThenBy(x => x.StartZone == 5))
                {
                    var color = SKColors.CornflowerBlue;
                    if (skill.Evaluation == EvaluationEnum.Sharp)
                    {
                        color = SKColors.GreenYellow;
                    }
                    else if (skill.Evaluation == EvaluationEnum.Equal)
                    {
                        color = SKColors.Red;
                    }

                    double offset = aaa * i;
                    if (skill1.Count() == 1)
                        offset = 75;

                    var c = zoneCount.Count(x => x.Item1 == skill.EndZone && x.Item2 == skill.EndZone);


                    Series.Add(new LineSeries<Point>
                    {
                        GeometrySize = 0,
                        Fill = null,
                        Mapping = (x, y) =>
                        {
                            y.PrimaryValue = x.X;
                            y.SecondaryValue = x.Y;
                        },
                        Values = new Point[]
                        {
                            new Point(
                                ((Serve)skill).GetStartZoneCoordinates()!.Value.X,
                                ((Serve)skill).GetStartZoneCoordinates()!.Value.Y),

                            new Point(
                                ((Serve)skill).GetEndZoneCoordinates()!.Value.X,
                                ((Serve)skill).GetEndZoneCoordinates()!.Value.Y - 100 + offset)
                        },
                        Tag = null,
                        Stroke = new SolidColorPaint
                        {
                            Color = color,
                            StrokeThickness = 1
                        },

                    });
                    i++;
                }
            }

        }
        private void PlayersComboBoxUpdate(ReadOnlyReactiveCollection<Player> players)
        {
            if (SelectedTeam.Value == "ホームチーム")
            {
                Players.Clear();
                Players.AddRange(players.Where(p => p.IsHomeTeam == true).Select(p =>
                {
                    if (p.PlayerName is null)
                        return p.PlayerNum.ToString();

                    else
                        return p.PlayerNum + " - "+ p.PlayerName;
                }));
            }
            else if (SelectedTeam.Value =="アウェイチーム")
            {
                Players.Clear();
                Players.AddRange(players.Where(p => p.IsHomeTeam == false).Select(p =>
                {
                    if (p.PlayerName is null)
                        return p.PlayerNum.ToString();

                    else
                        return p.PlayerNum + " - "+ p.PlayerName;
                }));
            }
            else
            {
                //両チーム
                Players.Clear();
                Players.AddRange(players.Select(p =>
                {
                    if (p.PlayerName is null)
                        return p.PlayerNum.ToString();

                    else
                        return p.PlayerNum + " - "+ p.PlayerName;
                }));
            }
        }
    }
}
