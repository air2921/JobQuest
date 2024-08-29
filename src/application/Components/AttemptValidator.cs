﻿using datahub.Redis;
using domain.Abstractions;
using System;
using System.Text;
using System.Threading.Tasks;

namespace application.Components;

public class AttemptValidator(IDataCache<ConnectionSecondary> dataCache)
{
    public async Task<bool> IsValidTry(string src, int acceptableAttempts = 10)
    {
        var encoded = Convert.ToBase64String(Encoding.UTF32.GetBytes(src));
        var attemptsCount = await dataCache.GetSingleAsync<int>(encoded);

        if (attemptsCount > acceptableAttempts)
            return false;

        return true;
    }

    public async Task AddAttempt(string src, int minutesDelay = 15)
    {
        var encoded = Convert.ToBase64String(Encoding.UTF32.GetBytes(src));
        var attemptsCount = await dataCache.GetSingleAsync<int>(encoded);

        await dataCache.SetAsync(encoded, attemptsCount + 1, TimeSpan.FromMinutes(minutesDelay));
    }
}