using System;
using System.Collections.Generic;
using Spectre.Console;
using PerceptronTrainer.Models;
using PerceptronTrainer.Data;
using PerceptronTrainer.UI;

namespace PerceptronTrainer
{
    class Program
    {
        static void Main()
        {
            var rule = new Rule("[cyan bold]PERCEPTRON[/]")
                .RuleStyle("cyan")
                .Centered();
            AnsiConsole.Write(rule);
            AnsiConsole.WriteLine();

            // Menu
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Choose training mode:[/]")
                    .AddChoices(new[] {
                        "🍎 Fruit Classifier - Sweet vs Citrus",
                        "🌡️ Temperature Classifier - Hot vs Cold",
                        "📏 Size Classifier - Big vs Small",
                        "Logic Gates (AND, OR, NAND, NOR)",
                        "Custom Training Data",
                        "Interactive Mode - Test Your Own Inputs"
                    }));

            TrainingData? trainingData = null;

            // Get training data based on user choice
            if (choice.StartsWith("🍎 Fruit"))
            {
                trainingData = ClassifierData.GetFruitClassifier();
            }
            else if (choice.StartsWith("🌡️ Temperature"))
            {
                trainingData = ClassifierData.GetTemperatureClassifier();
            }
            else if (choice.StartsWith("📏 Size"))
            {
                trainingData = ClassifierData.GetSizeClassifier();
            }
            else if (choice.StartsWith("Logic Gates"))
            {
                var gate = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Select a logic gate:[/]")
                        .AddChoices(new[] { "AND", "OR", "NAND", "NOR" }));

                trainingData = ClassifierData.GetLogicGate(gate);
            }
            else if (choice.StartsWith("Custom"))
            {
                trainingData = GetCustomTrainingData();
            }
            else // Interactive Mode
            {
                RunInteractiveMode();
                return;
            }

            // Train the perceptron
            AnsiConsole.MarkupLine($"\n[yellow]Training for {trainingData.Name}[/]");
            
            bool showDetails = AnsiConsole.Confirm("[cyan]Show step-by-step training details?[/]", false);
            AnsiConsole.WriteLine();

            Perceptron perceptron = new Perceptron(inputSize: 2, learningRate: 0.1);
            
            if (!showDetails)
            {
                AnsiConsole.Status()
                    .Start("[yellow]Training perceptron...[/]", ctx =>
                    {
                        perceptron.Train(trainingData.Inputs, trainingData.Outputs, epochs: 100, verbose: false);
                    });
                AnsiConsole.WriteLine();
            }
            else
            {
                perceptron.Train(trainingData.Inputs, trainingData.Outputs, epochs: 100, verbose: true);
            }

            // Show test results
            UIHelpers.ShowTestResults(perceptron, trainingData);

            // Interactive testing
            AnsiConsole.WriteLine();
            RunInteractiveTesting(perceptron, trainingData, choice);

