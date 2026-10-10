using System;

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Show all functionalities
            string funtionalities = """
                ===== Calculator Menu =====
                1. Addition 
                2. Subtraction 
                3. Multiplication 
                4. Division 
                5. Remainder 
                6. Even/Odd Check 
                7. Find Largest Number 
                8. Exit
                """;
            Console.WriteLine(funtionalities);
            Console.Write("Enter: ");
            string userInput = Console.ReadLine();

            // Check data is blank or not
            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("You don't enter any values. Please enter a valid number (1-8). Thank you!");
            }
            else
            {
                // Check input was letter or number
                if (int.TryParse(userInput, out int value))
                {
                    string resultMessage = value switch
                    {
                        1 => "One",
                        2 => "Two",
                        3 => "Three",
                        4 => "Four",
                        5 => "Five",
                        6 => "Six",
                        7 => "Seven",
                        8 => "Eight",
                        _ => "Enter 1 to 8 values. Thank You!"
                    };
                    Console.WriteLine($"\n{resultMessage}");
                }
                else
                {
                    Console.WriteLine("You can't use any letter's. Please enter a valid number (1-8). Thank you!");
                }
            }
        }
    }
}