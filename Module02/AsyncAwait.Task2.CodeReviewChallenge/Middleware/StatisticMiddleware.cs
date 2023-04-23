using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwait.Task2.CodeReviewChallenge.Headers;
using CloudServices.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AsyncAwait.Task2.CodeReviewChallenge.Middleware;

public class StatisticMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IStatisticService _statisticService;

    private readonly IDictionary<string, long> pageVisitCounts = new Dictionary<string, long>();

    public StatisticMiddleware(RequestDelegate next, IStatisticService statisticService)
    {
        _next = next;
        _statisticService = statisticService ?? throw new ArgumentNullException(nameof(statisticService));
    }

    private void IncrementPageVisitCount(string path)
    {
        if (!pageVisitCounts.ContainsKey(path))
        {
            pageVisitCounts[path] = 1;
        }
        else
        {
            pageVisitCounts[path]++;
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path;

        IncrementPageVisitCount(path);

        _statisticService.RegisterVisitAsync(path);
        await UpdateHeaders(context, path);

        await _next(context);
    }

    private async Task UpdateHeaders(HttpContext context, string path)
    {
        var visits = await _statisticService.GetVisitsCountAsync(path);
        var realVisits = visits != pageVisitCounts[path] ? pageVisitCounts[path] : visits;
        context.Response.Headers.Add(
            CustomHttpHeaders.TotalPageVisits,
            realVisits.ToString());
    }
}
