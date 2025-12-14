using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== SINGLE LAYER PERCEPTRON ===\n");

        // Example: AND logic gate
        Console.WriteLine("Training for AND logic gate:\n");

        double[][] trainingInputs = new double[][]
        {
            new double[] { 0, 0 },
            new double[] { 0, 1 },
            new double[] { 1, 0 },
            new double[] { 1, 1 }
        };

        int[] trainingOutputs = new int[] { 0, 0, 0, 1 };

        Perceptron perceptron = new Perceptron(inputSize: 2, learningRate: 0.1);
        perceptron.Train(trainingInputs, trainingOutputs, epochs: 100);

        // Testing
        Console.WriteLine("=== Testing the Perceptron ===\n");
        for (int i = 0; i < trainingInputs.Length; i++)
        {
            int prediction = perceptron.Predict(trainingInputs[i]);
            Console.WriteLine($"Input: [{trainingInputs[i][0]}, {trainingInputs[i][1]}] " +
                            $"→ Output: {prediction} (Expected: {trainingOutputs[i]})");
        }

        Console.WriteLine("\n=== End ===");
    }
}
