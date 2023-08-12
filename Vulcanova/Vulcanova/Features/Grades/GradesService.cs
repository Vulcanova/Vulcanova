using System.Reactive.Linq;
using AutoMapper;
using Vulcanova.Core.Uonet;
using Vulcanova.Features.Auth.Accounts;
using Vulcanova.Uonet.Api;
using Vulcanova.Uonet.Api.Grades;

namespace Vulcanova.Features.Grades;

public class GradesService : UonetResourceProvider, IGradesService
{
    private readonly IApiClientFactory _apiClientFactory;
    private readonly IGradesRepository _gradesRepository;
    private readonly IMapper _mapper;

    public GradesService(
        IApiClientFactory apiClientFactory,
        IMapper mapper,
        IGradesRepository gradesRepository)
    {
        _apiClientFactory = apiClientFactory;
        _mapper = mapper;
        _gradesRepository = gradesRepository;
    }

    public IObservable<IEnumerable<Grade>> GetPeriodGrades(Account account, int periodId, bool forceSync = false)
    {
        return Observable.Create<IEnumerable<Grade>>(async observer =>
        {
            var normalGradesResourceKey = GetGradesResourceKey(account, periodId);
            var behaviourGradesResourceKey = GetBehaviourGradesResourceKey(account, periodId);

            var items = await _gradesRepository.GetGradesForPupilAsync(account.Id, account.Pupil.Id,
                periodId);

            observer.OnNext(items);

            if (ShouldSync(normalGradesResourceKey) || ShouldSync(behaviourGradesResourceKey) || forceSync)
            {
                var onlineGrades = await FetchPeriodGradesAsync(account, periodId);

                await _gradesRepository.UpdatePupilGradesAsync(onlineGrades);

                SetJustSynced(normalGradesResourceKey);
                SetJustSynced(behaviourGradesResourceKey);

                items = await _gradesRepository.GetGradesForPupilAsync(account.Id, account.Pupil.Id,
                    periodId);

                observer.OnNext(items);
            }

            observer.OnCompleted();
        });
    }

    private async Task<Grade[]> FetchPeriodGradesAsync(Account account, int periodId)
    {
        var client = await _apiClientFactory.GetAuthenticatedAsync(account);

        var normalGradesLastSync = GetLastSync(GetGradesResourceKey(account, periodId));

        var normalGradesQuery =
            new GetGradesByPupilQuery(account.Unit.Id, account.Pupil.Id, periodId, normalGradesLastSync, 500);

        var normalGrades = client.GetAllAsync(GetGradesByPupilQuery.ApiEndpoint,
            normalGradesQuery);

        var behaviourGradesLastSync = GetLastSync(GetGradesResourceKey(account, periodId));

        var behaviourGradesQuery =
            new GetBehaviourGradesByPupilQuery(account.Unit.Id, account.Pupil.Id, periodId, behaviourGradesLastSync,
                500);

        var behaviourGrades = client.GetAllAsync(GetBehaviourGradesByPupilQuery.ApiEndpoint,
            behaviourGradesQuery);

        var domainGrades = await normalGrades
            .Concat(behaviourGrades)
            .Select(_mapper.Map<Grade>)
            .ToArrayAsync();

        foreach (var grade in domainGrades)
        {
            grade.AccountId = account.Id;
        }

        return domainGrades;
    }

    private static string GetGradesResourceKey(Account account, int periodId)
        => $"Grades_{account.Id}_{account.Pupil.Id}_{periodId}";
        
    private static string GetBehaviourGradesResourceKey(Account account, int periodId)
        => $"BehaviourGrades_{account.Id}_{account.Pupil.Id}_{periodId}";

    protected override TimeSpan OfflineDataLifespan => TimeSpan.FromMinutes(15);
}