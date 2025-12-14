using System;
using PerceptronTrainer.Models;
using Spectre.Console;

namespace PerceptronTrainer.Data
{
    public static class ClassifierData
    {
        public static TrainingData GetFruitClassifier()
        {
            var trainingInputs = new double[][]
            {
                new double[] { 2, 8 },   // Lemon: low sweetness, yellow
                new double[] { 3, 3 },   // Lime: low sweetness, green
                new double[] { 4, 6 },   // Grapefruit: somewhat sweet, pink/yellow
                new double[] { 5, 5 },   // Orange (border case)
                new double[] { 8, 7 },   // Apple: sweet, red
                new double[] { 9, 8 },   // Banana: very sweet, yellow
                new double[] { 7, 9 },   // Strawberry: sweet, red
                new double[] { 8, 6 }    // Grape: sweet, purple
            };

            var trainingOutputs = new int[] { 0, 0, 0, 0, 1, 1, 1, 1 };

            AnsiConsole.WriteLine();
            var fruitTable = new Table()
                .Border(TableBorder.Rounded)
                .Title("[bold yellow]Training Fruits[/]");
            
            fruitTable.AddColumn("Fruit");
            fruitTable.AddColumn("Sweetness");
            fruitTable.AddColumn("Color Int.");
            fruitTable.AddColumn("Type");

            string[] fruitNames = { "🍋 Lemon", "🟢 Lime", "🍊 Grapefruit", "🍊 Orange", 
                                   "🍎 Apple", "🍌 Banana", "🍓 Strawberry", "🍇 Grape" };
            
            for (int i = 0; i < fruitNames.Length; i++)
            {
                string type = trainingOutputs[i] == 0 ? "[cyan]Citrus[/]" : "[green]Sweet[/]";
                fruitTable.AddRow(
                    fruitNames[i],
                    trainingInputs[i][0].ToString(),
                    trainingInputs[i][1].ToString(),
                    type);
            }
            
            AnsiConsole.Write(fruitTable);

            return new TrainingData(
                trainingInputs,
                trainingOutputs,
                "Fruit Classifier (Sweet=1 vs Citrus=0)",
                new[] { "Sweetness (0-10)", "Color Intensity (0-10)" },
                "Type (0=Citrus, 1=Sweet)"
            );
        }

        public static TrainingData GetTemperatureClassifier()
        {
            var trainingInputs = new double[][]
            {
                new double[] { 5, 60 },   // Cold winter day
                new double[] { 10, 70 },  // Cool spring day
                new double[] { 12, 50 },  // Mild day
                new double[] { 15, 55 },  // Border case
                new double[] { 28, 80 },  // Hot humid day
                new double[] { 32, 60 },  // Very hot dry day
                new double[] { 35, 75 },  // Extremely hot
                new double[] { 30, 50 }   // Hot summer day
            };

            var trainingOutputs = new int[] { 0, 0, 0, 0, 1, 1, 1, 1 };

            AnsiConsole.WriteLine();
            var tempTable = new Table()
                .Border(TableBorder.Rounded)
                .Title("[bold yellow]Training Weather Conditions[/]");
            
            tempTable.AddColumn("Condition");
            tempTable.AddColumn("Temp (°C)");
            tempTable.AddColumn("Humidity (%)");
            tempTable.AddColumn("Classification");

            string[] conditions = { "❄️  Cold Winter", "🌸 Cool Spring", "🍃 Mild", "☁️  Border", 
                                   "☀️  Hot Humid", "🔥 Very Hot", "🌡️  Extreme ", "🏖️  Summer " };
            
            for (int i = 0; i < conditions.Length; i++)
            {
                string type = trainingOutputs[i] == 0 ? "[cyan]Cold[/]" : "[red]Hot[/]";
                tempTable.AddRow(
                    conditions[i],
                    trainingInputs[i][0].ToString(),
                    trainingInputs[i][1].ToString(),
                    type);
            }
            
            AnsiConsole.Write(tempTable);

            return new TrainingData(
                trainingInputs,
                trainingOutputs,
                "Temperature Classifier (Hot=1 vs Cold=0)",
                new[] { "Temp (°C)", "Humidity (%)" },
                "Classification (0=Cold, 1=Hot)"
            );
        }

        public static TrainingData GetSizeClassifier()
        {
            var trainingInputs = new double[][]
            {
                new double[] { 1, 20 },   // Small item
                new double[] { 2, 30 },   // Small item
                new double[] { 3, 35 },   // Small-medium
                new double[] { 4, 40 },   // Medium
                new double[] { 15, 80 },  // Big item
                new double[] { 20, 90 },  // Big item
                new double[] { 25, 100 }, // Very big
                new double[] { 18, 85 }   // Big item
            };

            var trainingOutputs = new int[] { 0, 0, 0, 0, 1, 1, 1, 1 };

            AnsiConsole.WriteLine();
            var sizeTable = new Table()
                .Border(TableBorder.Rounded)
                .Title("[bold yellow]Training Objects[/]");
            
            sizeTable.AddColumn("Object");
            sizeTable.AddColumn("Weight (kg)");
            sizeTable.AddColumn("Height (cm)");
            sizeTable.AddColumn("Size");

            string[] objects = { "📦 Tiny Box", "📕 Book", "🎒 Backpack", "💼 Suitcase", 
                                "🛋️ Chair", "🚪 Door", "🗄️ Cabinet", "📺 TV" };
            
            for (int i = 0; i < objects.Length; i++)
            {
                string type = trainingOutputs[i] == 0 ? "[cyan]Small[/]" : "[green]Big[/]";
                sizeTable.AddRow(
                    objects[i],
                    trainingInputs[i][0].ToString(),
                    trainingInputs[i][1].ToString(),
                    type);
            }
            
            AnsiConsole.Write(sizeTable);

            return new TrainingData(
                trainingInputs,
                trainingOutputs,
                "Size Classifier (Big=1 vs Small=0)",
                new[] { "Weight (kg)", "Height (cm)" },
                "Size (0=Small, 1=Big)"
            );
        }

        public static TrainingData GetLogicGate(string gate)
        {
            var trainingInputs = new double[][]
            {
                new double[] { 0, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 }
            };

            var trainingOutputs = gate switch
            {
                "AND" => new int[] { 0, 0, 0, 1 },
                "OR" => new int[] { 0, 1, 1, 1 },
                "NAND" => new int[] { 1, 1, 1, 0 },
                "NOR" => new int[] { 1, 0, 0, 0 },
                _ => new int[] { 0, 0, 0, 1 }
            };

            return new TrainingData(
                trainingInputs,
                trainingOutputs,
                $"{gate} logic gate"
            );
        }
    }
}
