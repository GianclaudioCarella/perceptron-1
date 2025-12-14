using System;
using PerceptronTrainer.Models;
using Spectre.Console;

namespace PerceptronTrainer.UI
{
    public static class UIHelpers
    {
        public static void ShowTestResults(Perceptron perceptron, TrainingData data)
        {
            if (data.Inputs.Length == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No test data available.[/]");
                return;
            }
            
            AnsiConsole.WriteLine();
            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Grey)
                .Title("[bold cyan]Test Results[/]");

            // Add columns dynamically based on input size
            int inputSize = data.Inputs[0].Length;
            for (int i = 0; i < inputSize; i++)
            {
                string label = (data.InputLabels != null && i < data.InputLabels.Length) 
                    ? data.InputLabels[i] 
                    : $"Input {i + 1}";
                table.AddColumn(new TableColumn($"[yellow]{label}[/]").Centered());
            }
            
            table.AddColumn(new TableColumn("[yellow]Output[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Expected[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Result[/]").Centered());

            int correct = 0;
            for (int i = 0; i < data.Inputs.Length; i++)
            {
                int prediction = perceptron.Predict(data.Inputs[i]);
                bool isCorrect = prediction == data.Outputs[i];
                if (isCorrect) correct++;

                string resultText = isCorrect ? "[green]✓ PASS[/]" : "[red]✗ FAIL[/]";

                // Build row data dynamically
                var rowData = new List<string>();
                for (int j = 0; j < data.Inputs[i].Length; j++)
                {
                    rowData.Add(data.Inputs[i][j].ToString());
                }
                rowData.Add(prediction.ToString());
                rowData.Add(data.Outputs[i].ToString());
                rowData.Add(resultText);
                
                table.AddRow(rowData.ToArray());
            }

            AnsiConsole.Write(table);

            double accuracy = correct * 100.0 / data.Inputs.Length;
            var panel = new Panel($"[bold green]{correct}/{data.Inputs.Length}[/] tests passed ([bold]{accuracy:F1}%[/] accuracy)")
                .Border(BoxBorder.Rounded)
                .BorderColor(accuracy == 100 ? Color.Green : Color.Yellow)
                .Header("[bold]Summary[/]");

            AnsiConsole.Write(panel);
        }

