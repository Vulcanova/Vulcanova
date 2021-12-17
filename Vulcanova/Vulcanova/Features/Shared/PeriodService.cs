using System;
using ImTools;
using Vulcanova.Features.Auth;

namespace Vulcanova.Features.Shared
{
    public class PeriodService : IPeriodService
    {
        private readonly IAccountRepository _accountRepository;

        public PeriodService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public PeriodResult GetCurrentPeriod(int accountId)
        {
            var account = _accountRepository.GetById(accountId);
            var currentPeriod = account.GetCurrentPeriod();
            var index = account.Periods.IndexOf(currentPeriod);

            var hasNext = index < account.Periods.Count - 2;
            var hasPrevious = index > 0;

            return new PeriodResult(currentPeriod, hasNext, hasPrevious);
        }

        public PeriodResult ChangePeriod(int accountId, PeriodChangeDirection direction)
        {
            var account = _accountRepository.GetById(accountId);

            var currentPeriod = account.GetCurrentPeriod();
            var allPeriods = account.Periods.ToArray();
            
            var currentPeriodIndex = allPeriods.IndexOf(currentPeriod);

            var nextIndex = direction switch
            {
                PeriodChangeDirection.Next => Math.Min(currentPeriodIndex + 1, allPeriods.Length - 1),
                PeriodChangeDirection.Previous => Math.Max(currentPeriodIndex - 1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };

            var nextPeriod = account.Periods[nextIndex];

            currentPeriod.Current = false;
            nextPeriod.Current = true;

            var hasNext = nextIndex < account.Periods.Count - 1;
            var hasPrevious = nextIndex > 0;

            _accountRepository.UpdateAccount(account);

            return new PeriodResult(nextPeriod, hasNext, hasPrevious);
        }
    }
    
    public enum PeriodChangeDirection
    {
        Previous,
        Next
    }

    public record PeriodResult(Period CurrentPeriod, bool HasNext, bool HasPrevious);
}