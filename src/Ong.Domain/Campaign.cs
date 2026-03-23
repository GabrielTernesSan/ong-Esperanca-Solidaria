using Ong.Domain.Enums;

namespace Ong.Domain
{
    public class Campaign
    {
        public const int MaxTitleLength = 100;
        public const int MaxDescriptionLength = 300;

        public Guid Id { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTimeOffset StartDate { get; private set; }
        public DateTimeOffset EndDate { get; private set; }
        public decimal FinancialGoal { get; private set; }
        public decimal CurrentAmount { get; private set; }
        public EStatusCampanha Status { get; private set; }

        public Campaign(string title, string description, DateTimeOffset startDate, DateTimeOffset endDate, decimal financialGoal, EStatusCampanha status)
        {
            if (title.Length > MaxTitleLength)
                throw new ArgumentException("Título não pode exceder 100 caracteres.", nameof(title));
            if (description.Length > MaxDescriptionLength)
                throw new ArgumentException("Descrição não pode exceder 300 caracteres.", nameof(description));

            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            FinancialGoal = financialGoal;
            Status = status;
            CurrentAmount = 0m;
        }

        public Campaign(Guid id, string title, string description, DateTimeOffset startDate, DateTimeOffset endDate, decimal financialGoal, EStatusCampanha status, decimal currentAmount)
        {
            Id = id;
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            FinancialGoal = financialGoal;
            Status = status;
            CurrentAmount = currentAmount;
        }

        public void UpdateStatus(EStatusCampanha newStatus) => Status = newStatus;

        public void UpdateDescription(string newDescription)
        {
            if (newDescription.Length > MaxDescriptionLength)
                throw new ArgumentException("Descrição não pode exceder 300 caracteres.", nameof(newDescription));

            Description = newDescription;
        }

        public void UpdateTitle(string newTitle)
        {
            if (newTitle.Length > MaxTitleLength)
                throw new ArgumentException("Título não pode exceder 100 caracteres.", nameof(newTitle));

            Title = newTitle;
        }

        public void UpdateDates(DateTimeOffset newStartDate, DateTimeOffset newEndDate)
        {
            StartDate = newStartDate;
            EndDate = newEndDate;
        }

        public void AddDonation(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Valor da doação deve ser maior que zero.");

            CurrentAmount += amount;

            if (CurrentAmount >= FinancialGoal)
                MarkAsCompleted();
        }

        public bool IsActive() => Status == EStatusCampanha.InProgress && DateTimeOffset.UtcNow >= StartDate && DateTimeOffset.UtcNow <= EndDate;

        public bool IsCompleted() => Status == EStatusCampanha.Completed || DateTimeOffset.UtcNow > EndDate;

        public bool InProgress() => Status == EStatusCampanha.InProgress && DateTimeOffset.UtcNow < EndDate;

        public void MarkAsCompleted()
        {
            Status = EStatusCampanha.Completed;

            EndDate = DateTimeOffset.UtcNow;
        }
    }
}
