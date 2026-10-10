using System;

class Program
{
    static void Main()
    {
        // Global variable's
        int choice = 0;
        double num1, num2, result;
        int number, count;
        double largest, value;

        // loop runs until the user chooses 8 (Exit)
        while (choice != 8)
        {
            Console.WriteLine("===== Calculator Menu =====");
            Console.WriteLine("1. Addition");
            Console.WriteLine("2. Subtraction");
            Console.WriteLine("3. Multiplication");
            Console.WriteLine("4. Division");
            Console.WriteLine("5. Remainder");
            Console.WriteLine("6. Even/Odd Check");
            Console.WriteLine("7. Find Largest Number");
            Console.WriteLine("8. Exit");
            Console.Write("Enter your choice: ");
            choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                // Addition
                case 1:
                    Console.Write("Enter first number: ");
                    num1 = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Enter second number: ");
                    num2 = Convert.ToDouble(Console.ReadLine());
                    result = num1 + num2;
                    Console.WriteLine("Addition = " + result);
                    break;

                // Subtraction
                case 2:
                    Console.Write("Enter first number: ");
                    num1 = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Enter second number: ");
                    num2 = Convert.ToDouble(Console.ReadLine());
                    result = num1 - num2;
                    Console.WriteLine("Subtraction = " + result);
                    break;

                // Multiplication
                case 3:
                    Console.Write("Enter first number: ");
                    num1 = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Enter second number: ");
                    num2 = Convert.ToDouble(Console.ReadLine());
                    result = num1 * num2;
                    Console.WriteLine("Multiplication = " + result);
                    break;

                // Division
                case 4:
                    Console.Write("Enter first number: ");
                    num1 = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Enter second number: ");
                    num2 = Convert.ToDouble(Console.ReadLine());
                    if (num2 == 0)
                    {
                        Console.WriteLine("Cannot divide by zero!");
                    }
                    else
                    {
                        result = num1 / num2;
                        Console.WriteLine("Division = " + result);
                    }
                    break;

                // Remainder
                case 5:
                    Console.Write("Enter first number: ");
                    num1 = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Enter second number: ");
                    num2 = Convert.ToDouble(Console.ReadLine());
                    // check divide by zero
                    if (num2 == 0)
                    {
                        Console.WriteLine("Cannot divide by zero!");
                    }
                    else
                    {
                        result = num1 % num2;
                        Console.WriteLine("Remainder = " + result);
                    }
                    break;

                // Even/Odd check
                case 6:
                    Console.Write("Enter a number: ");
                    number = Convert.ToInt32(Console.ReadLine());
                    if (number % 2 == 0)
                    {
                        Console.WriteLine(number + " is Even");
                    }
                    else
                    {
                        Console.WriteLine(number + " is Odd");
                    }
                    break;

                // Find largest number
                case 7:
                    Console.Write("How many numbers? ");
                    count = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter number 1: ");
                    largest = Convert.ToDouble(Console.ReadLine());

                    // check the remaining numbers
                    for (int i = 2; i <= count; i++)
                    {
                        Console.Write("Enter number " + i + ": ");
                        value = Convert.ToDouble(Console.ReadLine());
                        if (value > largest)
                        {
                            largest = value;
                        }
                    }
                    Console.WriteLine("Largest number = " + largest);
                    break;

                // Exit
                case 8:
                    Console.WriteLine("Program closed. Goodbye!");
                    break;

                // wrong choice
                default:
                    Console.WriteLine("Invalid choice! Please enter 1 to 8.");
                    break;
            }

            Console.WriteLine();
        }
    }
}