﻿using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Favorite.Resumes;

public class FavoriteResumesByIdSpec : IncludeSpec<FavoriteResumeModel>, IEntityById<FavoriteResumeModel>
{
    public FavoriteResumesByIdSpec(int id, int userId)
    {
        Id = id;
        UserId = userId;

        Query.Where(x => x.UserId.Equals(UserId));
        Query.Where(x => x.ResumeId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
    public int UserId { get; set; }
}