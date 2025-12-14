class Perceptron
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

    // Training
    public void Train(double[][] trainingInputs, int[] trainingOutputs, int epochs)
    {
        Console.WriteLine("=== Starting Training ===\n");

        for (int epoch = 0; epoch < epochs; epoch++)
        {
            int errors = 0;

            for (int i = 0; i < trainingInputs.Length; i++)
            {
                int prediction = Predict(trainingInputs[i]);
                int error = trainingOutputs[i] - prediction;

                if (error != 0)
                {
                    errors++;
                    // Update weights
                    for (int j = 0; j < weights.Length; j++)
                    {
                        weights[j] += learningRate * error * trainingInputs[i][j];
                    }
                    bias += learningRate * error;
                }
            }

            if (epoch % 10 == 0 || errors == 0)
            {
                Console.WriteLine($"Epoch {epoch}: {errors} errors");
            }

            if (errors == 0)
            {
                Console.WriteLine($"\n✓ Converged at epoch {epoch}!\n");
                break;
            }
        }

        Console.WriteLine("Final weights:");
        for (int i = 0; i < weights.Length; i++)
        {
            Console.WriteLine($"  w{i} = {weights[i]:F4}");
        }
        Console.WriteLine($"  bias = {bias:F4}\n");
    }
}
