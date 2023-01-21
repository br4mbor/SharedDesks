using System.Text;
using Abb.Euopc.SharedDesks.Domain.Common;
using Abb.Euopc.SharedDesks.Domain.Entities;

namespace Abb.Euopc.SharedDesks.Application.Common;

public static class NotificationBuilder
{
    public static EmailMessage CreateReservationMessage(Reservation reservation, Desk desk)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"User {reservation.CreatedByEmail} has just created a reservation on Shared Desks for you!");
        sb.AppendLine($"Reservation info: ");
        sb.AppendLine($"Date: {reservation.Date.ToShortDateString()}");
        sb.AppendLine($"Desk number: {desk.Label}");
        sb.AppendLine($"Area: {desk.Area?.Name} (Floor {desk.Area?.Floor})");
        sb.AppendLine();
        sb.AppendLine("For further information, check your Reservations in Shared Desks application.");
        sb.AppendLine();
        sb.AppendLine($"This notification was sent automatically by the system. Please do not reply directly to this email.");

        return CreateNotificationMessage(reservation.ReservedForEmail, null, "New reservation", sb.ToString());
    }

    public static EmailMessage CreateDeskUpdateMessage(Desk desk, string reservedForEmail, DateTime[] dates)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Data associated with desk {desk.Label} you've reserved were updated");
        sb.AppendLine($"We found {dates.Length} reservations of this desk created for you.");
        foreach (var date in dates)
        {
            sb.AppendLine($"\t> {date.ToString("yyyy-MM-dd")}");
        }
        sb.AppendLine();
        sb.AppendLine($"New desk data:");
        sb.AppendLine($"\t> Number: {desk.Label}");
        sb.AppendLine($"\t> Area: {desk.Area?.Name} (Floor {desk.Area?.Floor})");
        //sb.AppendLine($"- Items:");
        //foreach (var item in desk.DeskItemsToDesks.Select(ditd => ditd.DeskItem))
        //{
        //    sb.AppendLine($"\t> {item.Type.Name} - {item.Name}");
        //}
        sb.AppendLine();
        sb.AppendLine("For further information, check your Reservations in Shared Desks application.");
        sb.AppendLine();
        sb.AppendLine("This notification was sent automatically by the system. Please do not reply directly to this email.");

        return CreateNotificationMessage(reservedForEmail, null, "Desk you've reserved was updated", sb.ToString());
    }

    public static EmailMessage CreateReservationCancelMessage(Reservation reservation)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Your reservation on {reservation.Date.ToShortDateString()} of desk {reservation.Desk?.Label} was cancelled");
        sb.AppendLine("Please check your reservations in Shared Desks application.");
        sb.AppendLine();
        sb.AppendLine("This notification was sent automatically by the system. Please do not reply directly to this email.");

        return CreateNotificationMessage(reservation.ReservedForEmail, reservation.CreatedByEmail, "Your reservation was cancelled", sb.ToString());
    }

    public static EmailMessage CreateNotificationMessage(string to, string? cc, string subject, string message)
    {
        return new EmailMessage()
        {
            To = to,
            Cc = to == cc ? null : cc,
            Subject = $"Shared Desks - {subject}",
            Message = message
        };
    }
}