            AnsiConsole.MarkupLine("\n[grey]Press any key to exit...[/]");
            Console.ReadKey();
        }

        static TrainingData GetCustomTrainingData()
        {
            var samples = new List<double[]>();
            var outputs = new List<int>();

            AnsiConsole.MarkupLine("\n[yellow]Enter your training data (2 inputs, 1 output)[/]");
            AnsiConsole.MarkupLine("[grey]Example: Input1=0, Input2=1, Output=1[/]\n");

            int numSamples = AnsiConsole.Ask<int>("[cyan]How many training samples?[/] ");

            for (int i = 0; i < numSamples; i++)
            {
                AnsiConsole.MarkupLine($"\n[yellow]Sample {i + 1}:[/]");
                double input1 = AnsiConsole.Ask<double>("  Input 1: ");
                double input2 = AnsiConsole.Ask<double>("  Input 2: ");
                int output = AnsiConsole.Ask<int>("  Expected Output (0 or 1): ");

                samples.Add(new double[] { input1, input2 });
                outputs.Add(output);
            }

            return new TrainingData(samples.ToArray(), outputs.ToArray(), "custom data");
        }

        static void RunInteractiveTesting(Perceptron perceptron, TrainingData data, string mode)
        {
            if (mode.StartsWith("🍎 Fruit"))
            {
                UIHelpers.TestFruitClassifier(perceptron);
            }
            else if (mode.StartsWith("🌡️ Temperature"))
            {
                UIHelpers.TestTemperatureClassifier(perceptron);
            }
            else if (mode.StartsWith("📏 Size"))
            {
                UIHelpers.TestSizeClassifier(perceptron);
            }
            else
            {
                UIHelpers.TestGenericClassifier(perceptron, data);
            }
        }

        static void RunInteractiveMode()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[yellow bold]Interactive Learning Mode[/]\n");
            AnsiConsole.MarkupLine("[grey]The perceptron will learn from your examples in real-time![/]\n");

            var samples = new List<double[]>();
            var outputs = new List<int>();

            Perceptron? perceptron = null;
            int inputSize = 2;

            while (true)
            {
                AnsiConsole.MarkupLine($"[cyan]Current training samples: {samples.Count}[/]");

                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]What do you want to do?[/]")
                        .AddChoices(new[] {
                            "Add training sample",
                            "Train perceptron",
                            "Test input",
                            "Show training data",
                            "Exit"
                        }));

                if (action == "Exit")
                    break;

                switch (action)
                {
                    case "Add training sample":
                        AnsiConsole.WriteLine();
                        double input1 = AnsiConsole.Ask<double>("  Input 1: ");
                        double input2 = AnsiConsole.Ask<double>("  Input 2: ");
                        int output = AnsiConsole.Ask<int>("  Expected Output (0 or 1): ");

                        samples.Add(new double[] { input1, input2 });
                        outputs.Add(output);
                        AnsiConsole.MarkupLine("[green]✓ Sample added![/]\n");
                        break;

                    case "Train perceptron":
                        if (samples.Count < 2)
                        {
                            AnsiConsole.MarkupLine("[red]Add at least 2 samples before training![/]\n");
                            break;
                        }

                        AnsiConsole.WriteLine();
                        AnsiConsole.Status()
                            .Start("[yellow]Training perceptron...[/]", ctx =>
                            {
                                perceptron = new Perceptron(inputSize: inputSize, learningRate: 0.1);
                                perceptron.Train(samples.ToArray(), outputs.ToArray(), epochs: 100, verbose: false);
                            });
                        AnsiConsole.WriteLine();
                        break;

                    case "Test input":
                        if (perceptron == null)
                        {
                            AnsiConsole.MarkupLine("[red]Train the perceptron first![/]\n");
                            break;
                        }

                        AnsiConsole.WriteLine();
                        double test1 = AnsiConsole.Ask<double>("[cyan]Input 1:[/] ");
                        double test2 = AnsiConsole.Ask<double>("[cyan]Input 2:[/] ");

                        int pred = perceptron.Predict(new double[] { test1, test2 });
                        AnsiConsole.MarkupLine($"[bold yellow]Prediction: {pred}[/]\n");
                        break;

                    case "Show training data":
                        if (samples.Count == 0)
                        {
                            AnsiConsole.MarkupLine("[yellow]No training data yet![/]\n");
                            break;
                        }

                        var dataTable = new Table()
                            .Border(TableBorder.Rounded)
                            .Title("[bold]Training Data[/]");

                        dataTable.AddColumn("Input 1");
                        dataTable.AddColumn("Input 2");
                        dataTable.AddColumn("Output");

                        for (int i = 0; i < samples.Count; i++)
                        {
                            dataTable.AddRow(
                                samples[i][0].ToString(),
                                samples[i][1].ToString(),
                                outputs[i].ToString());
                        }

                        AnsiConsole.Write(dataTable);
                        AnsiConsole.WriteLine();
                        break;
                }
            }
        }
    }
}
