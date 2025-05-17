using System;

public class UserEnteredNumberEventArgs : EventArgs
{
    public int Input { get; set; }
    public DateTime EnteredAt { get; set; }
}

public class UserInterface
{
    public event EventHandler<UserEnteredNumberEventArgs> UserEnteredNumber;

    public void WriteMessage(string message)
    {
        Console.WriteLine(message);
    }

    public string ReadInput()
    {
        var input = Console.ReadLine();
        if (int.TryParse(input, out int number))
        {
            UserEnteredNumber?.Invoke(this, new UserEnteredNumberEventArgs
            {
                Input = number,
                EnteredAt = DateTime.Now
            });
        }
        return input;
    }
}
