using Ardalis.Specification;
using domain.Models.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace domain.Specifications.Message;

public class MessageByIdSpec : IncludeSpec<MessageModel>
{
    public MessageByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.MessageId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}
