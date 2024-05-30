using TicketSystem.Entities;
using TicketSystem.Repository;

namespace TicketSystem.Utils;

public class RandomNumberGeneratorNumber
{
    private static readonly HashSet<long> generatedNumbers = new();
    private static readonly Random random = new();

    public static long GenerateUnique6DigitNumber(IRepositoryWrapper repositoryWrapper)
    {
        long min = 1000;
        long max = 9999;

        long fourDigitNumber;
        long uniqueNumber;
        Ticket ticket;
        do
        {
            fourDigitNumber = (long)(random.NextDouble() * (max - min) + min);
            int dayOfYear = DateTime.Now.DayOfYear;
            uniqueNumber = long.Parse($"{fourDigitNumber}{dayOfYear}");

            ticket = repositoryWrapper.Ticket.Get(t => t.TicketNumber == uniqueNumber).Result;
        } while (generatedNumbers.Contains(fourDigitNumber) || ticket != null);

        generatedNumbers.Add(fourDigitNumber);

        return uniqueNumber;
    }
}