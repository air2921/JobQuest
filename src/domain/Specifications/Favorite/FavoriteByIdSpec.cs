﻿using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Favorite;

public class FavoriteByIdSpec : IncludeSpec<FavoriteModel>, IEntityById<FavoriteModel>
{
    public FavoriteByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.FavoriteId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}
