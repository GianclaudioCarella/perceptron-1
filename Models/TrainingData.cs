using System;

namespace PerceptronTrainer.Models
{
    public class TrainingData
    {
        public double[][] Inputs { get; set; }
        public int[] Outputs { get; set; }
        public string Name { get; set; }
        public string[]? InputLabels { get; set; }
        public string? OutputLabel { get; set; }

        public TrainingData(double[][] inputs, int[] outputs, string name, 
                           string[]? inputLabels = null, string? outputLabel = null)
        {
            Inputs = inputs;
            Outputs = outputs;
            Name = name;
            InputLabels = inputLabels;
            OutputLabel = outputLabel;
        }
    }
}
