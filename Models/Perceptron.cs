using System;
using Spectre.Console;

namespace PerceptronTrainer.Models
{
    public class Perceptron
    {
        private double[] weights;
        private double bias;
        private double learningRate;

        public Perceptron(int inputSize, double learningRate = 0.1)
        {
            this.learningRate = learningRate;
            this.weights = new double[inputSize];
            this.bias = 0;

            // Initialize weights randomly
            Random random = new Random();
            for (int i = 0; i < inputSize; i++)
            {
                weights[i] = random.NextDouble() * 2 - 1; // Between -1 and 1
            }
            bias = random.NextDouble() * 2 - 1;
        }

        // Activation function (step function)
        private int Activate(double sum)
        {
            return sum >= 0 ? 1 : 0;
        }

        // Prediction
        public int Predict(double[] inputs)
        {
            double sum = bias;
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += inputs[i] * weights[i];
            }
            return Activate(sum);
        }

        // Training (verbose mode with step-by-step details)
        public void Train(double[][] trainingInputs, int[] trainingOutputs, int epochs, bool verbose = true)
        {
            if (verbose)
            {
                AnsiConsole.WriteLine();
                var trainingRule = new Rule("[green bold]Starting Training[/]").RuleStyle("green");
                AnsiConsole.Write(trainingRule);
                AnsiConsole.WriteLine();

                // Show initial weights
                AnsiConsole.MarkupLine("[cyan]Initial Weights:[/]");
                for (int i = 0; i < weights.Length; i++)
                {
                    AnsiConsole.MarkupLine($"  w{i} = [yellow]{weights[i]:F4}[/]");
                }
                AnsiConsole.MarkupLine($"  bias = [yellow]{bias:F4}[/]\n");
            }

            int finalEpoch = 0;
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                if (verbose)
                {
                    AnsiConsole.MarkupLine($"[underline blue]Epoch {epoch}[/]");
                }
                
                int errors = 0;

                for (int i = 0; i < trainingInputs.Length; i++)
                {
                    int prediction = Predict(trainingInputs[i]);
                    int error = trainingOutputs[i] - prediction;

                    if (verbose)
                    {
                        // Calculate prediction step by step for display
                        double sum = bias;
                        AnsiConsole.Write($"  Sample {i}: [{trainingInputs[i][0]}, {trainingInputs[i][1]}] → ");
                        
                        string calculation = $"sum = {bias:F4}";
                        for (int j = 0; j < weights.Length; j++)
                        {
                            sum += trainingInputs[i][j] * weights[j];
                            calculation += $" + ({trainingInputs[i][j]} × {weights[j]:F4})";
                        }

                        AnsiConsole.Write($"pred=");
                        AnsiConsole.Markup($"[yellow]{prediction}[/]");
                        AnsiConsole.Write($", expected=");
                        AnsiConsole.Markup($"[cyan]{trainingOutputs[i]}[/]");

                        if (error != 0)
                        {
                            AnsiConsole.MarkupLine($" [red]✗ ERROR={error}[/]");
                            AnsiConsole.MarkupLine($"    {calculation} = {sum:F4}");
                            AnsiConsole.MarkupLine($"    [grey]Updating weights...[/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine($" [green]✓[/]");
                        }
                    }

                    if (error != 0)
                    {
                        errors++;
                        // Update weights
                        for (int j = 0; j < weights.Length; j++)
                        {
                            if (verbose)
                            {
                                double oldWeight = weights[j];
                                weights[j] += learningRate * error * trainingInputs[i][j];
                                AnsiConsole.MarkupLine($"      w{j}: {oldWeight:F4} → [yellow]{weights[j]:F4}[/]");
                            }
                            else
                            {
                                weights[j] += learningRate * error * trainingInputs[i][j];
                            }
                        }
                        
                        if (verbose)
                        {
                            double oldBias = bias;
                            bias += learningRate * error;
                            AnsiConsole.MarkupLine($"      bias: {oldBias:F4} → [yellow]{bias:F4}[/]");
                        }
                        else
                        {
                            bias += learningRate * error;
                        }
                    }
                }

                if (verbose)
                {
                    AnsiConsole.MarkupLine($"  [bold]Epoch {epoch} Summary: {errors} errors[/]\n");
                }
                else if (epoch % 10 == 0 || errors == 0)
                {
                    // Show progress every 10 epochs or when converged in non-verbose mode
                    AnsiConsole.MarkupLine($"[grey]Epoch {epoch}: {errors} errors[/]");
                }

                finalEpoch = epoch;
                if (errors == 0)
                {
                    if (verbose)
                        AnsiConsole.MarkupLine($"[green bold]✓ Converged at epoch {epoch}![/]\n");
                    else
                        AnsiConsole.MarkupLine($"[green]✓ Training complete! Converged at epoch {epoch}[/]\n");
                    break;
                }
            }

            // Show final weights
            if (verbose)
            {
                var weightsTable = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Green)
                    .Title("[bold green]Final Weights[/]");
                
                weightsTable.AddColumn("[yellow]Parameter[/]");
                weightsTable.AddColumn("[yellow]Value[/]");
                
                for (int i = 0; i < weights.Length; i++)
                {
                    weightsTable.AddRow($"w{i}", $"{weights[i]:F4}");
                }
                weightsTable.AddRow("bias", $"{bias:F4}");
                
                AnsiConsole.Write(weightsTable);
                AnsiConsole.WriteLine();
            }
        }

        // Train on a single sample with feedback (multiple iterations for better learning)
        public void TrainOnSingleSample(double[] input, int correctOutput)
        {
            int prediction = Predict(input);
            int initialError = correctOutput - prediction;

            if (initialError != 0)
            {
                // Train multiple times on this sample to reinforce learning
                int iterations = 5;
                for (int iter = 0; iter < iterations; iter++)
                {
                    prediction = Predict(input);
                    int error = correctOutput - prediction;
                    
                    if (error == 0)
                        break; // Already learned correctly
                    
                    // Update weights
                    for (int j = 0; j < weights.Length; j++)
                    {
                        weights[j] += learningRate * error * input[j];
                    }
                    bias += learningRate * error;
                }

                // Verify learning
                int finalPrediction = Predict(input);
                if (finalPrediction == correctOutput)
                {
                    AnsiConsole.MarkupLine($"[green]✓ Learned! Now predicting correctly.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]⚠ Partially learned. May need more examples.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine($"[grey]Already correct! No update needed.[/]");
            }
        }
    }
}
