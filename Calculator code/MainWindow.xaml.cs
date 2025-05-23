using System;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        private string currentInput = "0";
        private double lastValue = 0;
        private string lastOperator = "";
        private bool isNewEntry = true;

        public MainWindow()
        {
            InitializeComponent();
            UpdateDisplay();
            UpdateHistoryDisplay();
        }

        private void UpdateDisplay()
        {
            OutputTextBlock.Text = currentInput;
        }

        private void UpdateHistoryDisplay()
        {
            if (!string.IsNullOrEmpty(lastOperator))
                HistoryTextBlock.Text = $"{lastValue} {lastOperator}";
            else
                HistoryTextBlock.Text = string.Empty;
        }

        // Number buttons 0-9 and decimal point
        private void NumBtn(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            string num = btn.Content.ToString();

            if (isNewEntry)
            {
                currentInput = (num == ".") ? "0." : num;
                isNewEntry = false;
            }
            else
            {
                if (num == "." && currentInput.Contains(".")) return; // no multiple decimals
                currentInput += num;
            }

            UpdateDisplay();
            UpdateHistoryDisplay();
        }

        // Operator buttons +, -, ×, ÷
        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            string op = btn.Content.ToString();

            CalculateLastOperation();

            lastOperator = op;
            isNewEntry = true;

            UpdateDisplay();
            UpdateHistoryDisplay();
        }

        // Calculate the last operation if applicable
        private void CalculateLastOperation()
        {
            if (!double.TryParse(currentInput, out double currentValue)) return;

            if (string.IsNullOrEmpty(lastOperator))
            {
                lastValue = currentValue;
            }
            else
            {
                switch (lastOperator)
                {
                    case "+":
                        lastValue += currentValue;
                        break;
                    case "−":
                    case "-":
                        lastValue -= currentValue;
                        break;
                    case "×":
                    case "*":
                        lastValue *= currentValue;
                        break;
                    case "÷":
                    case "/":
                        if (currentValue != 0)
                            lastValue /= currentValue;
                        else
                        {
                            MessageBox.Show("Cannot divide by zero");
                            lastValue = 0;
                        }
                        break;
                }
            }

            currentInput = lastValue.ToString();
        }

        // Equals button
        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            CalculateLastOperation();
            lastOperator = string.Empty;
            isNewEntry = true;

            UpdateDisplay();
            UpdateHistoryDisplay();
        }

        // Clear button
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            currentInput = "0";
            lastValue = 0;
            lastOperator = string.Empty;
            isNewEntry = true;

            UpdateDisplay();
            UpdateHistoryDisplay();
        }

        // +/- button
        private void PlusMinus_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(currentInput, out double value))
            {
                value = -value;
                currentInput = value.ToString();
                UpdateDisplay();
                UpdateHistoryDisplay();
            }
        }

        // Square root button
        private void Sqrt_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(currentInput, out double value))
            {
                if (value < 0)
                {
                    MessageBox.Show("Cannot take square root of a negative number.");
                    return;
                }
                value = Math.Sqrt(value);
                currentInput = value.ToString();
                isNewEntry = true;
                UpdateDisplay();
                UpdateHistoryDisplay();
            }
        }

        // Square button
        private void Square_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(currentInput, out double value))
            {
                value *= value;
                currentInput = value.ToString();
                isNewEntry = true;
                UpdateDisplay();
                UpdateHistoryDisplay();
            }
        }
    }
}