        public static void TestFruitClassifier(Perceptron perceptron)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold yellow]🍎 Let's classify some fruits![/]\n");
            AnsiConsole.MarkupLine("[grey]Tell me about a fruit and I'll tell you if it's Sweet or Citrus[/]\n");
            
            while (true)
            {
                double sweetness = AnsiConsole.Ask<double>("[cyan]How sweet is the fruit? (0-10):[/] ");
                double colorIntensity = AnsiConsole.Ask<double>("[cyan]How intense is the color? (0-10):[/] ");

                int prediction = perceptron.Predict(new double[] { sweetness, colorIntensity });
                string result = prediction == 0 ? "🍋 [cyan bold]CITRUS[/]" : "🍎 [green bold]SWEET[/]";
                
                AnsiConsole.MarkupLine($"\n{result}");
                
                string explanation = prediction == 0 
                    ? "  This fruit is likely a citrus like lemon, lime, or grapefruit!" 
                    : "  This fruit is likely a sweet fruit like apple, banana, or strawberry!";
                AnsiConsole.MarkupLine($"[grey]{explanation}[/]\n");

                // Ask for feedback
                var feedback = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Was I correct?[/]")
                        .AddChoices(new[] { "✓ Yes, correct!", "✗ No, it's Sweet (1)", "✗ No, it's Citrus (0)" }));

                if (feedback.StartsWith("✗"))
                {
                    int correctAnswer = feedback.Contains("Sweet") ? 1 : 0;
                    perceptron.TrainOnSingleSample(new double[] { sweetness, colorIntensity }, correctAnswer);
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]Great! 🎉[/]");
                }
                
                if (!AnsiConsole.Confirm("[yellow]Test another fruit?[/]", true))
                    break;
                
                AnsiConsole.WriteLine();
            }
        }

        public static void TestTemperatureClassifier(Perceptron perceptron)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold yellow]🌡️ Weather Classification![/]\n");
            AnsiConsole.MarkupLine("[grey]Tell me the temperature and humidity, and I'll tell you if it's Hot or Cold[/]\n");
            
            while (true)
            {
                double temperature = AnsiConsole.Ask<double>("[cyan]What's the temperature? (°C):[/] ");
                double humidity = AnsiConsole.Ask<double>("[cyan]What's the humidity? (%):[/] ");

                int prediction = perceptron.Predict(new double[] { temperature, humidity });
                string result = prediction == 0 ? "❄️  [cyan bold]COLD[/]" : "🔥 [red bold]HOT[/]";
                
                AnsiConsole.MarkupLine($"\n{result}");
                
                string suggestion = prediction == 0 
                    ? "  Better bring a jacket! ❄️" 
                    : "  Stay hydrated and find some shade! ☀️";
                AnsiConsole.MarkupLine($"[grey]{suggestion}[/]\n");

                // Ask for feedback
                var feedback = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Was I correct?[/]")
                        .AddChoices(new[] { "✓ Yes, correct!", "✗ No, it's Hot (1)", "✗ No, it's Cold (0)" }));

                if (feedback.StartsWith("✗"))
                {
                    int correctAnswer = feedback.Contains("Hot") ? 1 : 0;
                    perceptron.TrainOnSingleSample(new double[] { temperature, humidity }, correctAnswer);
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]Great! 🎉[/]");
                }
                
                if (!AnsiConsole.Confirm("[yellow]Test another weather condition?[/]", true))
                    break;
                
                AnsiConsole.WriteLine();
            }
        }

        public static void TestSizeClassifier(Perceptron perceptron)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold yellow]📏 Object Size Classification![/]\n");
            AnsiConsole.MarkupLine("[grey]Tell me about an object and I'll tell you if it's Big or Small[/]\n");
            
            while (true)
            {
                double weight = AnsiConsole.Ask<double>("[cyan]What's the weight? (kg):[/] ");
                double height = AnsiConsole.Ask<double>("[cyan]What's the height? (cm):[/] ");

                int prediction = perceptron.Predict(new double[] { weight, height });
                string result = prediction == 0 ? "📦 [cyan bold]SMALL[/]" : "📏 [green bold]BIG[/]";
                
                AnsiConsole.MarkupLine($"\n{result}");
                
                string category = prediction == 0 
                    ? "  This is a small object, easy to carry!" 
                    : "  This is a big object, might need help to move it!";
                AnsiConsole.MarkupLine($"[grey]{category}[/]\n");

                // Ask for feedback
                var feedback = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Was I correct?[/]")
                        .AddChoices(new[] { "✓ Yes, correct!", "✗ No, it's Big (1)", "✗ No, it's Small (0)" }));

                if (feedback.StartsWith("✗"))
                {
                    int correctAnswer = feedback.Contains("Big") ? 1 : 0;
                    perceptron.TrainOnSingleSample(new double[] { weight, height }, correctAnswer);
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]Great! 🎉[/]");
                }
                
                if (!AnsiConsole.Confirm("[yellow]Test another object?[/]", true))
                    break;
                
                AnsiConsole.WriteLine();
            }
        }

        public static void TestGenericClassifier(Perceptron perceptron, TrainingData data)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[yellow]Test with your own inputs![/]\n");

            string label1 = data.InputLabels?[0] ?? "Input 1";
            string label2 = data.InputLabels?[1] ?? "Input 2";

            while (true)
            {
                double input1 = AnsiConsole.Ask<double>($"[cyan]{label1}:[/] ");
                double input2 = AnsiConsole.Ask<double>($"[cyan]{label2}:[/] ");

                int prediction = perceptron.Predict(new double[] { input1, input2 });

                var resultPanel = new Panel($"[bold yellow]{prediction}[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Cyan)
                    .Header("[bold]Prediction[/]");

                AnsiConsole.Write(resultPanel);
                AnsiConsole.WriteLine();

                // Ask for feedback
                if (AnsiConsole.Confirm("[yellow]Was this prediction correct?[/]"))
                {
                    AnsiConsole.MarkupLine("[green]Great! 🎉[/]\n");
                }
                else
                {
                    int correctAnswer = AnsiConsole.Ask<int>("[cyan]What should the correct output be? (0 or 1):[/] ");
                    perceptron.TrainOnSingleSample(new double[] { input1, input2 }, correctAnswer);
                    AnsiConsole.WriteLine();
                }
                
                if (!AnsiConsole.Confirm("[yellow]Test another input?[/]", true))
                    break;
                
                AnsiConsole.WriteLine();
            }
        }
    }
}
